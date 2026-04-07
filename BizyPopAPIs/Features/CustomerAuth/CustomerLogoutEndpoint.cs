using Application.Services.Interfaces;
using Common.Models;
using Microsoft.AspNetCore.Mvc;

namespace BizyPopAPIs.Features.CustomerAuth
{
    public static class CustomerLogoutEndpoint
    {
        public static IEndpointRouteBuilder MapCustomerLogout(this IEndpointRouteBuilder app)
        {
            app.MapPost("/logout", async (
                [FromServices] ICustomerAuthService customerAuthService) =>
            {
                await customerAuthService.LogoutAsync().ConfigureAwait(false);
                return Results.Ok(ApiResponse.SuccessResponse("Logged out successfully."));
            })
            .RequireAuthorization()
            .Produces<ApiResponseModel>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithDescription("Revoke the current customer's refresh token and end the session.")
            .WithName("CustomerLogout");

            return app;
        }
    }
}
