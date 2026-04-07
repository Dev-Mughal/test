namespace Domain
{
    /// <summary>Maps to <c>21A_StoreCreditBizDef</c> — business-side store credit definition.</summary>
    public class StoreCreditBizDef
    {
        public long Id { get; set; }
        public int BusinessId { get; set; }
        public string QRCode { get; set; } = string.Empty;
        public string? PhotoUrl { get; set; }
        public string? AdminNote { get; set; }
        public string? CashierPOSMessage { get; set; }

        // Navigation Properties
        public virtual Business Business { get; set; } = null!;
        public virtual ICollection<StoreCreditUserEnt> StoreCreditUserEnts { get; set; } = [];
    }
}
