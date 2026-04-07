using Application.Services.Interfaces;
using Common.Features.Incentive.DTOs;
using Common.Models;
using Microsoft.AspNetCore.Mvc;

namespace BizyPopAPIs.Features.Incentive
{
    public static class CheckTitleEndpoint
    {
        public static IEndpointRouteBuilder MapIncentiveEndpoints(this IEndpointRouteBuilder app)
        {
            app.MapIncentiveCrudEndpoints();

            app.MapGet("/api/incentives/check-title", async (
                [FromQuery] string title,
                [FromQuery] int incentiveTypeId,
                [FromQuery] int? businessId,
                [FromServices] IIncentiveTitleService titleService) =>
            {
                if (string.IsNullOrWhiteSpace(title))
                    return Results.BadRequest(ApiResponse.ErrorResponse("Title cannot be empty."));

                var result = await titleService.CheckTitleAsync(title, incentiveTypeId, businessId).ConfigureAwait(false);
                return Results.Ok(ApiResponse.SuccessResponse(result, "Title check completed."));
            })
            .RequireAuthorization()
            .Produces<ApiResponseModel<IncentiveTitleCheckResponse>>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithDescription(
                "Checks if an incentive title already exists for the given incentive type. " +
                "Returns a warning if a duplicate is found (not blocking), allowing the user to proceed if desired. `businessId` query is optional and falls back to token context.")
            .WithName("CheckIncentiveTitle");

            return app;
        }
    }
}
