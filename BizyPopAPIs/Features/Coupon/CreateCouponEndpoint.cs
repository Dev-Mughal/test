using Application.Services.IncentiveServices.Interfaces;
using BizyPopAPIs.Utilities.CustomAttribute;
using Common.Features.Coupon.DTOs;
using Common.Models;
using Microsoft.AspNetCore.Mvc;

namespace BizyPopAPIs.Features.Coupon
{
    public static class CreateCouponEndpoint
    {
        public static IEndpointRouteBuilder MapCreateCoupon(this IEndpointRouteBuilder app)
        {
            app.MapPost("/", async (
                [FromForm] CreateCouponDto request,
                [FromQuery] int? businessId,
                [FromServices] ICouponService couponService) =>
            {
                CouponResponseDto result = await couponService.CreateCouponAsync(request, businessId).ConfigureAwait(false);
                return Results.Created<ApiResponseModel<CouponResponseDto>>(
                    $"/api/coupons/{result.Id}",
                    ApiResponse.SuccessResponse(result, message: "Coupon created successfully."));
            })
            .RequireAuthorization()
            .AddEndpointFilter<ValidationFilter<CreateCouponDto>>()
            .DisableAntiforgery()
            .Accepts<CreateCouponDto>("multipart/form-data")
            .Produces<ApiResponseModel<CouponResponseDto>>(StatusCodes.Status201Created)
            .ProducesValidationProblem(StatusCodes.Status422UnprocessableEntity)
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithDescription("Endpoint to create a new coupon for the selected business with optional image upload. `businessId` query is optional and falls back to token context.")
            .WithName("CreateCoupon");

            return app;
        }
    }
}
