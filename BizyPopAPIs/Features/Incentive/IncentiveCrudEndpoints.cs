namespace BizyPopAPIs.Features.Incentive
{
    public static class IncentiveCrudEndpoints
    {
        public static IEndpointRouteBuilder MapIncentiveCrudEndpoints(this IEndpointRouteBuilder app)
        {
            var incentiveGroup = app.MapGroup("/api/incentives").WithTags("Incentives").RequireAuthorization();

            incentiveGroup.MapPromoEndpoints();
            incentiveGroup.MapStampEndpoints();
            incentiveGroup.MapGiftCardEndpoints();
            incentiveGroup.MapVipEndpoints();
            incentiveGroup.MapCouponEndpoints();

            return app;
        }
    }
}
