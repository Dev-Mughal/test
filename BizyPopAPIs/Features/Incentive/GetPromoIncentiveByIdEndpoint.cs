using Application.Services.IncentiveServices.Interfaces;
using Common.Exceptions;
using Common.Features.Incentive.Promo.DTOs;
using Common.Models;
using Microsoft.AspNetCore.Mvc;

namespace BizyPopAPIs.Features.Incentive
{
    public static class GetPromoIncentiveByIdEndpoint
    {
        public static IEndpointRouteBuilder MapGetPromoIncentiveById(this IEndpointRouteBuilder app)
        {
            app.MapGet("/{promoId:long}", async (
                long promoId,
                [FromQuery] int businessId,
                [FromServices] IPromoIncentiveService service) =>
            {
                var result = await service.GetByIdAsync(promoId, businessId).ConfigureAwait(false)
                             ?? throw new ResourceNotFoundException($"Promo incentive with id '{promoId}' was not found.");
                return Results.Ok(ApiResponse.SuccessResponse(result, "Promo incentive retrieved successfully."));
            })
            .Produces<ApiResponseModel<PromoIncentiveResponseDto>>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithDescription("Endpoint to retrieve a promo incentive by id for a selected business.")
            .WithName("GetPromoIncentiveById");

            return app;
        }
    }
}
