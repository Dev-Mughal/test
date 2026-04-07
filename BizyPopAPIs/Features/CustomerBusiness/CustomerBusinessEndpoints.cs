namespace BizyPopAPIs.Features.CustomerBusiness
{
    public static class CustomerBusinessEndpoints
    {
        public static void MapCustomerBusinessEndpoints(this IEndpointRouteBuilder app)
        {
            var group = app.MapGroup("/api/customer/businesses")
                .WithTags("Customer Business");

            group.MapGetBusinessDetail();
            group.MapGetBusinessIncentives();
        }
    }
}
