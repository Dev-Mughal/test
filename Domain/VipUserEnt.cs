namespace Domain
{
    /// <summary>
    /// Maps to <c>30B_VIPUserEnt</c> — a customer's VIP membership at a specific business.
    /// Linked to the business directly (not to VipBizDef) as per the Access DB schema.
    /// </summary>
    public class VipUserEnt
    {
        public long Id { get; set; }
        public long UserId { get; set; }
        public int BusinessId { get; set; }
        public string QRCode { get; set; } = null!;
        public IncentiveEntitlementStatus Status { get; set; }
        public DateTime? StatusDate { get; set; }
        public string? StatusNote { get; set; }
        public string? CashierNote { get; set; }
        public DateTime LastUpdated { get; set; }
        public DateTime Created { get; set; }

        // Per-user schedule overrides; null means the VipBizDef defaults apply.
        public int? StartDay { get; set; }
        public int? EndDay { get; set; }
        public int? DailyStartHour { get; set; }
        public int? DailyEndHour { get; set; }

        // Navigation Properties
        public virtual Customer Customer { get; set; } = null!;
        public virtual Business Business { get; set; } = null!;
        public virtual ICollection<VipAction> VipActions { get; set; } = [];
    }
}
