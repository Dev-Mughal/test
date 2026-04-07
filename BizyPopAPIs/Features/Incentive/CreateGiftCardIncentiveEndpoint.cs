using Application.Services.IncentiveServices.Interfaces;
using Common.Features.Incentive.GiftCard.DTOs;
using Common.Models;
using Microsoft.AspNetCore.Mvc;

namespace BizyPopAPIs.Features.Incentive
{
    public static class CreateGiftCardIncentiveEndpoint
    {
        public static IEndpointRouteBuilder MapCreateGiftCardIncentive(this IEndpointRouteBuilder app)
        {
            app.MapPost("/", async (
                [FromForm] CreateGiftCardIncentiveDto request,
                [FromQuery] int businessId,
                [FromServices] IGiftCardIncentiveService service) =>
            {
                var result = await service.CreateAsync(request, businessId).ConfigureAwait(false);
                return Results.Created(
                    $"/api/incentives/gift-cards/{result.Id}",
                    ApiResponse.SuccessResponse(result, "Gift card incentive created successfully."));
            })
            .DisableAntiforgery()
            .Accepts<CreateGiftCardIncentiveDto>("multipart/form-data")
            .Produces<ApiResponseModel<GiftCardIncentiveResponseDto>>(StatusCodes.Status201Created)
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithDescription("Endpoint to create a gift card incentive for a selected business.")
            .WithName("CreateGiftCardIncentive");

            return app;
        }
    }
}
