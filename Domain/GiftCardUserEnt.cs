namespace Domain
{
    /// <summary>Maps to <c>20B_GiftCardUserEnt</c> — a customer's gift card entitlement (wallet entry).</summary>
    public class GiftCardUserEnt
    {
        public long Id { get; set; }
        public long UserId { get; set; } // Customers are "users" in the system, so this is the customer ID.
        public long GiftCardId { get; set; }
        public string QRCode { get; set; } = null!;
        public int GiftCardValue { get; set; }
        public int GiftCardBalance { get; set; }
        public string? CashierNote { get; set; }
        public DateTime LastUpdated { get; set; }
        public DateTime Created { get; set; }

        // Navigation Properties
        public virtual Customer Customer { get; set; } = null!;
        public virtual GiftCardBizDef GiftCardBizDef { get; set; } = null!;
        public virtual ICollection<GiftCardAction> GiftCardActions { get; set; } = [];
    }
}
