namespace Domain
{
    /// <summary>Maps to <c>22T_StorePointTransfer</c> — transfer between store point entitlements.</summary>
    public class StorePointTransfer
    {
        public long Id { get; set; }
        public long SenderEntitlementId { get; set; }
        public long ReceiverEntitlementId { get; set; }
        public string Reason { get; set; } = null!;
        public long CashierId { get; set; }

        // Navigation Properties
        public virtual StorePointUserEnt SenderEntitlement { get; set; } = null!;
        public virtual StorePointUserEnt ReceiverEntitlement { get; set; } = null!;
    }
}
