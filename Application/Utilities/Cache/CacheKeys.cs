namespace Application.Utilities.Cache
{
    /// <summary>
    /// Centralised cache-key constants and factory methods.
    /// Naming convention:  {domain}:{variant}:{discriminator...}
    /// </summary>
    public static class CacheKeys
    {
        // ??? Static lookup tables (shared across all users) ??????????????????
        public const string CouponTypesLookup  = "coupon:types:lookup";
        public const string BizCategoryLookup  = "biz:cat:lookup";
        public const string BizCategoryGrouped = "biz:cat:grouped";

        // ??? Business detail (per-id) ?????????????????????????????????????????
        public static string Business(int id) => $"biz:{id}";

        // Customer-facing business detail — separate key prefix so admin and
        // customer caches can be invalidated independently.
        public static string CustomerBusinessDetail(int id) => $"cust-biz:detail:{id}";

        // Customer-facing paginated incentive list for a given business
        public static string CustomerIncentivesList(int businessId, int page, int size)
            => $"cust-incentives:{businessId}:{page}:{size}";

        // ??? Coupon detail / list (per-business, grouped for bulk invalidation)
        public static string CouponById(int businessId, long couponId)
            => $"coupon:{businessId}:{couponId}";

        public static string CouponsList(int businessId, int page, int size)
            => $"coupons:{businessId}:{page}:{size}";

        /// <summary>Group key used to invalidate all coupon entries for a business at once.</summary>
        public static string CouponsGroup(int businessId) => $"coupon-group:{businessId}";

        // ??? Business feed (short-lived, parameterised per-request) ??????????
        public static string BusinessFeedSearch(string term, int? catId, int page, int size)
            => $"feed:search:{term.ToLowerInvariant()}:{catId ?? 0}:{page}:{size}";

        public static string BusinessFeedGeoByLocationKey(string locationKey, int? catId, int page, int size)
            => $"feed:geo-key:{locationKey.ToLowerInvariant()}:{catId ?? 0}:{page}:{size}";

        public static string BusinessFeedLocation(double lat, double lng, double radius, int? catId, int page, int size)
            => $"feed:loc:{Math.Round(lat, 3):F3}:{Math.Round(lng, 3):F3}:{Math.Round(radius, 1):F1}:{catId ?? 0}:{page}:{size}";

        public static string BusinessFeedTimeZone(string timeZone, int? catId, int page, int size)
            => $"feed:tz:{timeZone}:{catId ?? 0}:{page}:{size}";
    }
}
