using Domain;
using Microsoft.AspNetCore.Http;

namespace Common.Features.Incentive.GiftCard.DTOs
{
    public class CreateGiftCardIncentiveDto
    {
        public string Title { get; set; } = null!;
        public IFormFile? Photo { get; set; }
        public string? MarketingText { get; set; }
        public string? FinePrint { get; set; }
        public DateTime? Expiration { get; set; }
        public string? AdminNote { get; set; }
        public string? CashierPOSMessage { get; set; }
        public IncentiveEntitlementStatus Status { get; set; } = IncentiveEntitlementStatus.Accumulating;
        public DateTime? StatusDate { get; set; }
        public string? StatusNote { get; set; }
    }
}
