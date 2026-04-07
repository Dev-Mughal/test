namespace Domain
{
    /// <summary>Maps to <c>30A_VIPBizDef</c> — the business-facing VIP access program definition.</summary>
    public class VipBizDef
    {
        public long Id { get; set; }
        public int BusinessId { get; set; }
        public string Description { get; set; } = null!;
        public string QRCode { get; set; } = string.Empty;
        public string? PhotoUrl { get; set; }
        public string? DesignData { get; set; }
        public string? FinePrint { get; set; }
        public int? DefaultStartDay { get; set; }
        public int? DefaultEndDay { get; set; }
        public int? DefaultDailyStartHour { get; set; }
        public int? DefaultDailyEndHour { get; set; }
        public DateTime? Expiration { get; set; }
        public string? AdminNote { get; set; }
        public string? CashierPOSMessage { get; set; }

        // Navigation Properties
        public virtual Business Business { get; set; } = null!;
    }
}
