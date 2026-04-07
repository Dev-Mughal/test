using Application.Services.Interfaces;
using Common.Models;
using Microsoft.AspNetCore.Mvc;

namespace BizyPopAPIs.Features.Profile
{
    public static class GetBusinessProfileEndpoint
    {
        extension(IEndpointRouteBuilder app)
        {
            public IEndpointRouteBuilder MapGetBusinessProfile()
            {
                app.MapGet("/profile", async (
                    [FromQuery] int? businessId,
                    [FromServices] IProfileService profileService) =>
                {
                    var result = await profileService.GetBusinessProfileAsync(businessId).ConfigureAwait(false);

                    return Results.Ok(ApiResponse.SuccessResponse(result, "Business profile fetched successfully."));
                })
                .RequireAuthorization()
                .Produces<ApiResponseModel<Common.Features.Profile.DTOs.BusinessProfileDto>>(StatusCodes.Status200OK)
                .ProducesProblem(StatusCodes.Status401Unauthorized)
                .ProducesProblem(StatusCodes.Status404NotFound)
                .ProducesProblem(StatusCodes.Status500InternalServerError)
                .WithDescription("Returns the full business and user profile for the logged-in user. `businessId` query is optional and falls back to token context.")
                .WithName("GetBusinessProfile");

                return app;
            }
        }
    }
}
