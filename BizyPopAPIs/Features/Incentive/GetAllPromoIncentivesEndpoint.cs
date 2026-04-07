using Application.Services.IncentiveServices.Interfaces;
using Common.Features.Incentive.Promo.DTOs;
using Common.Models;
using Infrastructure.Pagination;
using Microsoft.AspNetCore.Mvc;

namespace BizyPopAPIs.Features.Incentive
{
    public static class GetAllPromoIncentivesEndpoint
    {
        public static IEndpointRouteBuilder MapGetAllPromoIncentives(this IEndpointRouteBuilder app)
        {
            app.MapGet("/", async (
                [AsParameters] PaginationRequest request,
                [FromQuery] int businessId,
                [FromServices] IPromoIncentiveService service) =>
            {
                var result = await service.GetAllAsync(request, businessId).ConfigureAwait(false);
                return Results.Ok(ApiResponse.SuccessResponse(result, "Promo incentives retrieved successfully."));
            })
            .Produces<ApiResponseModel<PaginationResponse<PromoIncentiveListItemDto>>>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithDescription("Endpoint to retrieve all promo incentives for a selected business with pagination.")
            .WithName("GetPromoIncentives");

            return app;
        }
    }
}
