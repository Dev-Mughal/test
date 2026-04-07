using Application.Constants;
using Application.Services.Interfaces;
using Application.Utilities.UserContext;
using Common.Exceptions;
using Common.Features.Incentive.DTOs;
using Infrastructure;
using Infrastructure.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Application.Services.Implementations
{
    /// <summary>
    /// Checks for duplicate incentive titles across different incentive types.
    /// Register as scoped — uses DbContextFactory for lightweight read-only queries.
    /// </summary>
    public class IncentiveTitleService(
        IDbContextFactory<BizyPopDbContext> contextFactory, IAuthorizedUser authorizedUser) : IIncentiveTitleService
    {
        /// <summary>
        /// Checks if the given title already exists for the incentive type.
        /// Returns a warning (not blocking) — allows UX to show a notification.
        /// </summary>
        public async Task<IncentiveTitleCheckResponse> CheckTitleAsync(string title, int incentiveTypeId, int? businessId = null)
        {
            if (string.IsNullOrWhiteSpace(title))
                return new IncentiveTitleCheckResponse(false, "Title cannot be empty.");

            var resolvedBusinessId = businessId ?? authorizedUser.Current.BusinessId;
            if (resolvedBusinessId <= 0)
                throw new ForbiddenException("Only authenticated business users can check incentive titles.");

            var normalizedTitle = title.Trim().ToLower();
            var exists = false;

            // Query the appropriate table based on the incentive type
            switch (incentiveTypeId)
            {
                case 2: // Coupon (TypeCode = "C")
                    exists = await contextFactory.QueryWithDbContextAsync(ctx =>
                        ctx.Coupons.AnyAsync(c => c.Title.Trim().ToLower().Equals(normalizedTitle) && c.BusinessId == resolvedBusinessId))
                        .ConfigureAwait(false);
                    break;

                case 4: // Promotions (TypeCode = "M")
                    exists = await contextFactory.QueryWithDbContextAsync(ctx =>
                        ctx.PromoBizDefs.AnyAsync(p => p.PromotionDesc.Trim().ToLower().Equals(normalizedTitle) && p.BusinessId == resolvedBusinessId))
                        .ConfigureAwait(false);
                    break;

                case 3: // Stamp (TypeCode = "S")
                    exists = await contextFactory.QueryWithDbContextAsync(ctx =>
                        ctx.StampBizDefs.AnyAsync(s => s.RewardDesc.Trim().ToLower().Equals(normalizedTitle) && s.BusinessId == resolvedBusinessId))
                        .ConfigureAwait(false);
                    break;

                case 5: // Gift Card (TypeCode = "G")
                    exists = await contextFactory.QueryWithDbContextAsync(ctx =>
                        ctx.GiftCardBizDefs.AnyAsync(g => g.Title.Trim().ToLower().Equals(normalizedTitle) && g.BusinessId == resolvedBusinessId))
                        .ConfigureAwait(false);
                    break;

                case 8: // VIP Access (TypeCode = "V")
                    exists = await contextFactory.QueryWithDbContextAsync(ctx =>
                        ctx.VipBizDefs.AnyAsync(v => v.Description.Trim().ToLower().Equals(normalizedTitle) && v.BusinessId == resolvedBusinessId))
                        .ConfigureAwait(false);
                    break;

                case 7: // Store Point (TypeCode = "P") — not currently in "A" tables but reserved
                case 6: // Store Credit (TypeCode = "R") — not currently in "A" tables but reserved
                case 1: // BizyPop Dollars (TypeCode = "B") — not currently in "A" tables but reserved
                case 9: // Check-In (TypeCode = "I") — not currently in "A" tables but reserved
                default:
                    // Future: add more types as they are implemented
                    return new IncentiveTitleCheckResponse(false, $"Incentive type {incentiveTypeId} not yet supported.");
            }

            if (exists)
                return new IncentiveTitleCheckResponse(
                    true,
                    $"Warning: You already have an existing coupon titled {title}.  You may continue, but it may confuse customers and affect your reporting.");

            return new IncentiveTitleCheckResponse(false, null);
        }
    }
}
