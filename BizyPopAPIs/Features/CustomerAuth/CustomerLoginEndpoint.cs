using Application.Services.Interfaces;
using Application.Utilities.TokenManager;
using BizyPopAPIs.Utilities.CustomAttribute;
using Common.Features.Auth.Login;
using Common.Models;
using Microsoft.AspNetCore.Mvc;

namespace BizyPopAPIs.Features.CustomerAuth
{
    public static class CustomerLoginEndpoint
    {
        public static IEndpointRouteBuilder MapCustomerLogin(this IEndpointRouteBuilder app)
        {
            app.MapPost("/login", async (
                [FromBody] LoginDto request,
                [FromServices] ICustomerAuthService customerAuthService) =>
            {
                var result = await customerAuthService.LoginAsync(request).ConfigureAwait(false);
                return Results.Ok(ApiResponse.SuccessResponse(result, "Login successful."));
            })
            .AddEndpointFilter<ValidationFilter<LoginDto>>()
            .Accepts<LoginDto>("application/json")
            .Produces<ApiResponseModel<TokenResponse>>(StatusCodes.Status200OK)
            .ProducesValidationProblem(StatusCodes.Status422UnprocessableEntity)
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithDescription("Authenticate a customer with email and password.")
            .WithName("CustomerLogin");

            return app;
        }
    }
}
