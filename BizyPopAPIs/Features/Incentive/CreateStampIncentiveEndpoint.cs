using Application.Services.IncentiveServices.Interfaces;
using Common.Features.Incentive.Stamp.DTOs;
using Common.Models;
using Microsoft.AspNetCore.Mvc;

namespace BizyPopAPIs.Features.Incentive
{
    public static class CreateStampIncentiveEndpoint
    {
        public static IEndpointRouteBuilder MapCreateStampIncentive(this IEndpointRouteBuilder app)
        {
            app.MapPost("/", async (
                [FromForm] CreateStampIncentiveDto request,
                [FromQuery] int businessId,
                [FromServices] IStampIncentiveService service) =>
            {
                var result = await service.CreateAsync(request, businessId).ConfigureAwait(false);
                return Results.Created(
                    $"/api/incentives/stamps/{result.Id}",
                    ApiResponse.SuccessResponse(result, "Stamp incentive created successfully."));
            })
            .DisableAntiforgery()
            .Accepts<CreateStampIncentiveDto>("multipart/form-data")
            .Produces<ApiResponseModel<StampIncentiveResponseDto>>(StatusCodes.Status201Created)
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithDescription("Endpoint to create a stamp incentive for a selected business.")
            .WithName("CreateStampIncentive");

            return app;
        }
    }
}
