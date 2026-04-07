using Application.Services.Interfaces;
using Application.Utilities.UserContext;
using Application.Utilities.Cache;
using Common.Exceptions;
using Common.Features.Business.DTOs;
using Common.Features.Customer.Business;
using Common.Mappers;
using Infrastructure;
using Infrastructure.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Application.Services.Implementations
{
    public class BusinessService(
        IDbContextFactory<BizyPopDbContext> contextFactory,
        IImageService imageService,
        IGeoService geoService,
        IAuthorizedUser authorizedUser,
        IHttpContextAccessor httpContextAccessor,
        ICacheService cache) : IBusinessService
    {
        // Business profiles change infrequently; 10-minute TTL balances freshness
        // with reduced DB load under moderate traffic.
        private static readonly TimeSpan BusinessTtl = TimeSpan.FromMinutes(10);

        public Task<BusinessCardDto?> GetBusinessByIdAsync(int businessId) =>
            cache.GetOrCreateAsync<BusinessCardDto?>(
                CacheKeys.Business(businessId),
                () => contextFactory.QueryWithDbContextAsync(async context =>
                {
                    var business = await context.Businesses
                        .AsNoTracking()
                        .Include(b => b.Category)
                        .Include(b => b.GeoCity)
                        .Include(b => b.GeoZipCode)
                        .Where(b => b.BusinessId == businessId)
                        .Select(b => b.ToBusinessCardDto())
                        .FirstOrDefaultAsync()
                        .ConfigureAwait(false);

                    if (business is not null &&
                        !string.IsNullOrWhiteSpace(business.ImageUrl))
                    {
                        business = business with
                        {
                            ImageUrl = imageService.GetPublicImageUrl(
                                business.ImageUrl)
                        };
                    }

                    return business;
                }),
                BusinessTtl);

        public Task<BusinessDetailDto?> GetBusinessDetailAsync(int businessId)
        {
            var utcNow = DateTime.UtcNow;

            return cache.GetOrCreateAsync<BusinessDetailDto?>(
                CacheKeys.CustomerBusinessDetail(businessId),
                () => contextFactory.QueryWithDbContextAsync(async context =>
                {
                    var detail = await context.Businesses
                        .AsNoTracking()
                        .Where(b => b.BusinessId == businessId)
                        .Select(b => new BusinessDetailDto(
                            b.BusinessId,
                            b.BusinessName,
                            b.Category.CategoryName,
                            b.BusinessImageUrl,
                            b.StreetAddress + ", " + b.GeoCity.City + ", " + b.GeoCity.State + " " + b.GeoZipCode.ZipCode + ", " + b.Country,
                            b.AddressLine2,
                            b.GeoCity.City,
                            b.GeoCity.State,
                            b.GeoZipCode.ZipCode,
                            b.Country,
                            b.BusinessPhone,
                            b.BusinessEmail,
                            b.BusinessURL,
                            b.Latitude,
                            b.Longitude,
                            // Count only active, non-expired coupons for the badge
                            b.Coupons.Count(c => c.IsActive && c.EndDateTime >= utcNow)))
                        .FirstOrDefaultAsync()
                        .ConfigureAwait(false);

                    // Resolve the business image to a full absolute URL
                    if (detail is not null &&
                        !string.IsNullOrWhiteSpace(detail.BusinessImageUrl))
                    {
                        detail = detail with
                        {
                            BusinessImageUrl = imageService.GetPublicImageUrl(
                                detail.BusinessImageUrl)
                        };
                    }

                    return detail;
                }),
                BusinessTtl);
        }

        public async Task<BusinessCardDto> UpdateBusinessAsync(int businessId, UpdateBusinessDto dto)
        {
            if (authorizedUser.Id <= 0)
                throw new ForbiddenException("Only authenticated business users can update business profiles.");

            var result = await contextFactory.WriteWithDbContextAsync(async context =>
            {
                var hasBusinessAccess = await context.BusinessUserBusinesses
                    .AsNoTracking()
                    .AnyAsync(x => x.UserId == authorizedUser.Id && x.BusinessId == businessId)
                    .ConfigureAwait(false);

                if (!hasBusinessAccess)
                    throw new ForbiddenException($"You do not have access to business id '{businessId}'.");

                var business = await context.Businesses
                    .Include(b => b.Category)
                    .Include(b => b.GeoCity)
                    .Include(b => b.GeoZipCode)
                    .FirstOrDefaultAsync(b => b.BusinessId == businessId)
                    .ConfigureAwait(false)
                    ?? throw new ResourceNotFoundException($"Business with id '{businessId}' was not found.");

                var (stateCityId, stateCityZipId) = await geoService
                    .ResolveGeoIdsAsync(dto.City, dto.State, dto.ZipCode, dto.ForceCreate)
                    .ConfigureAwait(false);

                business.BusinessName = dto.BusinessName;
                business.BusinessEmail = dto.BusinessEmail;
                business.BusinessPhone = dto.BusinessPhone;
                business.BusinessURL = dto.BusinessURL;
                business.CountryCode = dto.CountryCode;
                business.StreetAddress = dto.StreetAddress;
                business.AddressLine2 = dto.AddressLine2;
                business.Country = dto.Country;
                business.Longitude = dto.Longitude;
                business.Latitude = dto.Latitude;
                business.CategoryId = dto.CategoryId;
                business.StateCityId = stateCityId;
                business.StateCityZipId = stateCityZipId;

                if (dto.BusinessImage is { Length: > 0 })
                {
                    business.BusinessImageUrl = await imageService
                        .UpdateImageAsync(dto.BusinessImage, business.BusinessImageUrl, Common.Models.ImageTypeEnum.Business)
                        .ConfigureAwait(false);
                }

                await context.SaveChangesAsync().ConfigureAwait(false);

                await context.Entry(business).Reference(b => b.Category).LoadAsync().ConfigureAwait(false);
                await context.Entry(business).Reference(b => b.GeoCity).LoadAsync().ConfigureAwait(false);
                await context.Entry(business).Reference(b => b.GeoZipCode).LoadAsync().ConfigureAwait(false);

                return business.ToBusinessCardDto();
            }).ConfigureAwait(false);

            cache.Remove(CacheKeys.Business(businessId));
            cache.Remove(CacheKeys.CustomerBusinessDetail(businessId));

            if (!string.IsNullOrWhiteSpace(result.ImageUrl))
            {
                result = result with
                {
                    ImageUrl = imageService.GetPublicImageUrl(result.ImageUrl)
                };
            }

            return result;
        }
    }
}


