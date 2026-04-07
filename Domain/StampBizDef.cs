namespace Domain
{
    /// <summary>Maps to <c>12A_StampBizDef</c> — the business-facing stamp card definition.</summary>
    public class StampBizDef
    {
        public long Id { get; set; }
        public int BusinessId { get; set; }
        public string RewardDesc { get; set; } = null!;
        public string QRCode { get; set; } = string.Empty;
        public string? PhotoUrl { get; set; }
        public int StampGoal { get; set; }
        public string? GoalReachedMessage { get; set; }
        public string? FinePrint { get; set; }
        public string? AdminNote { get; set; }
        public string? CashierPOSMessage { get; set; }
        public int? MaxStampPerDay { get; set; }

        // Navigation Properties
        public virtual Business Business { get; set; } = null!;
        public virtual ICollection<StampUserEnt> StampUserEnts { get; set; } = [];
    }
}
