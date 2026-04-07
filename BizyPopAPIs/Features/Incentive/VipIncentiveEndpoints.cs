namespace BizyPopAPIs.Features.Incentive
{
    public static class VipIncentiveEndpoints
    {
        public static RouteGroupBuilder MapVipEndpoints(this RouteGroupBuilder incentivesGroup)
        {
            var vipGroup = incentivesGroup.MapGroup("/vips").WithTags("Incentives/VIP");
            vipGroup.MapCreateVipIncentive();
            vipGroup.MapGetVipIncentiveById();
            vipGroup.MapGetAllVipIncentives();
            vipGroup.MapUpdateVipIncentive();
            vipGroup.MapDeleteVipIncentive();
            return incentivesGroup;
        }
    }
}
