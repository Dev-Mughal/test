namespace BizyPopAPIs.Features.CustomerWallet
{
    public static class CustomerWalletEndpoints
    {
        public static void MapCustomerWalletEndpoints(this IEndpointRouteBuilder app)
        {
            var group = app.MapGroup("/api/customer/wallet").WithTags("Customer Wallet");

            group.MapSaveIncentive();
            group.MapGetWallet();
        }
    }
}
