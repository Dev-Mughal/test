using Application.Services.Interfaces;
using Application.Utilities.UserContext;
using Common.Features.Customer.Auth.DTOs;
using Common.Models;
using Infrastructure;
using Infrastructure.Extensions;
using Microsoft.EntityFrameworkCore;

namespace BizyPopAPIs.Features.CustomerAuth
{
    public static class GetCustomerProfileEndpoint
    {
        public static IEndpointRouteBuilder MapGetCustomerProfile(this IEndpointRouteBuilder app)
        {
            app.MapGet("/profile", async (
                IAuthorizedCustomer authorizedCustomer,
                IDbContextFactory<BizyPopDbContext> contextFactory,
                IImageService imageService,
                IHttpContextAccessor httpContextAccessor) =>
            {
                // Get photo URL from database (not in JWT claims)
                var profilePhotoUrl = await contextFactory.QueryWithDbContextAsync(async context =>
                    await context.Customers
                        .AsNoTracking()
                        .Where(c => c.CustomerId == authorizedCustomer.Id)
                        .Select(c => c.ProfilePhotoUrl)
                        .FirstOrDefaultAsync()
                        .ConfigureAwait(false))
                    .ConfigureAwait(false);

                // Build the full profile DTO
                var profile = new CustomerProfileDto(
                    authorizedCustomer.FirstName,
                    authorizedCustomer.LastName,
                    authorizedCustomer.Email,
                    authorizedCustomer.TimeZone,
                    profilePhotoUrl);

                // Enrich with absolute URL if photo exists and HTTP context is available
                if (!string.IsNullOrWhiteSpace(profilePhotoUrl))
                {
                    var absolutePhotoUrl = imageService.GetPublicImageUrl(profilePhotoUrl);
                    profile = profile with { ProfilePhotoUrl = absolutePhotoUrl };
                }

                return Results.Ok(ApiResponse.SuccessResponse(profile, "Customer profile retrieved successfully."));
            })
            .RequireAuthorization()
            .Produces<ApiResponseModel<CustomerProfileDto>>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithDescription("Retrieve the logged-in customer's profile data including their profile photo URL.")
            .WithName("GetCustomerProfile");

            return app;
        }
    }
}
