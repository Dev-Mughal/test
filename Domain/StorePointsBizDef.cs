namespace Domain
{
    /// <summary>Maps to <c>22A_StorePointsBizDef</c> — business-side store points definition.</summary>
    public class StorePointsBizDef
    {
        public long Id { get; set; }
        public int BusinessId { get; set; }
        public string Description { get; set; } = null!;
        public string QRCode { get; set; } = string.Empty;
        public string? PhotoUrl { get; set; }
        public string? FinePrint { get; set; }
        public string? AdminNote { get; set; }
        public string? CashierPOSMessage { get; set; }
        public IncentiveEntitlementStatus Status { get; set; }
        public DateTime? StatusDate { get; set; }
        public string? StatusNote { get; set; }
        public int DollarPointRatio { get; set; }

        // Navigation Properties
        public virtual Business Business { get; set; } = null!;
        public virtual ICollection<StorePointUserEnt> StorePointUserEnts { get; set; } = [];
    }
}
