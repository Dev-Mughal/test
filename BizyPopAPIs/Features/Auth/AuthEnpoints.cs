namespace BizyPopAPIs.Features.Auth
{
    public static class AuthEnpoints
    {
        extension(IEndpointRouteBuilder app)
        {
            public void MapAuthEndpoints()
            {
                var authGroup = app.MapGroup("/api/auth").WithTags("Authentication");

                authGroup.MapLogIn();
                authGroup.MapLogOut();
                authGroup.MapRefreshToken();
                authGroup.MapEmailVerification();

                // SignUp uses form-data (separate group to avoid content-type conflict)
                var signupGroup = app.MapGroup("/api/auth").WithTags("Authentication");
                signupGroup.MapSignUp();
            }
        }
    }
}
