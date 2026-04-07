namespace BizyPopAPIs.Features.Incentive
{
    public static class GiftCardIncentiveEndpoints
    {
        public static RouteGroupBuilder MapGiftCardEndpoints(this RouteGroupBuilder incentivesGroup)
        {
            var giftCardGroup = incentivesGroup.MapGroup("/gift-cards").WithTags("Incentives/GiftCards");
            giftCardGroup.MapCreateGiftCardIncentive();
            giftCardGroup.MapGetGiftCardIncentiveById();
            giftCardGroup.MapGetAllGiftCardIncentives();
            giftCardGroup.MapUpdateGiftCardIncentive();
            giftCardGroup.MapDeleteGiftCardIncentive();
            return incentivesGroup;
        }
    }
}
