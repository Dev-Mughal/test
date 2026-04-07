using Application.Services.Interfaces;
using Common.Features.Customer.Wallet.DTOs;
using Common.Models;
using Infrastructure.Pagination;
using Microsoft.AspNetCore.Mvc;

namespace BizyPopAPIs.Features.CustomerWallet
{
    public static class GetWalletEndpoint
    {
        public static IEndpointRouteBuilder MapGetWallet(this IEndpointRouteBuilder app)
        {
            app.MapGet("/", async (
                [AsParameters] PaginationRequest request,
                [FromServices] ICustomerWalletService walletService) =>
            {
                var result = await walletService.GetWalletAsync(request).ConfigureAwait(false);
                return Results.Ok(ApiResponse.SuccessResponse(result, "Wallet retrieved successfully."));
            })
            .RequireAuthorization()
            .Produces<ApiResponseModel<PaginationResponse<WalletIncentiveItemDto>>>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithDescription("Returns a paginated list of all incentives saved in the authenticated customer's wallet.")
            .WithName("GetCustomerWallet");

            return app;
        }
    }
}
