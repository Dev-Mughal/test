using Application.Services.Interfaces;
using Common.Models;
using Microsoft.AspNetCore.Mvc;

namespace BizyPopAPIs.Features.State
{
    public static class LookupStatesEndpoint
    {
        public static IEndpointRouteBuilder MapLookupStates(this IEndpointRouteBuilder app)
        {
            app.MapGet("api/lookup/states", async (
                [FromServices] IStateService stateService) =>
            {
                var states = await stateService.GetStatesLookupAsync().ConfigureAwait(false);
                return Results.Ok(
                    ApiResponse.SuccessResponse(states, "US states lookup retrieved successfully."));
            })
            .AllowAnonymous()
            .Produces<ApiResponseModel<Dictionary<string, string>>>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithDescription("Returns a lookup of all US states as { code: \"name (code)\", ... }. Used for client dropdowns and validation.")
            .WithName("LookupStates");

            return app;
        }
    }
}
