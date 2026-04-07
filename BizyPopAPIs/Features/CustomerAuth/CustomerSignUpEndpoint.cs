using Application.Services.Interfaces;
using Application.Utilities.TokenManager;
using BizyPopAPIs.Utilities.CustomAttribute;
using Common.Features.Customer.Auth.DTOs;
using Common.Features.Customer.Auth.Validators;
using Common.Models;
using Microsoft.AspNetCore.Mvc;

namespace BizyPopAPIs.Features.CustomerAuth
{
    public static class CustomerSignUpEndpoint
    {
        public static IEndpointRouteBuilder MapCustomerSignUp(this IEndpointRouteBuilder app)
        {
            app.MapPost("/signup", async (
                [FromForm] CustomerSignUpDto request,
                [FromServices] ICustomerAuthService customerAuthService) =>
            {
                var result = await customerAuthService.SignUpAsync(request).ConfigureAwait(false);
                return Results.Created(
                    "/api/customer/auth/signup",
                    ApiResponse.SuccessResponse<TokenResponse>(result, "Customer account created successfully."));
            })
            .DisableAntiforgery()
            .AddEndpointFilter<ValidationFilter<CustomerSignUpDto>>()
            .Accepts<CustomerSignUpDto>("multipart/form-data")
            .Produces<ApiResponseModel<TokenResponse>>(StatusCodes.Status201Created)
            .ProducesValidationProblem(StatusCodes.Status422UnprocessableEntity)
            .ProducesProblem(StatusCodes.Status409Conflict)
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithDescription("Register a new customer account with optional profile photo and receive an access token.")
            .WithName("CustomerSignUp");

            return app;
        }
    }
}
