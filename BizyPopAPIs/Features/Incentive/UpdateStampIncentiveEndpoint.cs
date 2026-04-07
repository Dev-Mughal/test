using Application.Services.IncentiveServices.Interfaces;
using Common.Features.Incentive.Stamp.DTOs;
using Common.Models;
using Microsoft.AspNetCore.Mvc;

namespace BizyPopAPIs.Features.Incentive
{
    public static class UpdateStampIncentiveEndpoint
    {
        public static IEndpointRouteBuilder MapUpdateStampIncentive(this IEndpointRouteBuilder app)
        {
            app.MapPut("/{stampId:long}", async (
                long stampId,
                [FromForm] UpdateStampIncentiveDto request,
                [FromQuery] int businessId,
                [FromServices] IStampIncentiveService service) =>
            {
                var result = await service.UpdateAsync(stampId, request, businessId).ConfigureAwait(false);
                return Results.Ok(ApiResponse.SuccessResponse(result, "Stamp incentive updated successfully."));
            })
            .DisableAntiforgery()
            .Accepts<UpdateStampIncentiveDto>("multipart/form-data")
            .Produces<ApiResponseModel<StampIncentiveResponseDto>>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithDescription("Endpoint to update a stamp incentive for a selected business.")
            .WithName("UpdateStampIncentive");

            return app;
        }
    }
}
