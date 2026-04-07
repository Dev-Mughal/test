using Application.Services.Interfaces;
using Common.Models;
using Microsoft.AspNetCore.Mvc;

namespace BizyPopAPIs.Features.Auth
{
    public static class LogOutEndpoint
    {
        public static IEndpointRouteBuilder MapLogOut(this IEndpointRouteBuilder app)
        {
            app.MapPost("/logout", async
                (
                [FromServices] IAuthService authService
                ) =>
            {
                await authService.LogoutAsync().ConfigureAwait(false);
                return Results.Ok(
                    ApiResponse.SuccessResponse(message: "Logged out Successfully."));
            })
            .Produces<ApiResponseModel>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithDescription("Endpoint to log out the current user and revoke the refresh token.")
            .RequireAuthorization()
            .WithName("LogOut");
            return app;
        }
    }
}
