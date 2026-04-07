using Application.Services.IncentiveServices.Interfaces;
using Common.Models;
using Microsoft.AspNetCore.Mvc;

namespace BizyPopAPIs.Features.Incentive
{
    public static class DeleteVipIncentiveEndpoint
    {
        public static IEndpointRouteBuilder MapDeleteVipIncentive(this IEndpointRouteBuilder app)
        {
            app.MapDelete("/{vipId:long}", async (
                long vipId,
                [FromQuery] int businessId,
                [FromServices] IVipIncentiveService service) =>
            {
                await service.DeleteAsync(vipId, businessId).ConfigureAwait(false);
                return Results.Ok(ApiResponse.SuccessResponse("VIP incentive deleted successfully."));
            })
            .Produces<ApiResponseModel<string>>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithDescription("Endpoint to delete a VIP incentive for a selected business.")
            .WithName("DeleteVipIncentive");

            return app;
        }
    }
}
