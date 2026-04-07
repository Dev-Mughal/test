namespace BizyPopAPIs.Features.Incentive
{
    public static class PromoIncentiveEndpoints
    {
        public static RouteGroupBuilder MapPromoEndpoints(this RouteGroupBuilder incentivesGroup)
        {
            var promoGroup = incentivesGroup.MapGroup("/promos").WithTags("Incentives/Promos");
            promoGroup.MapCreatePromoIncentive();
            promoGroup.MapGetPromoIncentiveById();
            promoGroup.MapGetAllPromoIncentives();
            promoGroup.MapUpdatePromoIncentive();
            promoGroup.MapDeletePromoIncentive();
            return incentivesGroup;
        }
    }
}
