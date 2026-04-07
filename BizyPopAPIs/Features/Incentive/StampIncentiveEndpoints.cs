namespace BizyPopAPIs.Features.Incentive
{
    public static class StampIncentiveEndpoints
    {
        public static RouteGroupBuilder MapStampEndpoints(this RouteGroupBuilder incentivesGroup)
        {
            var stampGroup = incentivesGroup.MapGroup("/stamps").WithTags("Incentives/Stamps");
            stampGroup.MapCreateStampIncentive();
            stampGroup.MapGetStampIncentiveById();
            stampGroup.MapGetAllStampIncentives();
            stampGroup.MapUpdateStampIncentive();
            stampGroup.MapDeleteStampIncentive();
            return incentivesGroup;
        }
    }
}
