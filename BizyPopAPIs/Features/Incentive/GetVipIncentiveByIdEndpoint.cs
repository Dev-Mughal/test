using Application.Services.IncentiveServices.Interfaces;
using Common.Exceptions;
using Common.Features.Incentive.Vip.DTOs;
using Common.Models;
using Microsoft.AspNetCore.Mvc;

namespace BizyPopAPIs.Features.Incentive
{
    public static class GetVipIncentiveByIdEndpoint
    {
        public static IEndpointRouteBuilder MapGetVipIncentiveById(this IEndpointRouteBuilder app)
        {
            app.MapGet("/{vipId:long}", async (
                long vipId,
                [FromQuery] int businessId,
                [FromServices] IVipIncentiveService service) =>
            {
                var result = await service.GetByIdAsync(vipId, businessId).ConfigureAwait(false)
                             ?? throw new ResourceNotFoundException($"VIP incentive with id '{vipId}' was not found.");
                return Results.Ok(ApiResponse.SuccessResponse(result, "VIP incentive retrieved successfully."));
            })
            .Produces<ApiResponseModel<VipIncentiveResponseDto>>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithDescription("Endpoint to retrieve a VIP incentive by id for a selected business.")
            .WithName("GetVipIncentiveById");

            return app;
        }
    }
}
