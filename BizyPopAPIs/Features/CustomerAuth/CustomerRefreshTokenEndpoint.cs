using Application.Services.Interfaces;
using Application.Utilities.TokenManager;
using BizyPopAPIs.Utilities.CustomAttribute;
using Common.Features.Auth.RefreshToken;
using Common.Models;
using Microsoft.AspNetCore.Mvc;

namespace BizyPopAPIs.Features.CustomerAuth
{
    public static class CustomerRefreshTokenEndpoint
    {
        public static IEndpointRouteBuilder MapCustomerRefreshToken(this IEndpointRouteBuilder app)
        {
            app.MapPost("/refresh-token", async (
                [FromBody] RefreshTokenDto request,
                [FromServices] ICustomerAuthService customerAuthService) =>
            {
                var result = await customerAuthService.RefreshTokenAsync(request).ConfigureAwait(false);
                return Results.Ok(ApiResponse.SuccessResponse(result, "Token refreshed successfully."));
            })
            .AddEndpointFilter<ValidationFilter<RefreshTokenDto>>()
            .Accepts<RefreshTokenDto>("application/json")
            .Produces<ApiResponseModel<TokenResponse>>(StatusCodes.Status200OK)
            .ProducesValidationProblem(StatusCodes.Status422UnprocessableEntity)
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithDescription("Exchange a valid customer refresh token for a new access token.")
            .WithName("CustomerRefreshToken");

            return app;
        }
    }
}
