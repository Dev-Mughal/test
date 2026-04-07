namespace Domain
{
    /// <summary>
    /// Maps to <c>20T_GiftCardTransfer</c> — records a gift card balance transfer between two entitlements.
    /// Both FK columns use Restrict delete to prevent orphaned transfer records.
    /// </summary>
    public class GiftCardTransfer
    {
        public long Id { get; set; }
        public long SenderEntitlementId { get; set; }

        // Maps to "RecieverEntitlementID" — original Access DB column spelling preserved.
        public long ReceiverEntitlementId { get; set; }
        public string Reason { get; set; } = null!;
        public long CashierId { get; set; }

        // Navigation Properties
        public virtual GiftCardUserEnt SenderEntitlement { get; set; } = null!;
        public virtual GiftCardUserEnt ReceiverEntitlement { get; set; } = null!;
    }
}
