using Application.Services.IncentiveServices.Interfaces;
using Common.Features.Incentive.Vip.DTOs;
using Common.Models;
using Infrastructure.Pagination;
using Microsoft.AspNetCore.Mvc;

namespace BizyPopAPIs.Features.Incentive
{
    public static class GetAllVipIncentivesEndpoint
    {
        public static IEndpointRouteBuilder MapGetAllVipIncentives(this IEndpointRouteBuilder app)
        {
            app.MapGet("/", async (
                [AsParameters] PaginationRequest request,
                [FromQuery] int businessId,
                [FromServices] IVipIncentiveService service) =>
            {
                var result = await service.GetAllAsync(request, businessId).ConfigureAwait(false);
                return Results.Ok(ApiResponse.SuccessResponse(result, "VIP incentives retrieved successfully."));
            })
            .Produces<ApiResponseModel<PaginationResponse<VipIncentiveListItemDto>>>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithDescription("Endpoint to retrieve all VIP incentives for a selected business with pagination.")
            .WithName("GetVipIncentives");

            return app;
        }
    }
}
