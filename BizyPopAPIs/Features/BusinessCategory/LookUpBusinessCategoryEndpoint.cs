using Application.Services.Interfaces;
using Common.Features.BusinessCategory;
using Common.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BizyPopAPIs.Features.BusinessCategory
{
    public static class LookUpBusinessCategoryEndpoint
    {
        extension(IEndpointRouteBuilder app)
        {
            public IEndpointRouteBuilder MapLookUpBusinessCategory()
            {
                app.MapGet("/lookup", async
                    (
                    [FromServices] IBusinessCategoryService businessCategoryService
                    ) =>
                {
                    var result = await businessCategoryService.GetBusinessCategoriesLookupAsync().ConfigureAwait(false);
                    return Results.Ok(
                        ApiResponse.SuccessResponse(result, message: "Business Categories fetched successfully."));
                })
                .Produces<ApiResponseModel<IEnumerable<SelectListItem>>>(StatusCodes.Status200OK)
                .ProducesProblem(StatusCodes.Status500InternalServerError)
                .WithDescription("Endpoint to get lookup of business categories.")
                .WithName("GetBusinessCategoriesLookup");
                return app;
            }

            public IEndpointRouteBuilder MapGetGroupedBusinessCategories()
            {
                app.MapGet("/grouped", async
                    (
                    [FromServices] IBusinessCategoryService businessCategoryService
                    ) =>
                {
                    var result = await businessCategoryService.GetBusinessCategoriesGroupedAsync().ConfigureAwait(false);
                    return Results.Ok(
                        ApiResponse.SuccessResponse(result, message: "Business categories fetched successfully."));
                })
                .Produces<ApiResponseModel<IEnumerable<BusinessCategoryColumnDto>>>(StatusCodes.Status200OK)
                .ProducesProblem(StatusCodes.Status500InternalServerError)
                .WithDescription("Endpoint to get business categories grouped by display column and sorted by display order.")
                .WithName("GetGroupedBusinessCategories");
                return app;
            }
        }
    }
    public static class BusinessCategoryEndpoints
    {
        extension(IEndpointRouteBuilder app)
        {
            public void MapBusinessCategoryEndpoints()
            {
                var bCategoryGroup = app.MapGroup("/api/business-category").WithTags("BusinessCategories");
                // Map additional authentication endpoints here

                bCategoryGroup.MapLookUpBusinessCategory();
                bCategoryGroup.MapGetGroupedBusinessCategories();

            }
        }
    }
}
