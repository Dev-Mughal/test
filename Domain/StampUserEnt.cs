namespace Domain
{
    /// <summary>Maps to <c>12B_StampUserEnt</c> — a customer's stamp card entitlement for a stamp program.</summary>
    public class StampUserEnt
    {
        public long Id { get; set; }
        public IncentiveEntitlementStatus Status { get; set; }
        public long UserId { get; set; }
        public long StampId { get; set; }
        public string QRCode { get; set; } = null!;
        public DateTime? RedeemedDate { get; set; }
        public int StampCount { get; set; }
        public int StampGoal { get; set; }
        public string? CashierNote { get; set; }
        public DateTime LastUpdated { get; set; }
        public DateTime Created { get; set; }

        // Navigation Properties
        public virtual Customer Customer { get; set; } = null!;
        public virtual StampBizDef StampBizDef { get; set; } = null!;
        public virtual ICollection<StampAction> StampActions { get; set; } = [];
        public virtual ICollection<StampVoidLog> StampVoidLogs { get; set; } = [];
    }
}
