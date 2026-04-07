namespace Domain
{
    public class Coupon
    {
        public long Id { get; set; }
        public int BusinessId { get; set; }
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string? PhotoUrl { get; set; }
        public string QRCode { get; set; } = string.Empty;
        public DateTime StartDateTime { get; set; }
        public DateTime EndDateTime { get; set; }
        public DateTime ExpirationTime { get; set; }
        public DateTime CreatedOn { get; set; }
        public bool IsActive { get; set; }
        public bool IsFeatured { get; set; }

        // Navigation Properties
        public virtual Business Business { get; set; } = null!;
        public virtual ICollection<CustomerCoupon> CustomerCoupons { get; set; } = [];
    }
}
