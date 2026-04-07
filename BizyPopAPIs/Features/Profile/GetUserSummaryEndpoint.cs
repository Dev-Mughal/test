using Application.Services.Interfaces;
using Common.Models;
using Microsoft.AspNetCore.Mvc;

namespace BizyPopAPIs.Features.Profile
{
    public static class GetUserSummaryEndpoint
    {
        extension(IEndpointRouteBuilder app)
        {
            public IEndpointRouteBuilder MapGetUserSummary()
            {
                app.MapGet("/me", async (
                    HttpRequest httpRequest,
                    [FromQuery] int? businessId,
                    [FromServices] IProfileService profileService) =>
                {
                    var result = await profileService.GetUserSummaryAsync(httpRequest, businessId).ConfigureAwait(false);

                    return Results.Ok(ApiResponse.SuccessResponse(result, "User summary fetched successfully."));
                })
                .RequireAuthorization()
                .Produces<ApiResponseModel<Common.Features.Profile.DTOs.UserSummaryDto>>(StatusCodes.Status200OK)
                .ProducesProblem(StatusCodes.Status401Unauthorized)
                .ProducesProblem(StatusCodes.Status404NotFound)
                .ProducesProblem(StatusCodes.Status500InternalServerError)
                .WithDescription("Returns the logged-in user's name and selected business image. `businessId` query is optional and falls back to token context.")
                .WithName("GetUserSummary");

                return app;
            }
        }
    }
}
