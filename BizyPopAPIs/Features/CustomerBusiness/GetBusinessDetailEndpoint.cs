using Application.Services.Interfaces;
using Common.Exceptions;
using Common.Features.Customer.Business;
using Common.Models;
using Microsoft.AspNetCore.Mvc;

namespace BizyPopAPIs.Features.CustomerBusiness
{
    public static class GetBusinessDetailEndpoint
    {
        public static IEndpointRouteBuilder MapGetBusinessDetail(this IEndpointRouteBuilder app)
        {
            app.MapGet("/{businessId:int}", async (
                int businessId,
                [FromServices] IBusinessService businessService) =>
            {
                var detail = await businessService
                    .GetBusinessDetailAsync(businessId)
                    .ConfigureAwait(false);

                if (detail is null)
                    throw new ResourceNotFoundException($"Business with ID '{businessId}' was not found.");

                return Results.Ok(ApiResponse.SuccessResponse(detail, "Business details retrieved successfully."));
            })
            .AllowAnonymous()
            .Produces<ApiResponseModel<BusinessDetailDto>>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithDescription("Returns static business info (contact, address, map coordinates, active incentive count) for the customer detail page.")
            .WithName("GetCustomerBusinessDetail");

            return app;
        }
    }
}
