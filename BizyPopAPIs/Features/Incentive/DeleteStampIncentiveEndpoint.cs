using Application.Services.IncentiveServices.Interfaces;
using Common.Models;
using Microsoft.AspNetCore.Mvc;

namespace BizyPopAPIs.Features.Incentive
{
    public static class DeleteStampIncentiveEndpoint
    {
        public static IEndpointRouteBuilder MapDeleteStampIncentive(this IEndpointRouteBuilder app)
        {
            app.MapDelete("/{stampId:long}", async (
                long stampId,
                [FromQuery] int businessId,
                [FromServices] IStampIncentiveService service) =>
            {
                await service.DeleteAsync(stampId, businessId).ConfigureAwait(false);
                return Results.Ok(ApiResponse.SuccessResponse("Stamp incentive deleted successfully."));
            })
            .Produces<ApiResponseModel<string>>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithDescription("Endpoint to delete a stamp incentive for a selected business.")
            .WithName("DeleteStampIncentive");

            return app;
        }
    }
}
