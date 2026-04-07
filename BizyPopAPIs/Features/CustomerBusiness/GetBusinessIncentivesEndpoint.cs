using Application.Services.IncentiveServices.Interfaces;
using Common.Features.Customer.Business;
using Common.Models;
using Infrastructure.Pagination;
using Microsoft.AspNetCore.Mvc;

namespace BizyPopAPIs.Features.CustomerBusiness
{
    public static class GetBusinessIncentivesEndpoint
    {
        public static IEndpointRouteBuilder MapGetBusinessIncentives(this IEndpointRouteBuilder app)
        {
            app.MapGet("/{businessId:int}/incentives", async (
                int businessId,
                [AsParameters] CouponPaginationRequest request,
                [FromServices] ICouponService couponService) =>
            {
                var result = await couponService
                    .GetBusinessIncentivesForCustomerAsync(businessId, request)
                    .ConfigureAwait(false);

                return Results.Ok(ApiResponse.SuccessResponse(result, "Incentives retrieved successfully."));
            })
            .AllowAnonymous()
            .Produces<ApiResponseModel<PaginationResponse<CustomerIncentiveItemDto>>>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithDescription("Returns paginated active incentives for a business. Supports all incentive types.")
            .WithName("GetBusinessIncentivesForCustomer");

            return app;
        }
    }
}
