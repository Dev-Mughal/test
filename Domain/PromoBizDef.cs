namespace Domain
{
    /// <summary>Maps to <c>11A_PromoBizDef</c> — the business-facing promotion definition.</summary>
    public class PromoBizDef
    {
        public long Id { get; set; }
        public int BusinessId { get; set; }
        public string PromotionDesc { get; set; } = null!;
        public string QRCode { get; set; } = string.Empty;
        public string? PhotoUrl { get; set; }
        public string? FinePrint { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime ExpirationDate { get; set; }
        public string? AdminNote { get; set; }
        public string? CashierPOSMessage { get; set; }
        public string? VoidedReason { get; set; }

        // Navigation Properties
        public virtual Business Business { get; set; } = null!;
        public virtual ICollection<PromoUserUsage> PromoUserUsages { get; set; } = [];
    }
}
