using Application.Services.IncentiveServices.Interfaces;
using Common.Exceptions;
using Common.Features.Incentive.GiftCard.DTOs;
using Common.Models;
using Microsoft.AspNetCore.Mvc;

namespace BizyPopAPIs.Features.Incentive
{
    public static class GetGiftCardIncentiveByIdEndpoint
    {
        public static IEndpointRouteBuilder MapGetGiftCardIncentiveById(this IEndpointRouteBuilder app)
        {
            app.MapGet("/{giftCardId:long}", async (
                long giftCardId,
                [FromQuery] int businessId,
                [FromServices] IGiftCardIncentiveService service) =>
            {
                var result = await service.GetByIdAsync(giftCardId, businessId).ConfigureAwait(false)
                             ?? throw new ResourceNotFoundException($"Gift card incentive with id '{giftCardId}' was not found.");
                return Results.Ok(ApiResponse.SuccessResponse(result, "Gift card incentive retrieved successfully."));
            })
            .Produces<ApiResponseModel<GiftCardIncentiveResponseDto>>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithDescription("Endpoint to retrieve a gift card incentive by id for a selected business.")
            .WithName("GetGiftCardIncentiveById");

            return app;
        }
    }
}
