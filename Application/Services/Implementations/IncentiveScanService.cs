using Application.Constants;
using Application.Services.Interfaces;
using Common.Exceptions;
using Common.Features.Scan;
using Infrastructure;
using Infrastructure.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Application.Services.Implementations
{
    public class IncentiveScanService(
        IDbContextFactory<BizyPopDbContext> contextFactory,
        IQRCodeService qrCodeService) : IIncentiveScanService
    {
        // ??? ENTRY POINT ???????????????????????????????????????????????????
        // Parse → validate → dispatch to the correct table handler.
        // FindAsync on every handler guarantees a PK-index lookup with no full scan.
        public async Task<IncentiveScanResult> ScanAsync(string code)
        {
            var (tableCode, id) = qrCodeService.ParseIncentiveCode(code);

            return tableCode switch
            {
                IncentiveTableCode.CouponB   => await ScanCouponB(id).ConfigureAwait(false),
                IncentiveTableCode.PromoB    => await ScanPromoB(id).ConfigureAwait(false),
                IncentiveTableCode.StampB    => await ScanStampB(id).ConfigureAwait(false),
                IncentiveTableCode.GiftCardB => await ScanGiftCardB(id).ConfigureAwait(false),
                IncentiveTableCode.VipB      => await ScanVipB(id).ConfigureAwait(false),
                IncentiveTableCode.RaffleB   => await ScanRaffleB(id).ConfigureAwait(false),

                IncentiveTableCode.CouponA   => await ScanCoupon(id).ConfigureAwait(false),
                IncentiveTableCode.PromoA    => await ScanPromo(id).ConfigureAwait(false),
                IncentiveTableCode.StampA    => await ScanStamp(id).ConfigureAwait(false),
                IncentiveTableCode.GiftCardA => await ScanGiftCard(id).ConfigureAwait(false),
                IncentiveTableCode.VipA      => await ScanVip(id).ConfigureAwait(false),
                IncentiveTableCode.RaffleA   => await ScanRaffle(id).ConfigureAwait(false),
                _                           => throw new InvalidIncentiveCodeException(tableCode)
            };
        }

        private async Task<IncentiveScanResult> ScanCouponB(long id)
        {
            var record = await contextFactory.QueryWithDbContextAsync(ctx =>
                ctx.CustomerCoupons.FindAsync(id).AsTask()).ConfigureAwait(false)
                ?? throw new ResourceNotFoundException($"Coupon entitlement with id {id} was not found.");

            return new IncentiveScanResult("Coupon", IncentiveTableCode.CouponB, id, record);
        }

        private async Task<IncentiveScanResult> ScanPromoB(long id)
        {
            var record = await contextFactory.QueryWithDbContextAsync(ctx =>
                ctx.PromoUserUsages.FindAsync(id).AsTask()).ConfigureAwait(false)
                ?? throw new ResourceNotFoundException($"Promotion usage with id {id} was not found.");

            return new IncentiveScanResult("Promo", IncentiveTableCode.PromoB, id, record);
        }

        private async Task<IncentiveScanResult> ScanStampB(long id)
        {
            var record = await contextFactory.QueryWithDbContextAsync(ctx =>
                ctx.StampUserEnts.FindAsync(id).AsTask()).ConfigureAwait(false)
                ?? throw new ResourceNotFoundException($"Stamp entitlement with id {id} was not found.");

            return new IncentiveScanResult("Stamp", IncentiveTableCode.StampB, id, record);
        }

        private async Task<IncentiveScanResult> ScanGiftCardB(long id)
        {
            var record = await contextFactory.QueryWithDbContextAsync(ctx =>
                ctx.GiftCardUserEnts.FindAsync(id).AsTask()).ConfigureAwait(false)
                ?? throw new ResourceNotFoundException($"Gift card entitlement with id {id} was not found.");

            return new IncentiveScanResult("GiftCard", IncentiveTableCode.GiftCardB, id, record);
        }

        private async Task<IncentiveScanResult> ScanVipB(long id)
        {
            var record = await contextFactory.QueryWithDbContextAsync(ctx =>
                ctx.VipUserEnts.FindAsync(id).AsTask()).ConfigureAwait(false)
                ?? throw new ResourceNotFoundException($"VIP entitlement with id {id} was not found.");

            return new IncentiveScanResult("VIP", IncentiveTableCode.VipB, id, record);
        }

        private async Task<IncentiveScanResult> ScanRaffleB(long id)
        {
            var record = await contextFactory.QueryWithDbContextAsync(ctx =>
                ctx.RaffleSchedules.FindAsync(id).AsTask()).ConfigureAwait(false)
                ?? throw new ResourceNotFoundException($"Raffle schedule with id {id} was not found.");

            return new IncentiveScanResult("Raffle", IncentiveTableCode.RaffleB, id, record);
        }

        // ??? PRIVATE HANDLERS — one per "A" table ????????????????????????????????????
        // Every handler follows the same pattern:
        //   1. FindAsync(id) — PK lookup only, never a table scan
        //   2. Null-check → ResourceNotFoundException
        //   3. Wrap in IncentiveScanResult with the matching IncentiveTableCode constant

        private async Task<IncentiveScanResult> ScanCoupon(long id)
        {
            var record = await contextFactory.QueryWithDbContextAsync(ctx =>
                ctx.Coupons.FindAsync(id).AsTask()).ConfigureAwait(false)
                ?? throw new ResourceNotFoundException($"Coupon with id {id} was not found.");

            return new IncentiveScanResult("Coupon", IncentiveTableCode.CouponA, id, record);
        }

        private async Task<IncentiveScanResult> ScanPromo(long id)
        {
            var record = await contextFactory.QueryWithDbContextAsync(ctx =>
                ctx.PromoBizDefs.FindAsync(id).AsTask()).ConfigureAwait(false)
                ?? throw new ResourceNotFoundException($"Promotion with id {id} was not found.");

            return new IncentiveScanResult("Promo", IncentiveTableCode.PromoA, id, record);
        }

        private async Task<IncentiveScanResult> ScanStamp(long id)
        {
            var record = await contextFactory.QueryWithDbContextAsync(ctx =>
                ctx.StampBizDefs.FindAsync(id).AsTask()).ConfigureAwait(false)
                ?? throw new ResourceNotFoundException($"Stamp program with id {id} was not found.");

            return new IncentiveScanResult("Stamp", IncentiveTableCode.StampA, id, record);
        }

        private async Task<IncentiveScanResult> ScanGiftCard(long id)
        {
            var record = await contextFactory.QueryWithDbContextAsync(ctx =>
                ctx.GiftCardBizDefs.FindAsync(id).AsTask()).ConfigureAwait(false)
                ?? throw new ResourceNotFoundException($"Gift card program with id {id} was not found.");

            return new IncentiveScanResult("GiftCard", IncentiveTableCode.GiftCardA, id, record);
        }

        private async Task<IncentiveScanResult> ScanVip(long id)
        {
            var record = await contextFactory.QueryWithDbContextAsync(ctx =>
                ctx.VipBizDefs.FindAsync(id).AsTask()).ConfigureAwait(false)
                ?? throw new ResourceNotFoundException($"VIP program with id {id} was not found.");

            return new IncentiveScanResult("VIP", IncentiveTableCode.VipA, id, record);
        }

        private async Task<IncentiveScanResult> ScanRaffle(long id)
        {
            var record = await contextFactory.QueryWithDbContextAsync(ctx =>
                ctx.RaffleDefs.FindAsync(id).AsTask()).ConfigureAwait(false)
                ?? throw new ResourceNotFoundException($"Raffle with id {id} was not found.");

            return new IncentiveScanResult("Raffle", IncentiveTableCode.RaffleA, id, record);
        }
    }
}
