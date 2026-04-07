namespace Domain
{
    public class CustomerCoupon
    {
        public long Id { get; set; }
        public long CustomerId { get; set; }
        public long CouponId { get; set; }
        public string QRCode { get; set; } = null!;

        public IncentiveEntitlementStatus Status { get; set; }
        public DateTime StatusDate { get; set; }
        public DateTime? DateRedeemed { get; set; }

        // Notes
        public string? CashierNote { get; set; }
        public string? StatusAdminNote { get; set; }
        public string? StatusUserNote { get; set; }

        // Audit
        public DateTime Created { get; set; }
        public DateTime LastUpdated { get; set; }

        // Navigation Properties
        public virtual Customer Customer { get; set; } = null!;
        public virtual Coupon Coupon { get; set; } = null!;
    }
}
