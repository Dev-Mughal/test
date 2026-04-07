using Application.Services.Interfaces;
using BizyPopAPIs.Utilities.CustomAttribute;
using Common.Features.Auth.Login;
using Common.Models;
using Microsoft.AspNetCore.Mvc;

namespace BizyPopAPIs.Features.CustomerAuth
{
    public static class CustomerEmailVerificationEndpoint
    {
        public static IEndpointRouteBuilder MapCustomerEmailVerification(this IEndpointRouteBuilder app)
        {
            app.MapPost("/email-exist", async (
                [FromBody] EmailVerificationDto request,
                [FromServices] ICustomerAuthService customerAuthService) =>
            {
                var result = await customerAuthService.IsEmailAlreadyExistsAsync(request).ConfigureAwait(false);
                return Results.Ok(ApiResponse.SuccessResponse(result, "Email verification successful."));
            })
            .AddEndpointFilter<ValidationFilter<EmailVerificationDto>>()
            .Accepts<EmailVerificationDto>("application/json")
            .Produces<ApiResponseModel<EmailVerificationResultDto>>(StatusCodes.Status200OK)
            .ProducesValidationProblem(StatusCodes.Status422UnprocessableEntity)
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithDescription("Check whether an email address is already registered as a customer.")
            .WithName("CustomerEmailVerification");

            return app;
        }
    }
}
