using Application.Services.IncentiveServices.Interfaces;
using Common.Features.Incentive.Vip.DTOs;
using Common.Models;
using Microsoft.AspNetCore.Mvc;

namespace BizyPopAPIs.Features.Incentive
{
    public static class UpdateVipIncentiveEndpoint
    {
        public static IEndpointRouteBuilder MapUpdateVipIncentive(this IEndpointRouteBuilder app)
        {
            app.MapPut("/{vipId:long}", async (
                long vipId,
                [FromForm] UpdateVipIncentiveDto request,
                [FromQuery] int businessId,
                [FromServices] IVipIncentiveService service) =>
            {
                var result = await service.UpdateAsync(vipId, request, businessId).ConfigureAwait(false);
                return Results.Ok(ApiResponse.SuccessResponse(result, "VIP incentive updated successfully."));
            })
            .DisableAntiforgery()
            .Accepts<UpdateVipIncentiveDto>("multipart/form-data")
            .Produces<ApiResponseModel<VipIncentiveResponseDto>>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithDescription("Endpoint to update a VIP incentive for a selected business.")
            .WithName("UpdateVipIncentive");

            return app;
        }
    }
}
