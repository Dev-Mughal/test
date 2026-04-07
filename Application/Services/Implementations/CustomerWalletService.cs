using Application.Constants;
using Application.Services.Interfaces;
using Application.Utilities.UserContext;
using Common.Exceptions;
using Common.Features.Customer.Wallet.DTOs;
using Domain;
using Infrastructure;
using Infrastructure.Extensions;
using Infrastructure.Pagination;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Application.Services.Implementations
{
    public class CustomerWalletService(
        IDbContextFactory<BizyPopDbContext> contextFactory,
        IAuthorizedCustomer authorizedCustomer,
        IImageService imageService,
        IQRCodeService qrCodeService,
        IHttpContextAccessor httpContextAccessor) : ICustomerWalletService
    {
        // ??? Save Incentive ???????????????????????????????????????????????????
        // Wallet writes are never cached — each customer's wallet is personal data.
        public async Task SaveIncentiveAsync(SaveIncentiveDto dto)
        {
            var customerId = authorizedCustomer.Id;
            var utcNow     = DateTime.UtcNow;

            await contextFactory.WriteWithDbContextAsync(async context =>
            {
                // Validate the incentive type exists and is active; used to route to the
                // correct table when additional incentive types are implemented.
                var incentiveType = await context.IncentiveTypes
                    .AsNoTracking()
                    .FirstOrDefaultAsync(t => t.Id == dto.IncentiveTypeId && t.IsActive)
                    .ConfigureAwait(false)
                    ?? throw new ResourceNotFoundException($"Incentive type '{dto.IncentiveTypeId}' does not exist or is inactive.");

                //// MVP: only Coupon type is supported
                //if (!incentiveType.TypeCode.Equals(IncentiveTypeCodes.Coupon, StringComparison.OrdinalIgnoreCase))
                //    throw new BusinessException("Only Coupon incentives can be saved to the wallet at this time.");

                // Validate the coupon exists, is active, and has not expired
                var coupon = await context.Coupons
                    .AsNoTracking()
                    .FirstOrDefaultAsync(c =>
                        c.Id == dto.IncentiveId &&
                        c.IsActive &&
                        c.EndDateTime >= utcNow)
                    .ConfigureAwait(false)
                    ?? throw new ResourceNotFoundException($"Coupon '{dto.IncentiveId}' does not exist, is inactive, or has expired.");

                // Prevent saving the same coupon twice
                var alreadySaved = await context.CustomerCoupons
                    .AnyAsync(cc => cc.CustomerId == customerId && cc.CouponId == coupon.Id)
                    .ConfigureAwait(false);

                if (alreadySaved)
                    throw new DuplicateResourceException("This coupon is already in your wallet.");

                var entry = new CustomerCoupon
                {
                    CustomerId  = customerId,
                    CouponId    = coupon.Id,
                    QRCode      = string.Empty,
                    Status      = IncentiveEntitlementStatus.Accumulating,
                    StatusDate  = utcNow,
                    Created     = utcNow,
                    LastUpdated = utcNow
                };

                await context.CustomerCoupons.AddAsync(entry).ConfigureAwait(false);
                await context.SaveChangesAsync().ConfigureAwait(false);

                entry.QRCode = qrCodeService.GenerateIncentiveCode(IncentiveTableCode.CouponB, entry.Id);
                await context.SaveChangesAsync().ConfigureAwait(false);

                return entry.Id; // satisfy the generic WriteWithDbContextAsync<T> signature
            }).ConfigureAwait(false);
        }

        // ??? Get Wallet ???????????????????????????????????????????????????????
        public Task<PaginationResponse<WalletIncentiveItemDto>> GetWalletAsync(PaginationRequest request)
        {
            var customerId = authorizedCustomer.Id;

            return contextFactory.PaginateAsync(
                context => context.CustomerCoupons
                    .AsNoTracking()
                    .Where(cc => cc.CustomerId == customerId)
                    .OrderByDescending(cc => cc.Created)
                    .Select(cc => new WalletIncentiveItemDto(
                        cc.CouponId,
                        cc.Coupon.Business.BusinessName,
                        cc.Coupon.Title,
                        cc.Coupon.Description,
                        cc.Coupon.PhotoUrl,
                        cc.QRCode,         // Wallet track code comes from B-table record
                        cc.Coupon.EndDateTime,
                        cc.Status,
                        cc.DateRedeemed,
                        cc.Created)),
                request,
                urlMapperFunc: item =>
                {
                    if (string.IsNullOrWhiteSpace(item.PhotoUrl))
                        return item;

                    return item with { PhotoUrl = imageService.GetPublicImageUrl(item.PhotoUrl) };
                });
        }

    }
}
