namespace Domain
{
    /// <summary>Maps to <c>20C_GiftCardAction</c> — a single transaction (spend/reload/transfer) on a gift card.</summary>
    public class GiftCardAction
    {
        public long Id { get; set; }
        public long EntitlementId { get; set; }
        public int Amount { get; set; }
        public long CashierId { get; set; }
        public DateTime TransactionDate { get; set; }
        public long UserId { get; set; }

        // Populated only for transfer actions; maps to "TransferRecieverUserID" (original schema spelling preserved).
        public long? TransferReceiverUserId { get; set; }

        // Navigation Properties
        public virtual GiftCardUserEnt GiftCardUserEnt { get; set; } = null!;
    }
}
