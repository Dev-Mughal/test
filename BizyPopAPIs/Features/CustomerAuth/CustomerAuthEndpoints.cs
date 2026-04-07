namespace BizyPopAPIs.Features.CustomerAuth
{
    public static class CustomerAuthEndpoints
    {
        public static void MapCustomerAuthEndpoints(this IEndpointRouteBuilder app)
        {
            var group = app.MapGroup("/api/customer/auth").WithTags("Customer Authentication");

            group.MapCustomerLogin();
            group.MapCustomerLogout();
            group.MapCustomerRefreshToken();
            group.MapCustomerEmailVerification();
            group.MapCustomerSignUp();
            group.MapGetCustomerProfile();
            group.MapCustomerUpdateProfile();
            group.MapCustomerUpdatePassword();
        }
    }
}
