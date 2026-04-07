namespace Domain
{
    public class Customer
    {
        public long CustomerId { get; set; }

        // Authentication
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string PasswordHash { get; set; } = null!;

        // Profile
        public string? ProfilePhotoUrl { get; set; }
        public bool IsActive { get; set; }
        public string TimeZone { get; set; } = null!;

        // JWT Refresh Token
        public string? RefreshToken { get; set; }
        public DateTime? RefreshTokenExpiryTime { get; set; }

        // Audit
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        // Navigation Properties
        public virtual ICollection<CustomerCoupon> CustomerCoupons { get; set; } = [];
        public virtual ICollection<BizDollarUserBalance> BizDollarUserBalances { get; set; } = [];
        public virtual ICollection<BizDollarAction> BizDollarActions { get; set; } = [];
        public virtual ICollection<PromoUserUsage> PromoUserUsages { get; set; } = [];
        public virtual ICollection<StampUserEnt> StampUserEnts { get; set; } = [];
        public virtual ICollection<GiftCardUserEnt> GiftCardUserEnts { get; set; } = [];
        public virtual ICollection<StoreCreditUserEnt> StoreCreditUserEnts { get; set; } = [];
        public virtual ICollection<StorePointUserEnt> StorePointUserEnts { get; set; } = [];
        public virtual ICollection<VipUserEnt> VipUserEnts { get; set; } = [];
    }
}
