using Application.Services.Interfaces;
using Common.Features.Business.DTOs;
using Common.Models;
using Microsoft.AspNetCore.Mvc;

namespace BizyPopAPIs.Features.Business
{
    public static class LocationSearchEndpoint
    {
        public static IEndpointRouteBuilder MapSearchLocations(this IEndpointRouteBuilder app)
        {
            app.MapGet("/locations/search", async (
                [FromQuery] string query,
                [FromServices] IGeoService geoService) =>
            {
                var result = await geoService.SearchLocationsAsync(query);
                return Results.Ok(ApiResponse.SuccessResponse(result, "Locations retrieved successfully."));
            })
            .AllowAnonymous()
            .Produces<ApiResponseModel<IReadOnlyList<LocationLookupItemDto>>>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithDescription("Searches geo lookup tables and returns UI-friendly dropdown options.")
            .WithName("SearchLocations");

            return app;
        }
    }
}
