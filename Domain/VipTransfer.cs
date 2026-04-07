namespace Domain
{
    /// <summary>
    /// Maps to <c>30T_VipTransfer</c> — records a VIP entitlement transfer between two customers.
    /// Both FK columns use Restrict delete to prevent orphaned transfer records.
    /// </summary>
    public class VipTransfer
    {
        public long Id { get; set; }
        public long SenderEntitlementId { get; set; }

        // Maps to "RecieverEntitlementID" — original Access DB column spelling preserved.
        public long ReceiverEntitlementId { get; set; }
        public string Reason { get; set; } = null!;
        public long CashierId { get; set; }

        // Navigation Properties
        public virtual VipUserEnt SenderEntitlement { get; set; } = null!;
        public virtual VipUserEnt ReceiverEntitlement { get; set; } = null!;
    }
}
