namespace Domain
{
    /// <summary>Maps to <c>11B_PromoUserUsage</c> — tracks a customer's usage of a promotion.</summary>
    public class PromoUserUsage
    {
        public long Id { get; set; }
        public long UserId { get; set; }
        public long PromotionId { get; set; }
        public string QRCode { get; set; } = null!;
        public DateTime LastUpdated { get; set; }
        public DateTime Created { get; set; }
        public DateTime? UsedDate { get; set; }

        // Navigation Properties
        public virtual Customer Customer { get; set; } = null!;
        public virtual PromoBizDef PromoBizDef { get; set; } = null!;
    }
}
