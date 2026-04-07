namespace Domain
{
    /// <summary>Maps to <c>20A_GiftCardBizDef</c> — the business-facing gift card program definition.</summary>
    public class GiftCardBizDef
    {
        public long Id { get; set; }
        public int BusinessId { get; set; }
        public string Title { get; set; } = null!;
        public string QRCode { get; set; } = string.Empty;
        public string? PhotoUrl { get; set; }
        public int GiftCardValue { get; set; }
        public string? MarketingText { get; set; }
        public string? FinePrint { get; set; }
        public DateTime? Expiration { get; set; }
        public string? AdminNote { get; set; }
        public string? CashierPOSMessage { get; set; }
        public IncentiveEntitlementStatus Status { get; set; }
        public DateTime? StatusDate { get; set; }
        public string? StatusNote { get; set; }

        // Navigation Properties
        public virtual Business Business { get; set; } = null!;
        public virtual ICollection<GiftCardUserEnt> GiftCardUserEnts { get; set; } = [];
    }
}
