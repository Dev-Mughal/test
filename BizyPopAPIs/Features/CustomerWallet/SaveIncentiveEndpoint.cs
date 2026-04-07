using Application.Services.Interfaces;
using BizyPopAPIs.Utilities.CustomAttribute;
using Common.Features.Customer.Wallet.DTOs;
using Common.Models;
using Microsoft.AspNetCore.Mvc;

namespace BizyPopAPIs.Features.CustomerWallet
{
    public static class SaveIncentiveEndpoint
    {
        public static IEndpointRouteBuilder MapSaveIncentive(this IEndpointRouteBuilder app)
        {
            app.MapPost("/", async (
                [FromBody] SaveIncentiveDto request,
                [FromServices] ICustomerWalletService walletService) =>
            {
                await walletService.SaveIncentiveAsync(request).ConfigureAwait(false);
                return Results.Created("/api/customer/wallet", ApiResponse.SuccessResponse("Incentive saved to wallet successfully."));
            })
            .RequireAuthorization()
            .AddEndpointFilter<ValidationFilter<SaveIncentiveDto>>()
            .Accepts<SaveIncentiveDto>("application/json")
            .Produces<ApiResponseModel>(StatusCodes.Status201Created)
            .ProducesValidationProblem(StatusCodes.Status422UnprocessableEntity)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithDescription("Saves an incentive (MVP: Coupon only) to the authenticated customer's wallet.")
            .WithName("SaveIncentive");

            return app;
        }
    }
}
