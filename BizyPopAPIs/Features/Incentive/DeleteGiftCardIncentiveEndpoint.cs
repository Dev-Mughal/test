using Application.Services.IncentiveServices.Interfaces;
using Common.Models;
using Microsoft.AspNetCore.Mvc;

namespace BizyPopAPIs.Features.Incentive
{
    public static class DeleteGiftCardIncentiveEndpoint
    {
        public static IEndpointRouteBuilder MapDeleteGiftCardIncentive(this IEndpointRouteBuilder app)
        {
            app.MapDelete("/{giftCardId:long}", async (
                long giftCardId,
                [FromQuery] int businessId,
                [FromServices] IGiftCardIncentiveService service) =>
            {
                await service.DeleteAsync(giftCardId, businessId).ConfigureAwait(false);
                return Results.Ok(ApiResponse.SuccessResponse("Gift card incentive deleted successfully."));
            })
            .Produces<ApiResponseModel<string>>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithDescription("Endpoint to delete a gift card incentive for a selected business.")
            .WithName("DeleteGiftCardIncentive");

            return app;
        }
    }
}
