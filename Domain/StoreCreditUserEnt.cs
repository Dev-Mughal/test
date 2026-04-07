namespace Domain
{
    /// <summary>Maps to <c>21B_StoreCreditUserEnt</c> — customer store credit wallet entry.</summary>
    public class StoreCreditUserEnt
    {
        public long Id { get; set; }
        public long UserId { get; set; }
        public long StoreCredId { get; set; }
        public string QRCode { get; set; } = null!;
        public int StoreCreditBalance { get; set; }
        public string? CashierNote { get; set; }
        public DateTime LastUpdated { get; set; }
        public DateTime Created { get; set; }

        // Navigation Properties
        public virtual Customer Customer { get; set; } = null!;
        public virtual StoreCreditBizDef StoreCreditBizDef { get; set; } = null!;
        public virtual ICollection<StoreCreditAction> StoreCreditActions { get; set; } = [];
    }
}
