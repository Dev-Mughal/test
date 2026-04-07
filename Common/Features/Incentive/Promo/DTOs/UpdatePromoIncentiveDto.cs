using Microsoft.AspNetCore.Http;

namespace Common.Features.Incentive.Promo.DTOs
{
    public class UpdatePromoIncentiveDto
    {
        public string PromotionDesc { get; set; } = null!;
        public DateTime StartDate { get; set; }
        public DateTime ExpirationDate { get; set; }
        public IFormFile? Photo { get; set; }
        public string? FinePrint { get; set; }
        public string? AdminNote { get; set; }
        public string? CashierPOSMessage { get; set; }
        public string? VoidedReason { get; set; }
    }
}
