using Application.Services.IncentiveServices.Interfaces;
using Common.Features.Incentive.Stamp.DTOs;
using Common.Models;
using Infrastructure.Pagination;
using Microsoft.AspNetCore.Mvc;

namespace BizyPopAPIs.Features.Incentive
{
    public static class GetAllStampIncentivesEndpoint
    {
        public static IEndpointRouteBuilder MapGetAllStampIncentives(this IEndpointRouteBuilder app)
        {
            app.MapGet("/", async (
                [AsParameters] PaginationRequest request,
                [FromQuery] int businessId,
                [FromServices] IStampIncentiveService service) =>
            {
                var result = await service.GetAllAsync(request, businessId).ConfigureAwait(false);
                return Results.Ok(ApiResponse.SuccessResponse(result, "Stamp incentives retrieved successfully."));
            })
            .Produces<ApiResponseModel<PaginationResponse<StampIncentiveListItemDto>>>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithDescription("Endpoint to retrieve all stamp incentives for a selected business with pagination.")
            .WithName("GetStampIncentives");

            return app;
        }
    }
}
