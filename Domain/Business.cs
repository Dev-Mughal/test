namespace Domain
{
    public class Business
    {
        // Business Contact properties
        public int BusinessId { get; set; }
        public string BusinessName { get; set; } = null!;
        public string BusinessURL { get; set; } = null!;
        public string BusinessEmail { get; set; } = null!;
        public string BusinessPhone { get; set; } = null!;
        public short CountryCode { get; set; }

        // Address Properties
        public string StreetAddress { get; set; } = null!;
        public string? AddressLine2 { get; set; }
        public string Country { get; set; } = null!;
        public double Longitude { get; set; }
        public double Latitude { get; set; }

        // Geo lookup foreign keys — City/State resolved via L50; ZipCode via L51.
        // Stored as IDs to enable index-only searches without JOINs (see L50/L51 Jira).
        public long StateCityId { get; set; }
        public long StateCityZipId { get; set; }

        // Image Property
        public string? BusinessImageUrl { get; set; }

        // Other Fields
        public DateTime CreatedOn { get; set; }

        // Foreign Keys
        public int CategoryId { get; set; }

        // Navigation Properties
        public virtual GeoCity GeoCity { get; set; } = null!;
        public virtual GeoZipCode GeoZipCode { get; set; } = null!;
        public virtual BusinessCategory Category { get; set; } = null!;
        public virtual ICollection<Coupon> Coupons { get; set; } = [];
        public virtual ICollection<PromoBizDef> PromoBizDefs { get; set; } = [];
        public virtual ICollection<StampBizDef> StampBizDefs { get; set; } = [];
        public virtual ICollection<GiftCardBizDef> GiftCardBizDefs { get; set; } = [];
        public virtual ICollection<StoreCreditBizDef> StoreCreditBizDefs { get; set; } = [];
        public virtual ICollection<StorePointsBizDef> StorePointsBizDefs { get; set; } = [];
        public virtual ICollection<VipBizDef> VipBizDefs { get; set; } = [];
        public virtual ICollection<BizDollarAction> BizDollarActions { get; set; } = [];
        public virtual ICollection<RaffleDef> RaffleDefs { get; set; } = [];
        public virtual ICollection<BusinessUserBusiness> BusinessUserBusinesses { get; set; } = [];
    }
}
