using Application.Services.IncentiveServices.Interfaces;
using Common.Exceptions;
using Common.Features.Incentive.Stamp.DTOs;
using Common.Models;
using Microsoft.AspNetCore.Mvc;

namespace BizyPopAPIs.Features.Incentive
{
    public static class GetStampIncentiveByIdEndpoint
    {
        public static IEndpointRouteBuilder MapGetStampIncentiveById(this IEndpointRouteBuilder app)
        {
            app.MapGet("/{stampId:long}", async (
                long stampId,
                [FromQuery] int businessId,
                [FromServices] IStampIncentiveService service) =>
            {
                var result = await service.GetByIdAsync(stampId, businessId).ConfigureAwait(false)
                             ?? throw new ResourceNotFoundException($"Stamp incentive with id '{stampId}' was not found.");
                return Results.Ok(ApiResponse.SuccessResponse(result, "Stamp incentive retrieved successfully."));
            })
            .Produces<ApiResponseModel<StampIncentiveResponseDto>>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithDescription("Endpoint to retrieve a stamp incentive by id for a selected business.")
            .WithName("GetStampIncentiveById");

            return app;
        }
    }
}
