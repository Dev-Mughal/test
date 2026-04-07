namespace Application.Constants
{
    /// <summary>
    /// Table-code constants for incentive QR code generation and scanning.
    /// Format used in QR/track codes is: <c>{tableCode}-{id}</c>.
    /// </summary>
    public static class IncentiveTableCode
    {
        // A tables (business definition)
        public const string CouponA   = "10A";
        public const string PromoA    = "11A";
        public const string StampA    = "12A";
        public const string GiftCardA = "20A";
        public const string VipA      = "30A";
        public const string RaffleA   = "40A";

        // B tables (customer-business)
        public const string CouponB   = "10B";
        public const string PromoB    = "11B";
        public const string StampB    = "12B";
        public const string GiftCardB = "20B";
        public const string VipB      = "30B";
        public const string RaffleB   = "40B";

        // Existing references in business-side generation/services
        public const string Coupon   = CouponA;
        public const string Promo    = PromoA;
        public const string Stamp    = StampA;
        public const string GiftCard = GiftCardA;
        public const string VIP      = VipA;
        public const string Raffle   = RaffleA;

        /// <summary>All valid A-table codes.</summary>
        public static readonly IReadOnlySet<string> AAll = new HashSet<string>
        {
            CouponA, PromoA, StampA, GiftCardA, VipA, RaffleA
        };

        /// <summary>All valid B-table codes.</summary>
        public static readonly IReadOnlySet<string> BAll = new HashSet<string>
        {
            CouponB, PromoB, StampB, GiftCardB, VipB, RaffleB
        };

        /// <summary>All valid table codes used by <see cref="IQRCodeService"/> for validation.</summary>
        public static readonly IReadOnlySet<string> All = new HashSet<string>
        {
            CouponA, PromoA, StampA, GiftCardA, VipA, RaffleA,
            CouponB, PromoB, StampB, GiftCardB, VipB, RaffleB
        };
    }
}
