using Microsoft.AspNetCore.Http;

namespace Common.Features.Incentive.Vip.DTOs
{
    public class CreateVipIncentiveDto
    {
        public string Description { get; set; } = null!;
        public IFormFile? Photo { get; set; }
        public string? DesignData { get; set; }
        public string? FinePrint { get; set; }
        public int? DefaultStartDay { get; set; }
        public int? DefaultEndDay { get; set; }
        public int? DefaultDailyStartHour { get; set; }
        public int? DefaultDailyEndHour { get; set; }
        public DateTime? Expiration { get; set; }
        public string? AdminNote { get; set; }
        public string? CashierPOSMessage { get; set; }
    }
}
