using Application.Services.IncentiveServices.Interfaces;
using Common.Features.Coupon.DTOs;
using Common.Models;
using Infrastructure.Pagination;
using Microsoft.AspNetCore.Mvc;

namespace BizyPopAPIs.Features.Coupon
{
    public static class GetAllCouponsEndpoint
    {
        public static IEndpointRouteBuilder MapGetAllCoupons(this IEndpointRouteBuilder app)
        {
            app.MapGet("/", async (
                [AsParameters] CouponPaginationRequest request,
                [FromQuery] int? businessId,
                [FromServices] ICouponService couponService) =>
            {
                var result = await couponService.GetAllCouponsAsync(request, businessId).ConfigureAwait(false);


                return Results.Ok(ApiResponse.SuccessResponse(result, message: "Coupons retrieved successfully."));
            })
            .RequireAuthorization()
            .Produces<ApiResponseModel<PaginationResponse<CouponListItemDto>>>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithDescription("Endpoint to retrieve all coupons for the selected business with pagination. `businessId` query is optional and falls back to token context.")
            .WithName("GetAllCoupons");

            return app;
        }
    }
}
