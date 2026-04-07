using Application.Services.Interfaces;
using Application.Utilities.TokenManager;
using BizyPopAPIs.Utilities.CustomAttribute;
using Common.Features.Auth.Login;
using Common.Models;
using Microsoft.AspNetCore.Mvc;

namespace BizyPopAPIs.Features.Auth
{
    public static class LoginEndpoint
    {
        public static IEndpointRouteBuilder MapLogIn(this IEndpointRouteBuilder app)
        {
            app.MapPost("/login", async
                (
                [FromBody] LoginDto request,
                [FromServices] IAuthService authService
                ) =>
            {
                TokenResponse result = await authService.LoginAsync(request).ConfigureAwait(false);
                return Results.Ok(
                    ApiResponse.SuccessResponse(result, message: "Login successful."));
            })
            .AddEndpointFilter<ValidationFilter<LoginDto>>()
            .Accepts<LoginDto>("application/json")
            .Produces<ApiResponseModel<TokenResponse>>(StatusCodes.Status200OK)
            .ProducesValidationProblem(StatusCodes.Status422UnprocessableEntity)
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithDescription("Endpoint to login with credentials.")
            .WithName("Login");

            return app;
        }
    }
}
