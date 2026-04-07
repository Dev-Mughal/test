using Application.Services.Interfaces;
using BizyPopAPIs.Utilities.CustomAttribute;
using Common.Features.Customer.Auth.DTOs;
using Common.Models;
using Microsoft.AspNetCore.Mvc;

namespace BizyPopAPIs.Features.CustomerAuth
{
    public static class CustomerUpdateProfileEndpoint
    {
        public static IEndpointRouteBuilder MapCustomerUpdateProfile(this IEndpointRouteBuilder app)
        {
            app.MapPut("/profile", async (
                [FromForm] CustomerUpdateProfileDto request,
                [FromServices] ICustomerAuthService customerAuthService) =>
            {
                await customerAuthService.UpdateProfileAsync(request).ConfigureAwait(false);
                return Results.Ok(ApiResponse.SuccessResponse("Customer profile updated successfully."));
            })
            .RequireAuthorization()
            .DisableAntiforgery()
            .AddEndpointFilter<ValidationFilter<CustomerUpdateProfileDto>>()
            .Accepts<CustomerUpdateProfileDto>("multipart/form-data")
            .Produces<ApiResponseModel<string>>(StatusCodes.Status200OK)
            .ProducesValidationProblem(StatusCodes.Status422UnprocessableEntity)
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .WithDescription("Update a customer profile with an optional profile photo.")
            .WithName("CustomerUpdateProfile");

            return app;
        }
    }
}
