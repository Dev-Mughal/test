using Application.Services.Interfaces;
using Application.Utilities.UserContext;
using Common.Exceptions;
using Common.Features.Profile.DTOs;
using Infrastructure;
using Infrastructure.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Application.Services.Implementations
{
    public class ProfileService(
        IDbContextFactory<BizyPopDbContext> contextFactory,
        IImageService imageService,
        IAuthorizedUser authorizedUser) : IProfileService
    {
        public async Task<UserSummaryDto> GetUserSummaryAsync(HttpRequest request, int? businessId = null)
        {
            var userId = authorizedUser.Id;
            var resolvedBusinessId = businessId ?? authorizedUser.Current.BusinessId;

            if (resolvedBusinessId <= 0)
                throw new ForbiddenException("Only authenticated business users can access profile data.");

            return await contextFactory.QueryWithDbContextAsync(async context =>
            {
                var userData = await context.Users
                    .AsNoTracking()
                    .Where(u => u.Id == userId)
                    .Select(u => new
                    {
                        u.FirstName,
                        u.LastName,
                        u.Email
                    })
                    .FirstOrDefaultAsync()
                    .ConfigureAwait(false)
                    ?? throw new KeyNotFoundException("User not found.");

                var businessData = await context.BusinessUserBusinesses
                    .AsNoTracking()
                    .Where(link => link.UserId == userId && link.BusinessId == resolvedBusinessId)
                    .Select(link => new
                    {
                        link.Business.BusinessName,
                        link.Business.BusinessImageUrl
                    })
                    .FirstOrDefaultAsync()
                    .ConfigureAwait(false)
                    ?? throw new KeyNotFoundException("Business link not found for user.");

                return new UserSummaryDto(
                    FirstName: userData.FirstName,
                    LastName: userData.LastName,
                    Email: userData.Email!,
                    BusinessName: businessData.BusinessName,
                    BusinessImageUrl: imageService.GetPublicImageUrl(businessData.BusinessImageUrl)
                );
            }).ConfigureAwait(false);
        }

        public async Task<BusinessProfileDto> GetBusinessProfileAsync(int? businessId = null)
        {
            var userId = authorizedUser.Id;
            var resolvedBusinessId = businessId ?? authorizedUser.Current.BusinessId;

            if (resolvedBusinessId <= 0)
                throw new ForbiddenException("Only authenticated business users can access profile data.");

            return await contextFactory.QueryWithDbContextAsync(async context =>
            {
                    return await context.BusinessUserBusinesses
                    .AsNoTracking()
                    .Where(link => link.UserId == userId && link.BusinessId == resolvedBusinessId)
                    .Select(link => new BusinessProfileDto
                    (
                        link.BusinessId,
                        link.Business.BusinessName,
                        link.Business.BusinessEmail,
                        link.Business.BusinessPhone,
                        link.Business.BusinessURL,
                        link.Business.CountryCode,
                        link.Business.StreetAddress,
                        link.Business.AddressLine2,
                        link.Business.GeoCity.City,
                        link.Business.GeoCity.State,
                        link.Business.GeoZipCode.ZipCode,
                        link.Business.Country,
                        link.Business.Longitude,
                        link.Business.Latitude,
                        link.Business.Category.CategoryName
                    ))
                    .FirstOrDefaultAsync()
                    .ConfigureAwait(false)
                    ?? throw new KeyNotFoundException("Business link not found for user.");
            }).ConfigureAwait(false);
        }
    }
}
