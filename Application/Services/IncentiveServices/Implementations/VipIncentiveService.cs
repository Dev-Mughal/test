using Application.Constants;
using Application.Services.IncentiveServices.Interfaces;
using Application.Services.Interfaces;
using Application.Utilities.UserContext;
using Common.Exceptions;
using Common.Features.Incentive.Vip.DTOs;
using Common.Mappers;
using Infrastructure;
using Infrastructure.Extensions;
using Infrastructure.Pagination;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Application.Services.IncentiveServices.Implementations
{
    public class VipIncentiveService(
        IDbContextFactory<BizyPopDbContext> contextFactory,
        IAuthorizedUser authorizedUser,
        IQRCodeService qrCodeService,
        IImageService imageService,
        IHttpContextAccessor httpContextAccessor) : IVipIncentiveService
    {
        public async Task<VipIncentiveResponseDto> CreateAsync(CreateVipIncentiveDto dto, int businessId)
        {
            ValidateBusinessId(businessId);

            var created = await contextFactory.WriteWithDbContextAsync(async context =>
            {
                await EnsureBusinessAccessAsync(context, businessId).ConfigureAwait(false);

                var vip = dto.ToVipBizDef(businessId);
                if (dto.Photo is { Length: > 0 })
                    vip.PhotoUrl = await imageService.SaveImageAsync(dto.Photo, Common.Models.ImageTypeEnum.Incentive).ConfigureAwait(false);
                await context.VipBizDefs.AddAsync(vip).ConfigureAwait(false);
                await context.SaveChangesAsync().ConfigureAwait(false);

                vip.QRCode = qrCodeService.GenerateIncentiveCode(IncentiveTableCode.VIP, vip.Id);
                await context.SaveChangesAsync().ConfigureAwait(false);
                return vip;
            }).ConfigureAwait(false);

            return EnrichPhoto(created.ToVipIncentiveResponseDto(GetQrImageUrl(created.QRCode)));
        }

        public async Task<VipIncentiveResponseDto?> GetByIdAsync(long vipId, int businessId)
        {
            ValidateBusinessId(businessId);

            return await contextFactory.QueryWithDbContextAsync(async context =>
            {
                await EnsureBusinessAccessAsync(context, businessId).ConfigureAwait(false);

                var vip = await context.VipBizDefs
                    .AsNoTracking()
                    .FirstOrDefaultAsync(x => x.Id == vipId && x.BusinessId == businessId)
                    .ConfigureAwait(false);

                return vip is null ? null : EnrichPhoto(vip.ToVipIncentiveResponseDto(GetQrImageUrl(vip.QRCode)));
            }).ConfigureAwait(false);
        }

        public async Task<PaginationResponse<VipIncentiveListItemDto>> GetAllAsync(PaginationRequest request, int businessId)
        {
            ValidateBusinessId(businessId);

            return await contextFactory.QueryWithDbContextAsync(async context =>
            {
                await EnsureBusinessAccessAsync(context, businessId).ConfigureAwait(false);

                var baseQuery = context.VipBizDefs
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
                    .Select(x => x.ToVipIncentiveListItemDto())
                    .ToListAsync()
                    .ConfigureAwait(false);

                if (httpContextAccessor.HttpContext is not null)
                    items = items.Select(i => EnrichPhoto(i with { QRCodeImageUrl = GetQrImageUrl(i.TrackCode) })).ToList();

                return new PaginationResponse<VipIncentiveListItemDto>(
                    items.AsReadOnly(),
                    pageNumber,
                    pageSize,
                    totalCount,
                    totalPages,
                    pageNumber < totalPages,
                    pageNumber > 1);
            }).ConfigureAwait(false);
        }

        public async Task<VipIncentiveResponseDto> UpdateAsync(long vipId, UpdateVipIncentiveDto dto, int businessId)
        {
            ValidateBusinessId(businessId);

            var updated = await contextFactory.WriteWithDbContextAsync(async context =>
            {
                await EnsureBusinessAccessAsync(context, businessId).ConfigureAwait(false);

                var vip = await context.VipBizDefs
                    .FirstOrDefaultAsync(x => x.Id == vipId && x.BusinessId == businessId)
                    .ConfigureAwait(false)
                    ?? throw new ResourceNotFoundException($"VIP incentive with id '{vipId}' was not found.");

                dto.ApplyTo(vip);
                if (dto.Photo is { Length: > 0 })
                    vip.PhotoUrl = await imageService.UpdateImageAsync(dto.Photo, vip.PhotoUrl, Common.Models.ImageTypeEnum.Incentive).ConfigureAwait(false);
                await context.SaveChangesAsync().ConfigureAwait(false);
                return vip;
            }).ConfigureAwait(false);

            return EnrichPhoto(updated.ToVipIncentiveResponseDto(GetQrImageUrl(updated.QRCode)));
        }

        public async Task DeleteAsync(long vipId, int businessId)
        {
            ValidateBusinessId(businessId);

            await contextFactory.WriteWithDbContextAsync(async context =>
            {
                await EnsureBusinessAccessAsync(context, businessId).ConfigureAwait(false);

                var vip = await context.VipBizDefs
                    .FirstOrDefaultAsync(x => x.Id == vipId && x.BusinessId == businessId)
                    .ConfigureAwait(false)
                    ?? throw new ResourceNotFoundException($"VIP incentive with id '{vipId}' was not found.");

                context.VipBizDefs.Remove(vip);
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

        private VipIncentiveResponseDto EnrichPhoto(VipIncentiveResponseDto dto)
        {
            if (httpContextAccessor.HttpContext is null || string.IsNullOrWhiteSpace(dto.PhotoUrl))
                return dto;

            return dto with { PhotoUrl = imageService.GetPublicImageUrl(dto.PhotoUrl) };
        }

        private VipIncentiveListItemDto EnrichPhoto(VipIncentiveListItemDto dto)
        {
            if (httpContextAccessor.HttpContext is null || string.IsNullOrWhiteSpace(dto.PhotoUrl))
                return dto;

            return dto with { PhotoUrl = imageService.GetPublicImageUrl(dto.PhotoUrl) };
        }
    }
}
