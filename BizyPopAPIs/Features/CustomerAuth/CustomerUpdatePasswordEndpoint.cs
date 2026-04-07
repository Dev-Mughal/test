using Application.Services.Interfaces;
using BizyPopAPIs.Utilities.CustomAttribute;
using Common.Features.Customer.Auth.DTOs;
using Common.Models;
using Microsoft.AspNetCore.Mvc;

namespace BizyPopAPIs.Features.CustomerAuth
{
    public static class CustomerUpdatePasswordEndpoint
    {
        public static IEndpointRouteBuilder MapCustomerUpdatePassword(this IEndpointRouteBuilder app)
        {
            app.MapPut("/password", async (
                [FromBody] CustomerUpdatePasswordDto request,
                [FromServices] ICustomerAuthService customerAuthService) =>
            {
                await customerAuthService.UpdatePasswordAsync(request).ConfigureAwait(false);
                return Results.Ok(ApiResponse.SuccessResponse("Customer password updated successfully."));
            })
            .RequireAuthorization()
            .AddEndpointFilter<ValidationFilter<CustomerUpdatePasswordDto>>()
            .Accepts<CustomerUpdatePasswordDto>("application/json")
            .Produces<ApiResponseModel<string>>(StatusCodes.Status200OK)
            .ProducesValidationProblem(StatusCodes.Status422UnprocessableEntity)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .WithDescription("Update a customer's password.")
            .WithName("CustomerUpdatePassword");

            return app;
        }
    }
}
