using Application.Services.Interfaces;
using BizyPopAPIs.Utilities.CustomAttribute;
using Common.Features.Business.DTOs;
using Common.Models;
using Microsoft.AspNetCore.Mvc;

namespace BizyPopAPIs.Features.Business
{
    public static class UpdateBusinessEndpoint
    {
        public static IEndpointRouteBuilder MapUpdateBusiness(this IEndpointRouteBuilder app)
        {
            app.MapPut("/{businessId:int}",async (
                int businessId,
                [FromForm] UpdateBusinessDto request,
                [FromServices] IBusinessService businessService) =>
            {
                var result = await businessService.UpdateBusinessAsync(businessId, request).ConfigureAwait(false);
                return Results.Ok(ApiResponse.SuccessResponse(result, message: "Business updated successfully."));
            })
            .RequireAuthorization()
            .AddEndpointFilter<ValidationFilter<UpdateBusinessDto>>()
            .DisableAntiforgery()
            .Accepts<UpdateBusinessDto>("multipart/form-data")
            .Produces<ApiResponseModel<BusinessCardDto>>(StatusCodes.Status200OK)
            .ProducesValidationProblem(StatusCodes.Status422UnprocessableEntity)
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status403Forbidden)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithDescription("Endpoint to update a business profile including optional image replacement.")
            .WithName("UpdateBusiness");

            return app;
        }
    }
}

