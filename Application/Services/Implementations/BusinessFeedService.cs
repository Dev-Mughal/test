using Application.Services.Interfaces;
using Application.Utilities.Cache;
using Common.Features.Business.DTOs;
using Infrastructure;
using Infrastructure.Extensions;
using Infrastructure.Pagination;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Application.Services.Implementations
{
    public class BusinessFeedService(
        IDbContextFactory<BizyPopDbContext> contextFactory,
        IImageService imageService,
        IHttpContextAccessor httpContextAccessor,
        ICacheService cache) : IBusinessFeedService
    {
        // 1 degree of latitude is always ~69.0 miles.
        private const double MilesPerDegreeLat = 69.0;
        private const double DefaultRadiusMiles = 15.0;

        // Feed results are time-sensitive (active coupons expire) but DB-heavy.
        // A 2-minute TTL delivers a meaningful cache hit rate while keeping
        // data fresh enough for coupon validity.
        private static readonly TimeSpan FeedTtl = TimeSpan.FromMinutes(2);

        // ?????????????????????????????????????????????????????????????????????
        // PUBLIC: FEED ENTRY POINT
        // Routes the request to the correct query mode based on what the caller
        // has provided: free-text search ? location radius ? timezone ? empty.
        // ?????????????????????????????????????????????????????????????????????

        public Task<PaginationResponse<BusinessFeedItemDto>> GetBusinessFeedAsync(
            BusinessFeedRequest request,
            string? timeZone)
        {
            // Resolve the cache key before dispatching to a query mode so that
            // identical requests share a single cache entry regardless of call order.
            if (!string.IsNullOrWhiteSpace(request.Search))
            {
                var key = CacheKeys.BusinessFeedSearch(request.Search, request.CategoryId, request.PageNumber, request.PageSize);
                return cache.GetOrCreateAsync(key, () => ExecuteSearchModeAsync(request), FeedTtl);
            }

            if (!string.IsNullOrWhiteSpace(request.LocationKey))
            {
                var key = CacheKeys.BusinessFeedGeoByLocationKey(request.LocationKey, request.CategoryId, request.PageNumber, request.PageSize);
                return cache.GetOrCreateAsync(key, () => ExecuteGeoModeAsync(request), FeedTtl);
            }

            if (request.Lat.HasValue && request.Lng.HasValue)
            {
                var radius = request.RadiusMiles ?? DefaultRadiusMiles;
                var key = CacheKeys.BusinessFeedLocation(request.Lat.Value, request.Lng.Value, radius, request.CategoryId, request.PageNumber, request.PageSize);
                return cache.GetOrCreateAsync(key, () => ExecuteLocationModeAsync(request), FeedTtl);
            }

            if (!string.IsNullOrWhiteSpace(timeZone) &&
                !timeZone.Equals("UTC", StringComparison.OrdinalIgnoreCase))
            {
                var key = CacheKeys.BusinessFeedTimeZone(timeZone, request.CategoryId, request.PageNumber, request.PageSize);
                return cache.GetOrCreateAsync(key, () => ExecuteTimeZoneModeAsync(request, timeZone), FeedTtl);
            }

            return Task.FromResult(EmptyPage(request));
        }


        // ?????????????????????????????????????????????????????????????????????
        // PRIVATE: SEARCH MODE
        // Title-only text match on business name. City/State/Zip filtering is
        // handled by the dedicated geo mode. Category filter is respected.
        // Location radius is NOT applied in this mode.
        // ?????????????????????????????????????????????????????????????????????



        private async Task<PaginationResponse<BusinessFeedItemDto>> ExecuteSearchModeAsync(
            BusinessFeedRequest request)
        {
            var term = request.Search!.ToLower();

            // Capture once so the same moment is used in both the WHERE gate
            // and the badge count — avoids subtle clock drift across sub-queries.
            var utcNow = DateTime.UtcNow;

            var result = await contextFactory.PaginateAsync(
                context => context.Businesses
                    .AsNoTracking()
                    .Where(b =>
                        // A business with no coupons, or only expired ones, has
                        // nothing to offer — exclude it from every search result.
                        b.Coupons.Any(c => c.IsActive && c.EndDateTime >= utcNow) &&
                        // Title-only match; city/state/zip are handled by dedicated geo params
                        b.BusinessName.ToLower().Contains(term) &&
                        // Optional category narrow-down works together with search
                        (request.CategoryId == null || b.CategoryId == request.CategoryId))
                    .OrderBy(b => b.BusinessName)
                    .Select(b => new BusinessFeedItemDto(
                        b.BusinessId,
                        b.BusinessName,
                        b.Category.CategoryName,
                        b.StreetAddress + ", " + b.GeoCity.City + ", " + b.GeoCity.State + " " + b.GeoZipCode.ZipCode,
                        b.AddressLine2,
                        b.Coupons
                            .Where(c => c.IsActive && c.IsFeatured)
                            .OrderByDescending(c => c.CreatedOn)
                            .Select(c => c.Title)
                            .FirstOrDefault(),
                        // Prefer featured coupon photo; fall back to any active non-expired photo
                        b.Coupons
                            .Where(c => c.IsActive && c.IsFeatured)
                            .OrderByDescending(c => c.CreatedOn)
                            .Select(c => c.PhotoUrl)
                            .FirstOrDefault()
                        ??
                        b.Coupons
                            .Where(c => c.IsActive && c.EndDateTime >= utcNow && c.PhotoUrl != null)
                            .OrderByDescending(c => c.CreatedOn)
                            .Select(c => c.PhotoUrl)
                            .FirstOrDefault(),
                        b.Coupons.Count(c => c.IsActive && c.EndDateTime >= utcNow),
                        b.Latitude,
                        b.Longitude,
                        b.BusinessImageUrl,
                        null)),
                request)
                .ConfigureAwait(false);

            return EnrichFeedWithFullUrls(result);
        }

        // ?????????????????????????????????????????????????????????????????????
        // PRIVATE: GEO MODE
        // Filters businesses based on the single selected LocationKey.
        // LocationKey format is "C-{GeoCityId}" or "Z-{GeoZipCodeId}".
        // ?????????????????????????????????????????????????????????????????????

        private async Task<PaginationResponse<BusinessFeedItemDto>> ExecuteGeoModeAsync(
            BusinessFeedRequest request)
        {
            var locationKey = request.LocationKey?.Trim();
            
            long? geoCityId = null;
            long? geoZipId = null;

            if (locationKey != null)
            {
                if (locationKey.StartsWith("C-", StringComparison.OrdinalIgnoreCase) && long.TryParse(locationKey.Substring(2), out var cId))
                {
                    geoCityId = cId;
                }
                else if (locationKey.StartsWith("Z-", StringComparison.OrdinalIgnoreCase) && long.TryParse(locationKey.Substring(2), out var zId))
                {
                    geoZipId = zId;
                }
            }

            // Capture once so the same moment is used in both the WHERE gate
            // and the badge count  avoids subtle clock drift across sub-queries.
            var utcNow = DateTime.UtcNow;

            // Paginate businesses whose geo FKs match the selected ID.
            var result = await contextFactory.PaginateAsync(
                context => context.Businesses
                    .AsNoTracking()
                    .Where(b =>
                        b.Coupons.Any(c => c.IsActive && c.EndDateTime >= utcNow) &&
                        (geoCityId == null || b.StateCityId == geoCityId) &&
                        (geoZipId == null || b.StateCityZipId == geoZipId) &&
                        (request.CategoryId == null || b.CategoryId == request.CategoryId))
                    .OrderBy(b => b.BusinessName)
                    .Select(b => new BusinessFeedItemDto(
                        b.BusinessId,
                        b.BusinessName,
                        b.Category.CategoryName,
                        b.StreetAddress + ", " + b.GeoCity.City + ", " + b.GeoCity.State + " " + b.GeoZipCode.ZipCode,
                        b.AddressLine2,
                        b.Coupons
                            .Where(c => c.IsActive && c.IsFeatured)
                            .OrderByDescending(c => c.CreatedOn)
                            .Select(c => c.Title)
                            .FirstOrDefault(),
                        // Prefer featured coupon photo; fall back to any active non-expired photo
                        b.Coupons
                            .Where(c => c.IsActive && c.IsFeatured)
                            .OrderByDescending(c => c.CreatedOn)
                            .Select(c => c.PhotoUrl)
                            .FirstOrDefault()
                        ??
                        b.Coupons
                            .Where(c => c.IsActive && c.EndDateTime >= utcNow && c.PhotoUrl != null)
                            .OrderByDescending(c => c.CreatedOn)
                            .Select(c => c.PhotoUrl)
                            .FirstOrDefault(),
                        b.Coupons.Count(c => c.IsActive && c.EndDateTime >= utcNow),
                        b.Latitude,
                        b.Longitude,
                        b.BusinessImageUrl,
                        null)),
                request)
                .ConfigureAwait(false);

            return EnrichFeedWithFullUrls(result);
        }

        // ?????????????????????????????????????????????????????????????????????
        // PRIVATE: LOCATION MODE
        // A bounding-box pre-filter narrows candidates using lat/lng column
        // indexes (fast). Distance is then computed in the SELECT via a
        // Euclidean approximation. Radius defaults to 15 miles when not supplied.
        // Add PostGIS / NetTopologySuite for sub-mile precision if needed.
        // ?????????????????????????????????????????????????????????????????????

        private async Task<PaginationResponse<BusinessFeedItemDto>> ExecuteLocationModeAsync(
            BusinessFeedRequest request)
        {
            var (lat, lng) = (request.Lat!.Value, request.Lng!.Value);

            // Use client-supplied radius; fall back to 15-mile default when omitted.
            // Longitude degrees per mile decreases towards the poles.
            var radius = request.RadiusMiles ?? DefaultRadiusMiles;
            var latDelta = radius / MilesPerDegreeLat;
            var lngDelta = radius / (MilesPerDegreeLat * Math.Cos(lat * Math.PI / 180.0));

            var minLat = lat - latDelta;
            var maxLat = lat + latDelta;
            var minLng = lng - lngDelta;
            var maxLng = lng + lngDelta;

            // Capture once so the same moment is used in both the WHERE gate
            // and the badge count — avoids subtle clock drift across sub-queries.
            var utcNow = DateTime.UtcNow;

            var result = await contextFactory.PaginateAsync(
                context => context.Businesses
                    .AsNoTracking()
                    .Where(b =>
                        // A business with no coupons, or only expired ones, has
                        // nothing to offer — exclude it from nearby results.
                        b.Coupons.Any(c => c.IsActive && c.EndDateTime >= utcNow) &&
                        // ? Bounding-box pre-filter — hits lat/lng column indexes
                        b.Latitude >= minLat && b.Latitude <= maxLat &&
                        b.Longitude >= minLng && b.Longitude <= maxLng &&
                        // ? Optional category filter layered on top of the radius
                        (request.CategoryId == null || b.CategoryId == request.CategoryId))
                    // Sort closest first using Euclidean distance in miles
                    .OrderBy(b =>
                        Math.Sqrt(
                            Math.Pow((b.Latitude - lat) * MilesPerDegreeLat, 2) +
                            Math.Pow((b.Longitude - lng) * (MilesPerDegreeLat * Math.Cos(lat * Math.PI / 180.0)), 2)))
                    .Select(b => new BusinessFeedItemDto(
                        b.BusinessId,
                        b.BusinessName,
                        b.Category.CategoryName,
                        b.StreetAddress + ", " + b.GeoCity.City + ", " + b.GeoCity.State + " " + b.GeoZipCode.ZipCode,
                        b.AddressLine2,
                        b.Coupons
                            .Where(c => c.IsActive && c.IsFeatured)
                            .OrderByDescending(c => c.CreatedOn)
                            .Select(c => c.Title)
                            .FirstOrDefault(),
                        // Prefer featured coupon photo; fall back to any active non-expired photo
                        b.Coupons
                            .Where(c => c.IsActive && c.IsFeatured)
                            .OrderByDescending(c => c.CreatedOn)
                            .Select(c => c.PhotoUrl)
                            .FirstOrDefault()
                        ??
                        b.Coupons
                            .Where(c => c.IsActive && c.EndDateTime >= utcNow && c.PhotoUrl != null)
                            .OrderByDescending(c => c.CreatedOn)
                            .Select(c => c.PhotoUrl)
                            .FirstOrDefault(),
                        b.Coupons.Count(c => c.IsActive && c.EndDateTime >= utcNow),
                        b.Latitude,
                        b.Longitude,
                        b.BusinessImageUrl,
                        Math.Sqrt(
                            Math.Pow((b.Latitude - lat) * MilesPerDegreeLat, 2) +
                            Math.Pow((b.Longitude - lng) * (MilesPerDegreeLat * Math.Cos(lat * Math.PI / 180.0)), 2))))
                , request)
                .ConfigureAwait(false);

            return EnrichFeedWithFullUrls(result);
        }

        // ?????????????????????????????????????????????????????????????????????
        // PRIVATE: TIMEZONE MODE
        // No GPS available. Filter by businesses that have at least one coupon
        // valid right now in the user's local timezone so deals are time-relevant.
        // Ordered by the number of currently-active coupons (most active first).
        // ?????????????????????????????????????????????????????????????????????

        private async Task<PaginationResponse<BusinessFeedItemDto>> ExecuteTimeZoneModeAsync(
            BusinessFeedRequest request,
            string timeZone)
        {
            // Resolve current wall-clock time in the user's timezone.
            // Falls back to UTC if the IANA string is unrecognised.
            DateTime localNow;
            try
            {
                var tz = TimeZoneInfo.FindSystemTimeZoneById(timeZone);
                localNow = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, tz);
            }
            catch
            {
                localNow = DateTime.UtcNow;
            }

            var result = await contextFactory.PaginateAsync(
                context => context.Businesses
                    .AsNoTracking()
                    .Where(b =>
                        // Only businesses with at least one coupon live right now
                        b.Coupons.Any(c =>
                            c.IsActive &&
                            c.StartDateTime <= localNow &&
                            c.EndDateTime >= localNow) &&
                        // Optional category filter
                        (request.CategoryId == null || b.CategoryId == request.CategoryId))
                    // Rank by how many live coupons the business has
                    .OrderByDescending(b =>
                        b.Coupons.Count(c =>
                            c.IsActive &&
                            c.StartDateTime <= localNow &&
                            c.EndDateTime >= localNow))
                    .Select(b => new BusinessFeedItemDto(
                        b.BusinessId,
                        b.BusinessName,
                        b.Category.CategoryName,
                        b.StreetAddress + ", " + b.GeoCity.City + ", " + b.GeoCity.State + " " + b.GeoZipCode.ZipCode,
                        b.AddressLine2,
                        b.Coupons
                            .Where(c => c.IsActive && c.IsFeatured)
                            .OrderByDescending(c => c.CreatedOn)
                            .Select(c => c.Title)
                            .FirstOrDefault(),
                        // Prefer featured coupon photo; fall back to any active non-expired photo
                        b.Coupons
                            .Where(c => c.IsActive && c.IsFeatured)
                            .OrderByDescending(c => c.CreatedOn)
                            .Select(c => c.PhotoUrl)
                            .FirstOrDefault()
                        ??
                        b.Coupons
                            .Where(c => c.IsActive && c.EndDateTime >= localNow && c.PhotoUrl != null)
                            .OrderByDescending(c => c.CreatedOn)
                            .Select(c => c.PhotoUrl)
                            .FirstOrDefault(),
                        b.Coupons.Count(c => c.IsActive && c.EndDateTime >= localNow),
                        b.Latitude,
                        b.Longitude,
                        b.BusinessImageUrl,
                        null)),
                request)
                .ConfigureAwait(false);

            return EnrichFeedWithFullUrls(result);
        }

        // ?????????????????????????????????????????????????????????????????????
        // PRIVATE: URL ENRICHMENT
        // Converts stored relative paths to full absolute URLs so API clients
        // can use them directly without building the base URL themselves.
        // ?????????????????????????????????????????????????????????????????????

        private PaginationResponse<BusinessFeedItemDto> EnrichFeedWithFullUrls(
            PaginationResponse<BusinessFeedItemDto> result)
        {
            var httpRequest = httpContextAccessor.HttpContext?.Request ?? new DefaultHttpContext().Request;

            var enriched = result.Items
                .Select(item => item with
                {
                    BusinessImageUrl = item.BusinessImageUrl is not null
                        ? imageService.GetPublicImageUrl(item.BusinessImageUrl)
                        : null,

                    FeaturedCouponPhotoUrl = item.FeaturedCouponPhotoUrl is not null
                        ? imageService.GetPublicImageUrl(item.FeaturedCouponPhotoUrl)
                        : null
                })
                .ToList();

            return result with { Items = enriched.AsReadOnly() };
        }

        // ?????????????????????????????????????????????????????????????????????
        // PRIVATE: EMPTY PAGE FACTORY
        // Returns a well-formed empty pagination response so callers never
        // have to handle a null response.
        // ?????????????????????????????????????????????????????????????????????

        private static PaginationResponse<BusinessFeedItemDto> EmptyPage(PaginationRequest request) =>
            new([], request.ValidPageNumber, request.ValidPageSize, 0, 0, false, false);
    }
}
