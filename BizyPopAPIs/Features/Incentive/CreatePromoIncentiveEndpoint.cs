using Application.Services.IncentiveServices.Interfaces;
using Common.Features.Incentive.Promo.DTOs;
using Common.Models;
using Microsoft.AspNetCore.Mvc;

namespace BizyPopAPIs.Features.Incentive
{
    public static class CreatePromoIncentiveEndpoint
    {
        public static IEndpointRouteBuilder MapCreatePromoIncentive(this IEndpointRouteBuilder app)
        {
            app.MapPost("/", async (
                [FromForm] CreatePromoIncentiveDto request,
                [FromQuery] int businessId,
                [FromServices] IPromoIncentiveService service) =>
            {
                var result = await service.CreateAsync(request, businessId).ConfigureAwait(false);
                return Results.Created(
                    $"/api/incentives/promos/{result.Id}",
                    ApiResponse.SuccessResponse(result, "Promo incentive created successfully."));
            })
            .DisableAntiforgery()
            .Accepts<CreatePromoIncentiveDto>("multipart/form-data")
            .Produces<ApiResponseModel<PromoIncentiveResponseDto>>(StatusCodes.Status201Created)
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithDescription("Endpoint to create a promo incentive for a selected business.")
            .WithName("CreatePromoIncentive");

            return app;
        }
    }
}
