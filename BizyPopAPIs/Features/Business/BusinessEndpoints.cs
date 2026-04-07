namespace BizyPopAPIs.Features.Business
{
    public static class BusinessEndpoints
    {
        public static void MapBusinessEndpoints(this IEndpointRouteBuilder app)
        {
            var businessGroup = app.MapGroup("/api/businesses").WithTags("Businesses");

            businessGroup.MapGetBusinessFeed();
            businessGroup.MapGetBusinessById();
            businessGroup.MapUpdateBusiness();
            businessGroup.MapSearchLocations();
        }
    }
}


