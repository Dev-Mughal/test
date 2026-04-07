using Application.Services.IncentiveServices.Interfaces;
using Common.Exceptions;
using Common.Features.Coupon.DTOs;
using Common.Models;
using Microsoft.AspNetCore.Mvc;

namespace BizyPopAPIs.Features.Coupon
{
    public static class GetCouponByIdEndpoint
    {
        public static IEndpointRouteBuilder MapGetCouponById(this IEndpointRouteBuilder app)
        {
            app.MapGet("/{couponId:long}", async (
                long couponId,
                [FromQuery] int? businessId,
                [FromServices] ICouponService couponService) =>
            {
                var coupon = await couponService.GetCouponByIdAsync(couponId, businessId).ConfigureAwait(false);

                if (coupon is null)
                    throw new ResourceNotFoundException($"Coupon with ID '{couponId}' was not found.");

                return Results.Ok(ApiResponse.SuccessResponse(coupon, message: "Coupon retrieved successfully."));
            })
            .RequireAuthorization()
            .Produces<ApiResponseModel<CouponResponseDto>>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithDescription("Endpoint to retrieve a specific coupon by ID for the selected business. `businessId` query is optional and falls back to token context.")
            .WithName("GetCouponById");

            return app;
        }
    }
}

