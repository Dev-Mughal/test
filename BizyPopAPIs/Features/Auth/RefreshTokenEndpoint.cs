using Application.Utilities.TokenManager;
using Common.Mappers;
using Common.Features.Auth.RefreshToken;
using Common.Models;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using BizyPopAPIs.Utilities.CustomAttribute;
using Application.Services.Interfaces;

namespace BizyPopAPIs.Features.Auth
{
    public static class RefreshTokenEndpoint
    {
        public static IEndpointRouteBuilder MapRefreshToken(this IEndpointRouteBuilder app)
        {
            app.MapPost("/refresh-token", async
                (
                    [FromBody] RefreshTokenDto refreshToken,
                    IAuthService authService
                ) =>
            {
                TokenResponse result = await authService.RefreshTokenAsync(refreshToken).ConfigureAwait(false);
                return Results.Ok(
                    ApiResponse.SuccessResponse(result, message: "Token refreshed Successfully."));
            })
            .AddEndpointFilter<ValidationFilter<RefreshTokenDto>>()
            .Accepts<RefreshTokenDto>("application/json")
            .Produces<ApiResponseModel<TokenResponse>>(StatusCodes.Status200OK)
            .ProducesValidationProblem(StatusCodes.Status422UnprocessableEntity)
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .Produces(StatusCodes.Status401Unauthorized)
            .WithDescription("Endpoint to refresh authentication token.")
            .WithName("RefreshToken");
            return app;
        }
    }
}
