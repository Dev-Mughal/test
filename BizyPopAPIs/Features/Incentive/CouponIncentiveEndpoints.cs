using Application.Services.IncentiveServices.Interfaces;
using BizyPopAPIs.Utilities.CustomAttribute;
using Common.Exceptions;
using Common.Features.Coupon.DTOs;
using Common.Models;
using Infrastructure.Pagination;
using Microsoft.AspNetCore.Mvc;

namespace BizyPopAPIs.Features.Incentive
{
    public static class CouponIncentiveEndpoints
    {
        public static RouteGroupBuilder MapCouponEndpoints(this RouteGroupBuilder incentivesGroup)
        {
            var couponGroup = incentivesGroup.MapGroup("/coupons").WithTags("Incentives/Coupons");

            couponGroup.MapGet("/types", async ([FromServices] ICouponService couponService) =>
            {
                var result = await couponService.GetCouponTypesLookupAsync().ConfigureAwait(false);
                return Results.Ok(ApiResponse.SuccessResponse(result, message: "Coupon types retrieved successfully."));
            })
            .Produces<ApiResponseModel<IReadOnlyList<IncentiveTypeLookupDto>>>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithName("IncentiveLookupCouponTypes");

            couponGroup.MapGet("/", async (
                [AsParameters] CouponPaginationRequest request,
                [FromQuery] int? businessId,
                [FromServices] ICouponService couponService) =>
            {
                var result = await couponService.GetAllCouponsAsync(request, businessId).ConfigureAwait(false);
                return Results.Ok(ApiResponse.SuccessResponse(result, message: "Coupons retrieved successfully."));
            })
            .Produces<ApiResponseModel<PaginationResponse<CouponListItemDto>>>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithName("IncentiveGetAllCoupons");

            couponGroup.MapPost("/", async (
                [FromForm] CreateCouponDto request,
                [FromQuery] int? businessId,
                [FromServices] ICouponService couponService) =>
            {
                var result = await couponService.CreateCouponAsync(request, businessId).ConfigureAwait(false);
                return Results.Created<ApiResponseModel<CouponResponseDto>>(
                    $"/api/incentives/coupons/{result.Id}",
                    ApiResponse.SuccessResponse(result, message: "Coupon created successfully."));
            })
            .AddEndpointFilter<ValidationFilter<CreateCouponDto>>()
            .DisableAntiforgery()
            .Accepts<CreateCouponDto>("multipart/form-data")
            .Produces<ApiResponseModel<CouponResponseDto>>(StatusCodes.Status201Created)
            .ProducesValidationProblem(StatusCodes.Status422UnprocessableEntity)
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithName("IncentiveCreateCoupon");

            couponGroup.MapPut("/{couponId:long}", async (
                long couponId,
                [FromForm] UpdateCouponDto request,
                [FromQuery] int? businessId,
                [FromServices] ICouponService couponService) =>
            {
                var result = await couponService.UpdateCouponAsync(couponId, request, businessId).ConfigureAwait(false);
                return Results.Ok(ApiResponse.SuccessResponse(result, message: "Coupon updated successfully."));
            })
            .AddEndpointFilter<ValidationFilter<UpdateCouponDto>>()
            .DisableAntiforgery()
            .Accepts<UpdateCouponDto>("multipart/form-data")
            .Produces<ApiResponseModel<CouponResponseDto>>(StatusCodes.Status200OK)
            .ProducesValidationProblem(StatusCodes.Status422UnprocessableEntity)
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithName("IncentiveUpdateCoupon");

            couponGroup.MapGet("/{couponId:long}", async (
                long couponId,
                [FromQuery] int? businessId,
                [FromServices] ICouponService couponService) =>
            {
                var coupon = await couponService.GetCouponByIdAsync(couponId, businessId).ConfigureAwait(false);

                if (coupon is null)
                    throw new ResourceNotFoundException($"Coupon with ID '{couponId}' was not found.");

                return Results.Ok(ApiResponse.SuccessResponse(coupon, message: "Coupon retrieved successfully."));
            })
            .Produces<ApiResponseModel<CouponResponseDto>>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithName("IncentiveGetCouponById");

            return incentivesGroup;
        }
    }
}


