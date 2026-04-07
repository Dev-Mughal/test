using Microsoft.AspNetCore.Http;

namespace Common.Features.Incentive.Stamp.DTOs
{
    public class CreateStampIncentiveDto
    {
        public string RewardDesc { get; set; } = null!;
        public int StampGoal { get; set; }
        public IFormFile? Photo { get; set; }
        public string? GoalReachedMessage { get; set; }
        public string? FinePrint { get; set; }
        public string? AdminNote { get; set; }
        public string? CashierPOSMessage { get; set; }
        public int? MaxStampPerDay { get; set; }
    }
}
