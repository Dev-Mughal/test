namespace BizyPopAPIs.Features.Profile
{
    public static class ProfileEndpoints
    {
        extension(IEndpointRouteBuilder app)
        {
            public void MapProfileEndpoints()
            {
                var profileGroup = app.MapGroup("/api/business").WithTags("Profile");

                profileGroup.MapGetUserSummary();
                profileGroup.MapGetBusinessProfile();
            }
        }
    }
}
