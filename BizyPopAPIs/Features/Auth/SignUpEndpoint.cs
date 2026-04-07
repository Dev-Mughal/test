using Application.Services.Interfaces;
using Application.Utilities.TokenManager;
using BizyPopAPIs.Utilities.CustomAttribute;
using Common.Features.Auth.SignUp.DTOs;
using Common.Models;
using Microsoft.AspNetCore.Mvc;

namespace BizyPopAPIs.Features.Auth
{
    public static class SignUpEndpoint
    {
        public static IEndpointRouteBuilder MapSignUp(this IEndpointRouteBuilder app)
        {
            app.MapPost("/signup", async (
                [FromForm] SignUpDto request,
                [FromServices] IAuthService authService) =>
            {
                TokenResponse result = await authService.SignUpAsync(request, request.BusinessImage).ConfigureAwait(false);
                return Results.Created<ApiResponseModel<TokenResponse>>(
                    "/signup",
                    ApiResponse.SuccessResponse<TokenResponse>(result, message: "Signup successful."));
            })
            .DisableAntiforgery()
            .AddEndpointFilter<ValidationFilter<SignUpDto>>()
            .Accepts<SignUpDto>("multipart/form-data")
            .Produces<ApiResponseModel<TokenResponse>>(StatusCodes.Status201Created)
            .ProducesValidationProblem(StatusCodes.Status422UnprocessableEntity)
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithDescription("Endpoint to register a new business and its associated user with optional business email, website link, address line 2, and business image upload.")
            .WithName("Signup");

            return app;
        }
    }
}
