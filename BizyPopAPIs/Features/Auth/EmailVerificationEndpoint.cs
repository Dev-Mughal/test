using Application.Services.Interfaces;
using BizyPopAPIs.Utilities.CustomAttribute;
using Common.Features.Auth.Login;
using Common.Models;
using Microsoft.AspNetCore.Mvc;

namespace BizyPopAPIs.Features.Auth
{
    public static class EmailVerificationEndpoint
    {
        public static IEndpointRouteBuilder MapEmailVerification(this IEndpointRouteBuilder app)
        {
            app.MapPost("/email-exist", async
                (
                [FromBody] EmailVerificationDto request,
                [FromServices] IAuthService authService
                ) =>
            {
                var result = await authService.IsEmailAlreadyExists(request).ConfigureAwait(false);
                return Results.Ok(
                    ApiResponse.SuccessResponse(result, message: "Email verification successful."));
            })
            .AddEndpointFilter<ValidationFilter<EmailVerificationDto>>()
            .Accepts<EmailVerificationDto>("application/json")
            .Produces<ApiResponseModel<EmailVerificationResultDto>>(StatusCodes.Status200OK)
            .ProducesValidationProblem(StatusCodes.Status422UnprocessableEntity)
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithDescription("Endpoint to verify email.")
            .WithName("EmailVerification");

            return app;
        }
    }
}
