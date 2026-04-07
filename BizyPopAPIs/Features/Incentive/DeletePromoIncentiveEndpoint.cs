using Application.Services.IncentiveServices.Interfaces;
using Common.Models;
using Microsoft.AspNetCore.Mvc;

namespace BizyPopAPIs.Features.Incentive
{
    public static class DeletePromoIncentiveEndpoint
    {
        public static IEndpointRouteBuilder MapDeletePromoIncentive(this IEndpointRouteBuilder app)
        {
            app.MapDelete("/{promoId:long}", async (
                long promoId,
                [FromQuery] int businessId,
                [FromServices] IPromoIncentiveService service) =>
            {
                await service.DeleteAsync(promoId, businessId).ConfigureAwait(false);
                return Results.Ok(ApiResponse.SuccessResponse("Promo incentive deleted successfully."));
            })
            .Produces<ApiResponseModel<string>>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithDescription("Endpoint to delete a promo incentive for a selected business.")
            .WithName("DeletePromoIncentive");

            return app;
        }
    }
}
