using Application.Constants;
using Application.Services.IncentiveServices.Interfaces;
using Application.Services.Interfaces;
using Application.Utilities.UserContext;
using Common.Exceptions;
using Common.Features.Incentive.GiftCard.DTOs;
using Common.Mappers;
using Infrastructure;
using Infrastructure.Extensions;
using Infrastructure.Pagination;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Application.Services.IncentiveServices.Implementations
{
    public class GiftCardIncentiveService(
        IDbContextFactory<BizyPopDbContext> contextFactory,
        IAuthorizedUser authorizedUser,
        IQRCodeService qrCodeService,
        IImageService imageService,
        IHttpContextAccessor httpContextAccessor) : IGiftCardIncentiveService
    {
        public async Task<GiftCardIncentiveResponseDto> CreateAsync(CreateGiftCardIncentiveDto dto, int businessId)
        {
            ValidateBusinessId(businessId);

            var created = await contextFactory.WriteWithDbContextAsync(async context =>
            {
                await EnsureBusinessAccessAsync(context, businessId).ConfigureAwait(false);

                var giftCard = dto.ToGiftCardBizDef(businessId);
                if (dto.Photo is { Length: > 0 })
                    giftCard.PhotoUrl = await imageService.SaveImageAsync(dto.Photo, Common.Models.ImageTypeEnum.Incentive).ConfigureAwait(false);
                await context.GiftCardBizDefs.AddAsync(giftCard).ConfigureAwait(false);
                await context.SaveChangesAsync().ConfigureAwait(false);

                giftCard.QRCode = qrCodeService.GenerateIncentiveCode(IncentiveTableCode.GiftCard, giftCard.Id);
                await context.SaveChangesAsync().ConfigureAwait(false);
                return giftCard;
            }).ConfigureAwait(false);

            return EnrichPhoto(created.ToGiftCardIncentiveResponseDto(GetQrImageUrl(created.QRCode)));
        }

        public async Task<GiftCardIncentiveResponseDto?> GetByIdAsync(long giftCardId, int businessId)
        {
            ValidateBusinessId(businessId);

            return await contextFactory.QueryWithDbContextAsync(async context =>
            {
                await EnsureBusinessAccessAsync(context, businessId).ConfigureAwait(false);

                var giftCard = await context.GiftCardBizDefs
                    .AsNoTracking()
                    .FirstOrDefaultAsync(x => x.Id == giftCardId && x.BusinessId == businessId)
                    .ConfigureAwait(false);

                return giftCard is null ? null : EnrichPhoto(giftCard.ToGiftCardIncentiveResponseDto(GetQrImageUrl(giftCard.QRCode)));
            }).ConfigureAwait(false);
        }

        public async Task<PaginationResponse<GiftCardIncentiveListItemDto>> GetAllAsync(PaginationRequest request, int businessId)
        {
            ValidateBusinessId(businessId);

            return await contextFactory.QueryWithDbContextAsync(async context =>
            {
                await EnsureBusinessAccessAsync(context, businessId).ConfigureAwait(false);

                var baseQuery = context.GiftCardBizDefs
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
                    .Select(x => x.ToGiftCardIncentiveListItemDto())
                    .ToListAsync()
                    .ConfigureAwait(false);

                if (httpContextAccessor.HttpContext is not null)
                {
                    items = items.Select(i =>
                    {
                        var enriched = i with { QRCodeImageUrl = GetQrImageUrl(i.TrackCode) };
                        if (!string.IsNullOrWhiteSpace(enriched.PhotoUrl))
                            enriched = enriched with { PhotoUrl = imageService.GetPublicImageUrl(enriched.PhotoUrl) };
                        return enriched;
                    }).ToList();
                }

                return new PaginationResponse<GiftCardIncentiveListItemDto>(
                    items.AsReadOnly(),
                    pageNumber,
                    pageSize,
                    totalCount,
                    totalPages,
                    pageNumber < totalPages,
                    pageNumber > 1);
            }).ConfigureAwait(false);
        }

        public async Task<GiftCardIncentiveResponseDto> UpdateAsync(long giftCardId, UpdateGiftCardIncentiveDto dto, int businessId)
        {
            ValidateBusinessId(businessId);

            var updated = await contextFactory.WriteWithDbContextAsync(async context =>
            {
                await EnsureBusinessAccessAsync(context, businessId).ConfigureAwait(false);

                var giftCard = await context.GiftCardBizDefs
                    .FirstOrDefaultAsync(x => x.Id == giftCardId && x.BusinessId == businessId)
                    .ConfigureAwait(false)
                    ?? throw new ResourceNotFoundException($"Gift card incentive with id '{giftCardId}' was not found.");

                dto.ApplyTo(giftCard);
                if (dto.Photo is { Length: > 0 })
                    giftCard.PhotoUrl = await imageService.UpdateImageAsync(dto.Photo, giftCard.PhotoUrl, Common.Models.ImageTypeEnum.Incentive).ConfigureAwait(false);
                await context.SaveChangesAsync().ConfigureAwait(false);
                return giftCard;
            }).ConfigureAwait(false);

            return EnrichPhoto(updated.ToGiftCardIncentiveResponseDto(GetQrImageUrl(updated.QRCode)));
        }

        public async Task DeleteAsync(long giftCardId, int businessId)
        {
            ValidateBusinessId(businessId);

            await contextFactory.WriteWithDbContextAsync(async context =>
            {
                await EnsureBusinessAccessAsync(context, businessId).ConfigureAwait(false);

                var giftCard = await context.GiftCardBizDefs
                    .FirstOrDefaultAsync(x => x.Id == giftCardId && x.BusinessId == businessId)
                    .ConfigureAwait(false)
                    ?? throw new ResourceNotFoundException($"Gift card incentive with id '{giftCardId}' was not found.");

                context.GiftCardBizDefs.Remove(giftCard);
                await context.SaveChangesAsync().ConfigureAwait(false);
            }).ConfigureAwait(false);
        }

        private static void ValidateBusinessId(int businessId)
        {
            if (businessId <= 0)
                throw new ForbiddenException("A valid business id is required.");
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

        private GiftCardIncentiveResponseDto EnrichPhoto(GiftCardIncentiveResponseDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.PhotoUrl))
                return dto;

            return dto with { PhotoUrl = imageService.GetPublicImageUrl(dto.PhotoUrl) };
        }

    }
}
