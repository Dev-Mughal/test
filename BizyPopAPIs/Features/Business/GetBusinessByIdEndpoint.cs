using Application.Services.Interfaces;
using Common.Exceptions;
using Common.Features.Business.DTOs;
using Common.Models;
using Microsoft.AspNetCore.Mvc;

namespace BizyPopAPIs.Features.Business
{
    public static class GetBusinessByIdEndpoint
    {
        public static IEndpointRouteBuilder MapGetBusinessById(this IEndpointRouteBuilder app)
        {
            app.MapGet("/{businessId:int}", async (
                int businessId,
                [FromServices] IBusinessService businessService) =>
            {
                var business = await businessService.GetBusinessByIdAsync(businessId)
                    .ConfigureAwait(false);

                if (business is null)
                    throw new ResourceNotFoundException($"Business with ID '{businessId}' was not found.");

                return Results.Ok(ApiResponse.SuccessResponse(business, message: "Business details retrieved successfully."));
            })
            .Produces<ApiResponseModel<BusinessCardDto>>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithDescription("Endpoint to retrieve detailed information about a specific business.")
            .WithName("GetBusinessById");

            return app;
        }
    }
}

