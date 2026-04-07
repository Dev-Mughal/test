using Application.Services.IncentiveServices.Interfaces;
using Common.Features.Incentive.GiftCard.DTOs;
using Common.Models;
using Microsoft.AspNetCore.Mvc;

namespace BizyPopAPIs.Features.Incentive
{
    public static class UpdateGiftCardIncentiveEndpoint
    {
        public static IEndpointRouteBuilder MapUpdateGiftCardIncentive(this IEndpointRouteBuilder app)
        {
            app.MapPut("/{giftCardId:long}", async (
                long giftCardId,
                [FromForm] UpdateGiftCardIncentiveDto request,
                [FromQuery] int businessId,
                [FromServices] IGiftCardIncentiveService service) =>
            {
                var result = await service.UpdateAsync(giftCardId, request, businessId).ConfigureAwait(false);
                return Results.Ok(ApiResponse.SuccessResponse(result, "Gift card incentive updated successfully."));
            })
            .DisableAntiforgery()
            .Accepts<UpdateGiftCardIncentiveDto>("multipart/form-data")
            .Produces<ApiResponseModel<GiftCardIncentiveResponseDto>>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithDescription("Endpoint to update a gift card incentive for a selected business.")
            .WithName("UpdateGiftCardIncentive");

            return app;
        }
    }
}
