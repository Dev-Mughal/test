namespace BizyPopAPIs.Features.Coupon
{
    public static class CouponEndpoints
    {
        public static void MapCouponEndpoints(this IEndpointRouteBuilder app)
        {
            var couponGroup = app.MapGroup("/api/coupons").WithTags("Coupons");

            couponGroup.MapLookupCouponTypes();
            couponGroup.MapGetAllCoupons();
            couponGroup.MapCreateCoupon();
            couponGroup.MapUpdateCoupon();
            couponGroup.MapGetCouponById();
        }
    }
}
