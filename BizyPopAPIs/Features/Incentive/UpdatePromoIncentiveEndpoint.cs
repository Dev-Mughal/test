using Application.Services.IncentiveServices.Interfaces;
using Common.Features.Incentive.Promo.DTOs;
using Common.Models;
using Microsoft.AspNetCore.Mvc;

namespace BizyPopAPIs.Features.Incentive
{
    public static class UpdatePromoIncentiveEndpoint
    {
        public static IEndpointRouteBuilder MapUpdatePromoIncentive(this IEndpointRouteBuilder app)
        {
            app.MapPut("/{promoId:long}", async (
                long promoId,
                [FromForm] UpdatePromoIncentiveDto request,
                [FromQuery] int businessId,
                [FromServices] IPromoIncentiveService service) =>
            {
                var result = await service.UpdateAsync(promoId, request, businessId).ConfigureAwait(false);
                return Results.Ok(ApiResponse.SuccessResponse(result, "Promo incentive updated successfully."));
            })
            .DisableAntiforgery()
            .Accepts<UpdatePromoIncentiveDto>("multipart/form-data")
            .Produces<ApiResponseModel<PromoIncentiveResponseDto>>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithDescription("Endpoint to update a promo incentive for a selected business.")
            .WithName("UpdatePromoIncentive");

            return app;
        }
    }
}
