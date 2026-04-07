using Application.Services.IncentiveServices.Interfaces;
using Common.Features.Incentive.Vip.DTOs;
using Common.Models;
using Microsoft.AspNetCore.Mvc;

namespace BizyPopAPIs.Features.Incentive
{
    public static class CreateVipIncentiveEndpoint
    {
        public static IEndpointRouteBuilder MapCreateVipIncentive(this IEndpointRouteBuilder app)
        {
            app.MapPost("/", async (
                [FromForm] CreateVipIncentiveDto request,
                [FromQuery] int businessId,
                [FromServices] IVipIncentiveService service) =>
            {
                var result = await service.CreateAsync(request, businessId).ConfigureAwait(false);
                return Results.Created(
                    $"/api/incentives/vips/{result.Id}",
                    ApiResponse.SuccessResponse(result, "VIP incentive created successfully."));
            })
            .DisableAntiforgery()
            .Accepts<CreateVipIncentiveDto>("multipart/form-data")
            .Produces<ApiResponseModel<VipIncentiveResponseDto>>(StatusCodes.Status201Created)
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithDescription("Endpoint to create a VIP incentive for a selected business.")
            .WithName("CreateVipIncentive");

            return app;
        }
    }
}
