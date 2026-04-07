using Application.Services.IncentiveServices.Interfaces;
using Common.Features.Coupon.DTOs;
using Common.Models;
using Microsoft.AspNetCore.Mvc;

namespace BizyPopAPIs.Features.Coupon
{
    public static class LookupCouponTypeEndpoint
    {
        public static IEndpointRouteBuilder MapLookupCouponTypes(this IEndpointRouteBuilder app)
        {
            app.MapGet("/types/lookup", async (
                [FromServices] ICouponService couponService) =>
            {
                var result = await couponService.GetCouponTypesLookupAsync().ConfigureAwait(false);
                return Results.Ok(ApiResponse.SuccessResponse(result, message: "Coupon types fetched successfully."));
            })
            .AllowAnonymous()
            .Produces<ApiResponseModel<IReadOnlyList<IncentiveTypeLookupDto>>>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithDescription("Returns all active incentive types for use in dropdowns and selection lists. Does not require authentication.")
            .WithName("GetIncentiveTypesLookup");

            return app;
        }
    }
}
