using Application.Constants;
using Application.Services.IncentiveServices.Interfaces;
using Application.Services.Interfaces;
using Application.Utilities.UserContext;
using Common.Exceptions;
using Common.Features.Incentive.Stamp.DTOs;
using Common.Mappers;
using Infrastructure;
using Infrastructure.Extensions;
using Infrastructure.Pagination;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Application.Services.IncentiveServices.Implementations
{
    public class StampIncentiveService(
        IDbContextFactory<BizyPopDbContext> contextFactory,
        IAuthorizedUser authorizedUser,
        IQRCodeService qrCodeService,
        IImageService imageService,
        IHttpContextAccessor httpContextAccessor) : IStampIncentiveService
    {
        public async Task<StampIncentiveResponseDto> CreateAsync(CreateStampIncentiveDto dto, int businessId)
        {
            ValidateBusinessId(businessId);

            var created = await contextFactory.WriteWithDbContextAsync(async context =>
            {
                await EnsureBusinessAccessAsync(context, businessId).ConfigureAwait(false);

                var stamp = dto.ToStampBizDef(businessId);
                if (dto.Photo is { Length: > 0 })
                    stamp.PhotoUrl = await imageService.SaveImageAsync(dto.Photo, Common.Models.ImageTypeEnum.Incentive).ConfigureAwait(false);
                await context.StampBizDefs.AddAsync(stamp).ConfigureAwait(false);
                await context.SaveChangesAsync().ConfigureAwait(false);

                stamp.QRCode = qrCodeService.GenerateIncentiveCode(IncentiveTableCode.Stamp, stamp.Id);
                await context.SaveChangesAsync().ConfigureAwait(false);
                return stamp;
            }).ConfigureAwait(false);

            return EnrichPhoto(created.ToStampIncentiveResponseDto(GetQrImageUrl(created.QRCode)));
        }

        public async Task<StampIncentiveResponseDto?> GetByIdAsync(long stampId, int businessId)
        {
            ValidateBusinessId(businessId);

            return await contextFactory.QueryWithDbContextAsync(async context =>
            {
                await EnsureBusinessAccessAsync(context, businessId).ConfigureAwait(false);

                var stamp = await context.StampBizDefs
                    .AsNoTracking()
                    .FirstOrDefaultAsync(x => x.Id == stampId && x.BusinessId == businessId)
                    .ConfigureAwait(false);

                return stamp is null ? null : EnrichPhoto(stamp.ToStampIncentiveResponseDto(GetQrImageUrl(stamp.QRCode)));
            }).ConfigureAwait(false);
        }

        public async Task<PaginationResponse<StampIncentiveListItemDto>> GetAllAsync(PaginationRequest request, int businessId)
        {
            ValidateBusinessId(businessId);

            return await contextFactory.QueryWithDbContextAsync(async context =>
            {
                await EnsureBusinessAccessAsync(context, businessId).ConfigureAwait(false);

                var baseQuery = context.StampBizDefs
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
                    .Select(x => x.ToStampIncentiveListItemDto())
                    .ToListAsync()
                    .ConfigureAwait(false);

                if (httpContextAccessor.HttpContext is not null)
                    items = items.Select(i => EnrichPhoto(i with { QRCodeImageUrl = GetQrImageUrl(i.TrackCode) })).ToList();

                return new PaginationResponse<StampIncentiveListItemDto>(
                    items.AsReadOnly(),
                    pageNumber,
                    pageSize,
                    totalCount,
                    totalPages,
                    pageNumber < totalPages,
                    pageNumber > 1);
            }).ConfigureAwait(false);
        }

        public async Task<StampIncentiveResponseDto> UpdateAsync(long stampId, UpdateStampIncentiveDto dto, int businessId)
        {
            ValidateBusinessId(businessId);

            var updated = await contextFactory.WriteWithDbContextAsync(async context =>
            {
                await EnsureBusinessAccessAsync(context, businessId).ConfigureAwait(false);

                var stamp = await context.StampBizDefs
                    .FirstOrDefaultAsync(x => x.Id == stampId && x.BusinessId == businessId)
                    .ConfigureAwait(false)
                    ?? throw new ResourceNotFoundException($"Stamp incentive with id '{stampId}' was not found.");

                dto.ApplyTo(stamp);
                if (dto.Photo is { Length: > 0 })
                    stamp.PhotoUrl = await imageService.UpdateImageAsync(dto.Photo, stamp.PhotoUrl, Common.Models.ImageTypeEnum.Incentive).ConfigureAwait(false);
                await context.SaveChangesAsync().ConfigureAwait(false);
                return stamp;
            }).ConfigureAwait(false);

            return EnrichPhoto(updated.ToStampIncentiveResponseDto(GetQrImageUrl(updated.QRCode)));
        }

        public async Task DeleteAsync(long stampId, int businessId)
        {
            ValidateBusinessId(businessId);

            await contextFactory.WriteWithDbContextAsync(async context =>
            {
                await EnsureBusinessAccessAsync(context, businessId).ConfigureAwait(false);

                var stamp = await context.StampBizDefs
                    .FirstOrDefaultAsync(x => x.Id == stampId && x.BusinessId == businessId)
                    .ConfigureAwait(false)
                    ?? throw new ResourceNotFoundException($"Stamp incentive with id '{stampId}' was not found.");

                context.StampBizDefs.Remove(stamp);
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

        private StampIncentiveResponseDto EnrichPhoto(StampIncentiveResponseDto dto)
        {
            if (httpContextAccessor.HttpContext is null || string.IsNullOrWhiteSpace(dto.PhotoUrl))
                return dto;

            return dto with { PhotoUrl = imageService.GetPublicImageUrl(dto.PhotoUrl) };
        }

        private StampIncentiveListItemDto EnrichPhoto(StampIncentiveListItemDto dto)
        {
            if (httpContextAccessor.HttpContext is null || string.IsNullOrWhiteSpace(dto.PhotoUrl))
                return dto;

            return dto with { PhotoUrl = imageService.GetPublicImageUrl(dto.PhotoUrl) };
        }
    }
}
