using Application.Constants;
using Application.Services.IncentiveServices.Interfaces;
using Application.Services.Interfaces;
using Application.Utilities.UserContext;
using Common.Exceptions;
using Common.Features.Incentive.Promo.DTOs;
using Common.Mappers;
using Infrastructure;
using Infrastructure.Extensions;
using Infrastructure.Pagination;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Application.Services.IncentiveServices.Implementations
{
    public class PromoIncentiveService(
        IDbContextFactory<BizyPopDbContext> contextFactory,
        IAuthorizedUser authorizedUser,
        IQRCodeService qrCodeService,
        IImageService imageService,
        IHttpContextAccessor httpContextAccessor) : IPromoIncentiveService
    {
        public async Task<PromoIncentiveResponseDto> CreateAsync(CreatePromoIncentiveDto dto, int businessId)
        {
            if (businessId <= 0)
                throw new ForbiddenException("A valid business id is required.");

            var created = await contextFactory.WriteWithDbContextAsync(async context =>
            {
                await EnsureBusinessAccessAsync(context, businessId).ConfigureAwait(false);

                var promo = dto.ToPromoBizDef(businessId);
                if (dto.Photo is { Length: > 0 })
                    promo.PhotoUrl = await imageService.SaveImageAsync(dto.Photo, Common.Models.ImageTypeEnum.Incentive).ConfigureAwait(false);

                await context.PromoBizDefs.AddAsync(promo).ConfigureAwait(false);
                await context.SaveChangesAsync().ConfigureAwait(false);

                promo.QRCode = qrCodeService.GenerateIncentiveCode(IncentiveTableCode.Promo, promo.Id);
                await context.SaveChangesAsync().ConfigureAwait(false);

                return promo;
            }).ConfigureAwait(false);

            return EnrichPhoto(created.ToPromoIncentiveResponseDto(GetQrImageUrl(created.QRCode)));
        }

        public async Task<PromoIncentiveResponseDto?> GetByIdAsync(long promoId, int businessId)
        {
            if (businessId <= 0)
                throw new ForbiddenException("A valid business id is required.");

            return await contextFactory.QueryWithDbContextAsync(async context =>
            {
                await EnsureBusinessAccessAsync(context, businessId).ConfigureAwait(false);

                var promo = await context.PromoBizDefs
                    .AsNoTracking()
                    .FirstOrDefaultAsync(x => x.Id == promoId && x.BusinessId == businessId)
                    .ConfigureAwait(false);

                return promo is null ? null : EnrichPhoto(promo.ToPromoIncentiveResponseDto(GetQrImageUrl(promo.QRCode)));
            }).ConfigureAwait(false);
        }

        public async Task<PaginationResponse<PromoIncentiveListItemDto>> GetAllAsync(PaginationRequest request, int businessId)
        {
            if (businessId <= 0)
                throw new ForbiddenException("A valid business id is required.");

            return await contextFactory.QueryWithDbContextAsync(async context =>
            {
                await EnsureBusinessAccessAsync(context, businessId).ConfigureAwait(false);

                var baseQuery = context.PromoBizDefs
                    .AsNoTracking()
                    .Where(x => x.BusinessId == businessId)
                    .OrderByDescending(x => x.Id);

                var totalCount = await baseQuery.CountAsync().ConfigureAwait(false);
                var pageNumber = request.ValidPageNumber;
                var pageSize = request.ValidPageSize;
                var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

                var items = await baseQuery
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .Select(x => x.ToPromoIncentiveListItemDto())
                    .ToListAsync()
                    .ConfigureAwait(false);

                if (httpContextAccessor.HttpContext is not null)
                {
                    items = items
                        .Select(i => EnrichPhoto(i with { QRCodeImageUrl = GetQrImageUrl(i.TrackCode) }))
                        .ToList();
                }

                return new PaginationResponse<PromoIncentiveListItemDto>(
                    items.AsReadOnly(),
                    pageNumber,
                    pageSize,
                    totalCount,
                    totalPages,
                    pageNumber < totalPages,
                    pageNumber > 1);
            }).ConfigureAwait(false);
        }

        public async Task<PromoIncentiveResponseDto> UpdateAsync(long promoId, UpdatePromoIncentiveDto dto, int businessId)
        {
            if (businessId <= 0)
                throw new ForbiddenException("A valid business id is required.");

            var updated = await contextFactory.WriteWithDbContextAsync(async context =>
            {
                await EnsureBusinessAccessAsync(context, businessId).ConfigureAwait(false);

                var promo = await context.PromoBizDefs
                    .FirstOrDefaultAsync(x => x.Id == promoId && x.BusinessId == businessId)
                    .ConfigureAwait(false)
                    ?? throw new ResourceNotFoundException($"Promotion with id '{promoId}' was not found.");

                dto.ApplyTo(promo);
                if (dto.Photo is { Length: > 0 })
                    promo.PhotoUrl = await imageService.UpdateImageAsync(dto.Photo, promo.PhotoUrl, Common.Models.ImageTypeEnum.Incentive).ConfigureAwait(false);

                await context.SaveChangesAsync().ConfigureAwait(false);
                return promo;
            }).ConfigureAwait(false);

            return EnrichPhoto(updated.ToPromoIncentiveResponseDto(GetQrImageUrl(updated.QRCode)));
        }

        public async Task DeleteAsync(long promoId, int businessId)
        {
            if (businessId <= 0)
                throw new ForbiddenException("A valid business id is required.");

            await contextFactory.WriteWithDbContextAsync(async context =>
            {
                await EnsureBusinessAccessAsync(context, businessId).ConfigureAwait(false);

                var promo = await context.PromoBizDefs
                    .FirstOrDefaultAsync(x => x.Id == promoId && x.BusinessId == businessId)
                    .ConfigureAwait(false)
                    ?? throw new ResourceNotFoundException($"Promotion with id '{promoId}' was not found.");

                context.PromoBizDefs.Remove(promo);
                await context.SaveChangesAsync().ConfigureAwait(false);
            }).ConfigureAwait(false);
        }

        private async Task EnsureBusinessAccessAsync(BizyPopDbContext context, int businessId)
        {
            if (authorizedUser.Id <= 0)
                throw new ForbiddenException("Only authenticated business users can access incentives.");

            var hasBusinessAccess = await context.BusinessUserBusinesses
                .AsNoTracking()
                .AnyAsync(x => x.UserId == authorizedUser.Id && x.BusinessId == businessId)
                .ConfigureAwait(false);

            if (!hasBusinessAccess)
                throw new ForbiddenException($"You do not have access to business id '{businessId}'.");
        }

        private string? GetQrImageUrl(string code)
        {
            if (httpContextAccessor.HttpContext is null)
                return null;

            return qrCodeService.GenerateQRCodeImageUrl(code, httpContextAccessor.HttpContext.Request);
        }

        private PromoIncentiveResponseDto EnrichPhoto(PromoIncentiveResponseDto dto)
        {
            if (httpContextAccessor.HttpContext is null || string.IsNullOrWhiteSpace(dto.PhotoUrl))
                return dto;

            return dto with { PhotoUrl = imageService.GetPublicImageUrl(dto.PhotoUrl) };
        }

        private PromoIncentiveListItemDto EnrichPhoto(PromoIncentiveListItemDto dto)
        {
            if (httpContextAccessor.HttpContext is null || string.IsNullOrWhiteSpace(dto.PhotoUrl))
                return dto;

            return dto with { PhotoUrl = imageService.GetPublicImageUrl(dto.PhotoUrl) };
        }
    }
}
