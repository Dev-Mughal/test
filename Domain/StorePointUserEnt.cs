namespace Domain
{
    /// <summary>Maps to <c>22B_StorePointUserEnt</c> — customer store point wallet entry.</summary>
    public class StorePointUserEnt
    {
        public long Id { get; set; }
        public long UserId { get; set; }
        public long StorePointId { get; set; }
        public string QRCode { get; set; } = null!;
        public int StorePointTotal { get; set; }
        public string? CashierNote { get; set; }
        public DateTime LastUpdated { get; set; }
        public DateTime Created { get; set; }

        // Navigation Properties
        public virtual Customer Customer { get; set; } = null!;
        public virtual StorePointsBizDef StorePointsBizDef { get; set; } = null!;
        public virtual ICollection<StorePointAction> StorePointActions { get; set; } = [];
    }
}
