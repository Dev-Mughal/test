using Application.Services.IncentiveServices.Interfaces;
using BizyPopAPIs.Utilities.CustomAttribute;
using Common.Features.Coupon.DTOs;
using Common.Models;
using Microsoft.AspNetCore.Mvc;

namespace BizyPopAPIs.Features.Coupon
{
    public static class UpdateCouponEndpoint
    {
        public static IEndpointRouteBuilder MapUpdateCoupon(this IEndpointRouteBuilder app)
        {
            app.MapPut("/{couponId:long}", async (
                long couponId,
                [FromForm] UpdateCouponDto request,
                [FromQuery] int? businessId,
                [FromServices] ICouponService couponService) =>
            {
                var result = await couponService.UpdateCouponAsync(couponId, request, businessId).ConfigureAwait(false);
                return Results.Ok(ApiResponse.SuccessResponse(result, message: "Coupon updated successfully."));
            })
            .RequireAuthorization()
            .AddEndpointFilter<ValidationFilter<UpdateCouponDto>>()
            .DisableAntiforgery()
            .Accepts<UpdateCouponDto>("multipart/form-data")
            .Produces<ApiResponseModel<CouponResponseDto>>(StatusCodes.Status200OK)
            .ProducesValidationProblem(StatusCodes.Status422UnprocessableEntity)
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithDescription("Endpoint to update an existing coupon for the selected business with optional image replacement. `businessId` query is optional and falls back to token context.")
            .WithName("UpdateCoupon");

            return app;
        }
    }
}

