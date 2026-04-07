namespace Domain
{
    /// <summary>Maps to <c>30C_VipAction</c> — a single VIP access transaction or validation event.</summary>
    public class VipAction
    {
        public long Id { get; set; }
        public long EntitlementId { get; set; }
        public long CashierId { get; set; }
        public DateTime TransactionDate { get; set; }

        // Populated only for transfer actions; maps to "TransferRecieverUserID" (original schema spelling preserved).
        public long? TransferReceiverUserId { get; set; }
        public bool IsValid { get; set; }

        // Navigation Properties
        public virtual VipUserEnt VipUserEnt { get; set; } = null!;
    }
}
