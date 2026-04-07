using Application.Services.IncentiveServices.Interfaces;
using Common.Features.Incentive.GiftCard.DTOs;
using Common.Models;
using Infrastructure.Pagination;
using Microsoft.AspNetCore.Mvc;

namespace BizyPopAPIs.Features.Incentive
{
    public static class GetAllGiftCardIncentivesEndpoint
    {
        public static IEndpointRouteBuilder MapGetAllGiftCardIncentives(this IEndpointRouteBuilder app)
        {
            app.MapGet("/", async (
                [AsParameters] PaginationRequest request,
                [FromQuery] int businessId,
                [FromServices] IGiftCardIncentiveService service) =>
            {
                var result = await service.GetAllAsync(request, businessId).ConfigureAwait(false);
                return Results.Ok(ApiResponse.SuccessResponse(result, "Gift card incentives retrieved successfully."));
            })
            .Produces<ApiResponseModel<PaginationResponse<GiftCardIncentiveListItemDto>>>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithDescription("Endpoint to retrieve all gift card incentives for a selected business with pagination.")
            .WithName("GetGiftCardIncentives");

            return app;
        }
    }
}
