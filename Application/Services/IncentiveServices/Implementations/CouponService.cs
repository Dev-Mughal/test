using Application.Constants;
using Application.Utilities.Cache;
using Application.Utilities.UserContext;
using Common.Exceptions;
using Common.Features.Coupon.DTOs;
using Common.Features.Customer.Business;
using Common.Mappers;
using Infrastructure;
using Infrastructure.Extensions;
using Infrastructure.Pagination;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Common.Models;
using Application.Services.Interfaces;
using Application.Services.IncentiveServices.Interfaces;

namespace Application.Services.IncentiveServices.Implementations
{
    public class CouponService(
        IDbContextFactory<BizyPopDbContext> contextFactory,
        IImageService imageService,
        IQRCodeService qrCodeService,
        IHttpContextAccessor httpContextAccessor,
        IAuthorizedUser authorizedUser,
        ICacheService cache) : ICouponService
    {
        // ??? TTLs ????????????????????????????????????????????????????????????
        private static readonly TimeSpan TypesLookupTtl = TimeSpan.FromHours(24);
        private static readonly TimeSpan CouponByIdTtl  = TimeSpan.FromMinutes(5);
        private static readonly TimeSpan CouponsListTtl = TimeSpan.FromMinutes(3);

        // ??? Coupon types lookup ?????????????????????????????????????????????
        // Coupon types are admin-managed, near-static ? long-lived shared cache.
        public Task<IReadOnlyList<IncentiveTypeLookupDto>> GetCouponTypesLookupAsync() =>
            cache.GetOrCreateAsync(
                CacheKeys.CouponTypesLookup,
                () => contextFactory.QueryWithDbContextAsync(async context =>
                    (IReadOnlyList<IncentiveTypeLookupDto>) await context.IncentiveTypes
                        .AsNoTracking()
                        .Where(ct => ct.IsActive)
                        .OrderBy(ct => ct.TypeDescription)
                        .Select(ct => new IncentiveTypeLookupDto(ct.Id, ct.TypeDescription))
                        .ToListAsync()
                        .ConfigureAwait(false)),
                TypesLookupTtl);

        // ??? Create coupon ???????????????????????????????????????????????????
        // Two-step save: insert the coupon first to obtain the PK, then generate
        // the deterministic {tableCode}-{id} code and persist it in a second save.
        // Invalidates the per-business coupon group so list/detail caches are
        // evicted immediately after a new coupon is persisted.
        public async Task<CouponResponseDto> CreateCouponAsync(CreateCouponDto dto, int? businessId = null)
        {
            var timeZone = authorizedUser.TimeZone;
            var resolvedBusinessId = businessId ?? authorizedUser.Current.BusinessId;

            if (resolvedBusinessId <= 0)
                throw new ForbiddenException("Only authenticated business users can create coupons.");

            var result = await contextFactory.WriteWithDbContextAsync(async context =>
            {
                var businessExists = await context.Businesses
                    .AsNoTracking()
                    .AnyAsync(b => b.BusinessId == resolvedBusinessId)
                    .ConfigureAwait(false);

                if (!businessExists)
                    throw new ResourceNotFoundException($"Business profile with id '{resolvedBusinessId}' was not found.");

                string? photoUrl = null;
                if (dto.Photo is { Length: > 0 })
                    photoUrl = await imageService.SaveImageAsync(dto.Photo, ImageTypeEnum.Coupon).ConfigureAwait(false);

                // Step 1: insert without QRCode to get the auto-generated PK.
                var coupon = dto.ToCoupon(photoUrl, resolvedBusinessId, timeZone);
                await context.Coupons.AddAsync(coupon).ConfigureAwait(false);
                await context.SaveChangesAsync().ConfigureAwait(false);

                // Step 2: generate the {tableCode}-{id} code and store in QRCode column.
                coupon.QRCode = qrCodeService.GenerateIncentiveCode(IncentiveTableCode.Coupon, coupon.Id);
                await context.SaveChangesAsync().ConfigureAwait(false);

                return coupon.ToCouponResponseDto();
            }).ConfigureAwait(false);

            // Bust all cached coupon lists/details for this business
            cache.InvalidateGroup(CacheKeys.CouponsGroup(resolvedBusinessId));

            // Enrich with absolute URLs � follows same pattern as image URL enrichment
            if (httpContextAccessor.HttpContext is not null)
            {
                var request = httpContextAccessor.HttpContext.Request;

                if (result.PhotoUrl is not null)
                    result = result with { PhotoUrl = imageService.GetPublicImageUrl(result.PhotoUrl) };

                // Build the full QR image URL from the code string stored in DB
                result = result with { QRCodeImageUrl = qrCodeService.GenerateQRCodeImageUrl(result.TrackCode, request) };
            }

            return result;
        }

        public async Task<CouponResponseDto> UpdateCouponAsync(long couponId, UpdateCouponDto dto, int? businessId = null)
        {
            var timeZone = authorizedUser.TimeZone;
            var resolvedBusinessId = businessId ?? authorizedUser.Current.BusinessId;

            if (resolvedBusinessId <= 0)
                throw new ForbiddenException("Only authenticated business users can update coupons.");

            var result = await contextFactory.WriteWithDbContextAsync(async context =>
            {
                var coupon = await context.Coupons
                    .FirstOrDefaultAsync(c => c.Id == couponId && c.BusinessId == resolvedBusinessId)
                    .ConfigureAwait(false)
                    ?? throw new ResourceNotFoundException($"Coupon with id '{couponId}' was not found.");

                if (dto.Photo is { Length: > 0 })
                {
                    coupon.PhotoUrl = await imageService
                        .UpdateImageAsync(dto.Photo, coupon.PhotoUrl, ImageTypeEnum.Coupon)
                        .ConfigureAwait(false);
                }

                dto.ApplyTo(coupon, timeZone);
                await context.SaveChangesAsync().ConfigureAwait(false);
                return coupon.ToCouponResponseDto();
            }).ConfigureAwait(false);

            cache.InvalidateGroup(CacheKeys.CouponsGroup(resolvedBusinessId));

            if (httpContextAccessor.HttpContext is not null)
            {
                var request = httpContextAccessor.HttpContext.Request;

                if (result.PhotoUrl is not null)
                    result = result with { PhotoUrl = imageService.GetPublicImageUrl(result.PhotoUrl) };

                result = result with { QRCodeImageUrl = qrCodeService.GenerateQRCodeImageUrl(result.TrackCode, request) };
            }

            return result;
        }

        // ??? Get coupon by id ????????????????????????????????????????????????
        public Task<CouponResponseDto?> GetCouponByIdAsync(long couponId, int? businessId = null)
        {
            var resolvedBusinessId = businessId ?? authorizedUser.Current.BusinessId;
            if (resolvedBusinessId <= 0)
                throw new ForbiddenException("Only authenticated business users can access coupons.");

            var key        = CacheKeys.CouponById(resolvedBusinessId, couponId);
            var group      = CacheKeys.CouponsGroup(resolvedBusinessId);

            return cache.GetOrCreateInGroupAsync<CouponResponseDto?>(
                key, group,
                () => contextFactory.QueryWithDbContextAsync(async context =>
                {
                    var coupon = await context.Coupons
                        .AsNoTracking()
                        .Where(c => c.Id == couponId && c.BusinessId == resolvedBusinessId)
                        .Select(c => c.ToCouponResponseDto())
                        .FirstOrDefaultAsync()
                        .ConfigureAwait(false);

                    if (coupon is not null && !string.IsNullOrWhiteSpace(coupon.PhotoUrl))
                    {
                        coupon = coupon with { PhotoUrl = imageService.GetPublicImageUrl(coupon.PhotoUrl) };
                    }

                    return coupon;
                }),
                CouponByIdTtl);
        }

        // ??? Get all coupons (paginated) ?????????????????????????????????????
        public Task<PaginationResponse<CouponListItemDto>> GetAllCouponsAsync(CouponPaginationRequest request, int? businessId = null)
        {
            var resolvedBusinessId = businessId ?? authorizedUser.Current.BusinessId;
            if (resolvedBusinessId <= 0)
                throw new ForbiddenException("Only authenticated business users can access coupons.");

            var key        = CacheKeys.CouponsList(resolvedBusinessId, request.PageNumber, request.PageSize);
            var group      = CacheKeys.CouponsGroup(resolvedBusinessId);

            return cache.GetOrCreateInGroupAsync(
                key, group,
                () => contextFactory.PaginateAsync(
                    context => context.Coupons
                        .AsNoTracking()
                        .Where(c => c.BusinessId == resolvedBusinessId)
                        .OrderByDescending(c => c.CreatedOn)
                        .Select(c => new CouponListItemDto(
                            c.Id,
                            c.Title,
                            c.Description,
                            c.PhotoUrl,
                            c.QRCode,   // TrackCode = code string
                            null,       // QRCodeImageUrl enriched later
                            c.StartDateTime,
                            c.EndDateTime,
                            c.ExpirationTime,
                            c.IsActive,
                            c.IsFeatured,
                            c.CreatedOn)),
                    request,
                    urlMapperFunc: item =>
                    {
                        var enriched = item;
                        if (!string.IsNullOrWhiteSpace(item.PhotoUrl))
                            enriched = enriched with { PhotoUrl = imageService.GetPublicImageUrl(item.PhotoUrl) };

                        var request = httpContextAccessor.HttpContext?.Request ?? new DefaultHttpContext().Request;

                        // Enrich with QR image URL from the TrackCode string
                        if (httpContextAccessor.HttpContext is not null)
                            enriched = enriched with { QRCodeImageUrl = qrCodeService.GenerateQRCodeImageUrl(item.TrackCode, request) };

                        return enriched;
                    }),
                CouponsListTtl);
        }

        // ??? Customer: paginated active incentives for a business ?????????????
        // All supported incentive definitions are projected into one unified list
        // with incentive-type metadata so wallet save can route by type.
        public Task<PaginationResponse<CustomerIncentiveItemDto>> GetBusinessIncentivesForCustomerAsync(
            int businessId,
            CouponPaginationRequest request)
        {
            var utcNow = DateTime.UtcNow;
            var key    = CacheKeys.CustomerIncentivesList(businessId, request.ValidPageNumber, request.ValidPageSize);
            var group  = CacheKeys.CouponsGroup(businessId);

            return cache.GetOrCreateInGroupAsync(
                key, group,
                async () =>
                {
                    var typeMap = await contextFactory.QueryWithDbContextAsync(async context =>
                        await context.IncentiveTypes
                            .AsNoTracking()
                            .Where(t => t.IsActive)
                            .Select(t => new { t.Id, t.TypeCode })
                            .ToDictionaryAsync(t => t.TypeCode, t => t.Id)
                            .ConfigureAwait(false)).ConfigureAwait(false);

                    var couponTypeId = typeMap.GetValueOrDefault(IncentiveTypeCodes.Coupon);
                    var promoTypeId = typeMap.GetValueOrDefault(IncentiveTypeCodes.Promotions);
                    var stampTypeId = typeMap.GetValueOrDefault(IncentiveTypeCodes.Stamp);
                    var giftCardTypeId = typeMap.GetValueOrDefault(IncentiveTypeCodes.GiftCard);
                    var vipTypeId = typeMap.GetValueOrDefault(IncentiveTypeCodes.VIPAccess);
                    var raffleTypeId = typeMap.GetValueOrDefault(IncentiveTypeCodes.StoreCredit);

                    var result = await contextFactory.PaginateAsync(
                        context =>
                            context.Coupons
                                .AsNoTracking()
                                .Where(c =>
                                    c.BusinessId == businessId &&
                                    c.IsActive &&
                                    c.EndDateTime >= utcNow)
                                .Select(c => new
                                {
                                    Id = c.Id,
                                    TypeId = couponTypeId,
                                    TypeCode = IncentiveTypeCodes.Coupon,
                                    Title = c.Title,
                                    Description = c.Description,
                                    PhotoUrl = c.PhotoUrl,
                                    QRCode = c.QRCode,
                                    EndDateTime = c.EndDateTime,
                                    IsFeatured = c.IsFeatured
                                })

                            .Concat(context.PromoBizDefs
                                .AsNoTracking()
                                .Where(p =>
                                    p.BusinessId == businessId &&
                                    p.StartDate <= utcNow &&
                                    p.ExpirationDate >= utcNow)
                                .Select(p => new
                                {
                                    Id = p.Id,
                                    TypeId = promoTypeId,
                                    TypeCode = IncentiveTypeCodes.Promotions,
                                    Title = p.PromotionDesc,
                                    Description = p.FinePrint ?? p.PromotionDesc,
                                    PhotoUrl = (string?)null,
                                    QRCode = p.QRCode,
                                    EndDateTime = p.ExpirationDate,
                                    IsFeatured = false
                                }))

                            .Concat(context.StampBizDefs
                                .AsNoTracking()
                                .Where(s => s.BusinessId == businessId)
                                .Select(s => new
                                {
                                    Id = s.Id,
                                    TypeId = stampTypeId,
                                    TypeCode = IncentiveTypeCodes.Stamp,
                                    Title = s.RewardDesc,
                                    Description = s.FinePrint ?? s.RewardDesc,
                                    PhotoUrl = (string?)null,
                                    QRCode = s.QRCode,
                                    EndDateTime = DateTime.MaxValue,
                                    IsFeatured = false
                                }))

                            .Concat(context.GiftCardBizDefs
                                .AsNoTracking()
                                .Where(g =>
                                    g.BusinessId == businessId &&
                                    (g.Expiration == null || g.Expiration >= utcNow))
                                .Select(g => new
                                {
                                    Id = g.Id,
                                    TypeId = giftCardTypeId,
                                    TypeCode = IncentiveTypeCodes.GiftCard,
                                    Title = g.Title,
                                    Description = g.FinePrint ?? g.MarketingText ?? g.Title,
                                    PhotoUrl = (string?)null,
                                    QRCode = g.QRCode,
                                    EndDateTime = g.Expiration ?? DateTime.MaxValue,
                                    IsFeatured = false
                                }))

                            .Concat(context.VipBizDefs
                                .AsNoTracking()
                                .Where(v =>
                                    v.BusinessId == businessId &&
                                    (v.Expiration == null || v.Expiration >= utcNow))
                                .Select(v => new
                                {
                                    Id = v.Id,
                                    TypeId = vipTypeId,
                                    TypeCode = IncentiveTypeCodes.VIPAccess,
                                    Title = v.Description,
                                    Description = v.FinePrint ?? v.Description,
                                    PhotoUrl = (string?)null,
                                    QRCode = v.QRCode,
                                    EndDateTime = v.Expiration ?? DateTime.MaxValue,
                                    IsFeatured = false
                                }))

                            .Concat(context.RaffleDefs
                                .AsNoTracking()
                                .Where(r =>
                                    r.BusinessId == businessId &&
                                    r.Enabled &&
                                    (r.DateOfDrawing == null || r.DateOfDrawing >= utcNow))
                                .Select(r => new
                                {
                                    Id = r.Id,
                                    TypeId = raffleTypeId,
                                    TypeCode = IncentiveTypeCodes.StoreCredit,
                                    Title = r.Name,
                                    Description = r.CustomPrize ?? r.Name,
                                    PhotoUrl = (string?)null,
                                    QRCode = r.QRCode,
                                    EndDateTime = r.DateOfDrawing ?? DateTime.MaxValue,
                                    IsFeatured = false
                                }))

                            .OrderByDescending(i => i.IsFeatured)
                            .ThenBy(i => i.EndDateTime)
                            .ThenBy(i => i.Id)
                            .Select(i => new CustomerIncentiveItemDto(
                                i.Id,
                                i.TypeId,
                                i.TypeCode,
                                i.Title,
                                i.Description,
                                i.PhotoUrl,
                                i.QRCode,
                                i.EndDateTime,
                                true,
                                i.IsFeatured
                            )),
                        request,
                        urlMapperFunc: item =>
                        {
                            if (string.IsNullOrWhiteSpace(item.PhotoUrl))
                                return item;

                            return item with { PhotoUrl = imageService.GetPublicImageUrl(item.PhotoUrl) };
                        }).ConfigureAwait(false);

                    return result;
                },
                CouponsListTtl);
        }
    }
}



