using Application.Services.Interfaces;
using Common.Features.Business.DTOs;
using Common.Models;
using Infrastructure.Pagination;
using Microsoft.AspNetCore.Mvc;

namespace BizyPopAPIs.Features.Business
{
    public static class GetBusinessFeedEndpoint
    {
        public static IEndpointRouteBuilder MapGetBusinessFeed(this IEndpointRouteBuilder app)
        {
            app.MapGet("/feed", async (
                [AsParameters] BusinessFeedRequest request,
                [FromServices] IBusinessFeedService businessFeedService,
                HttpContext httpContext) =>
            {
                // ?? Resolve timezone from JWT (optional) ?????????????????????
                // The token is not required. If present and valid, its timezone
                // claim is used as a fallback when no coordinates are supplied.
                string? timeZone = null;
                if (httpContext.User.Identity?.IsAuthenticated == true)
                {
                    var timeZoneClaim = httpContext.User.FindFirst("TimeZone")?.Value;
                    if (!string.IsNullOrWhiteSpace(timeZoneClaim))
                        timeZone = timeZoneClaim;
                }

                var result = await businessFeedService
                    .GetBusinessFeedAsync(request, timeZone)
                    .ConfigureAwait(false);

                return Results.Ok(
                    ApiResponse.SuccessResponse(result, "Business feed retrieved successfully."));
            })
            .AllowAnonymous()
            .Produces<ApiResponseModel<PaginationResponse<BusinessFeedItemDto>>>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithDescription(
                "Returns a paginated feed of nearby businesses with their featured coupon and incentive count. " +
                "Provide lat/lng for a 50-mile radius feed, a JWT for timezone-based feed, or a search term for text search.")
            .WithName("GetBusinessFeed");

            return app;
        }
    }
}

