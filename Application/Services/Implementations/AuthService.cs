using Application.Constants;
using Application.Utilities.TokenManager;
using Application.Utilities.UserContext;
using Common.Exceptions;
using Common.Features.Auth.Login;
using Common.Features.Auth.RefreshToken;
using Common.Features.Auth.SignUp.DTOs;
using Common.Mappers;
using Common.Models;
using Domain;
using Infrastructure;
using Infrastructure.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Text;
using Common.Models;
using Application.Services.Interfaces;

namespace Application.Services.Implementations
{
    public class AuthService(
        IDbContextFactory<BizyPopDbContext> contextFactory
        , UserManager<BusinessUser> userManager
        , ITokenService tokenService
        , IImageService imageService
        , IGeoService geoService
        , IAuthorizedUser authorizedUser
        ) : IAuthService
    {
        public async Task<TokenResponse> SignUpAsync(SignUpDto dto, IFormFile? businessImage)
        {
            // Handle business image upload
            string? businessImageUrl = null;
            if (businessImage != null && businessImage.Length > 0)
                businessImageUrl = await imageService.SaveImageAsync(businessImage, ImageTypeEnum.Business).ConfigureAwait(false);

            // Resolve L50 (City/State) and L51 (City/State/Zip) primary keys.
            // Throws GeoLocationNotFoundException (422) when the combination is unknown
            // and ForceCreate is false so the client can prompt "Continue Anyways?".
            var (stateCityId, stateCityZipId) = await geoService
                .ResolveGeoIdsAsync(dto.City, dto.State, dto.ZipCode, dto.ForceCreate)
                .ConfigureAwait(false);

            // STEP 1: Save Business first to get the auto-generated long PK
            var business = dto.ToBusiness(businessImageUrl, stateCityId, stateCityZipId);

            await contextFactory.WriteWithDbContextAsync(async context =>
            {
                await context.Businesses.AddAsync(business).ConfigureAwait(false);
                await context.SaveChangesAsync().ConfigureAwait(false);
            }).ConfigureAwait(false);

            // STEP 2: Create BusinessUser account
            var businessUser = dto.ToBusinessUser();
            var createUserResult = await userManager.CreateAsync(businessUser, dto.Password).ConfigureAwait(false);

            if (!createUserResult.Succeeded)
            {
                // Clean up: remove saved business and image on user creation failure
                await contextFactory.WriteWithDbContextAsync(async context =>
                {
                    var saved = await context.Businesses.FindAsync(business.BusinessId).ConfigureAwait(false);
                    if (saved is not null)
                    {
                        context.Businesses.Remove(saved);
                        await context.SaveChangesAsync().ConfigureAwait(false);
                    }
                }).ConfigureAwait(false);

                if (!string.IsNullOrEmpty(businessImageUrl))
                    await imageService.DeleteImageAsync(businessImageUrl).ConfigureAwait(false);

                var errors = new StringBuilder();
                foreach (var error in createUserResult.Errors)
                    errors.AppendLine(error.Description);

                throw new Exception($"User creation failed: {errors}");
            }

            // STEP 3: Link user and business through B03 join table (default on signup)
            await contextFactory.WriteWithDbContextAsync(async context =>
            {
                await context.BusinessUserBusinesses.AddAsync(new BusinessUserBusiness
                {
                    BusinessId = business.BusinessId,
                    UserId = businessUser.Id,
                    IsDefault = true
                }).ConfigureAwait(false);

                await context.SaveChangesAsync().ConfigureAwait(false);
            }).ConfigureAwait(false);

            var tokens = tokenService.GenerateTokens(businessUser, business.BusinessId);
            await tokenService.SaveRefreshTokenAsync(businessUser.Id, tokens.RefreshToken).ConfigureAwait(false);

            return tokens;
        }
        public async Task<EmailVerificationResultDto> IsEmailAlreadyExists(EmailVerificationDto dto)
        {
            var exists = await contextFactory.QueryWithDbContextAsync(async context =>
                await context.Users
                    .AnyAsync(u => u.Email.Equals(dto.Email))
                    .ConfigureAwait(false)
            ).ConfigureAwait(false);
            return new EmailVerificationResultDto(exists);
        }
        public async Task<TokenResponse> LoginAsync(LoginDto request)
        {
            var user = await userManager.FindByEmailAsync(request.Email).ConfigureAwait(false);
            if (user == null || !await userManager.CheckPasswordAsync(user, request.Password).ConfigureAwait(false))
                throw new UnauthorizedAccessException("Invalid credentials");

            var businessId = await GetDefaultBusinessIdAsync(user.Id).ConfigureAwait(false);
            var tokens = tokenService.GenerateTokens(user, businessId, request.RememberMe);
            await tokenService.SaveRefreshTokenAsync(user.Id, tokens.RefreshToken, request.RememberMe).ConfigureAwait(false);

            return tokens;
        }

        public async Task<TokenResponse> RefreshTokenAsync(RefreshTokenDto request)
        {
            var user = await contextFactory.WriteWithDbContextAsync(async context =>
                await context.Users
                    .FirstOrDefaultAsync(u =>
                        u.RefreshToken == request.RefreshToken &&
                        u.RefreshTokenExpiryTime > DateTime.UtcNow)
                    .ConfigureAwait(false)
            ).ConfigureAwait(false) ?? throw new UnauthorizedException("Invalid or expired refresh token.");

            var rememberMe = tokenService.IsRememberMeSession(user.RefreshTokenExpiryTime);
            var businessId = await GetDefaultBusinessIdAsync(user.Id).ConfigureAwait(false);
            var tokens = tokenService.GenerateTokens(user, businessId, rememberMe);
            await tokenService.SaveRefreshTokenAsync(user.Id, tokens.RefreshToken, rememberMe).ConfigureAwait(false);

            return tokens;
        }
        public async Task LogoutAsync()
        {
            await tokenService.RevokeRefreshTokenAsync(authorizedUser.Id).ConfigureAwait(false);
        }

        private async Task<int> GetDefaultBusinessIdAsync(long userId)
        {
            var businessId = await contextFactory.QueryWithDbContextAsync(async context =>
                await context.BusinessUserBusinesses
                    .AsNoTracking()
                    .Where(x => x.UserId == userId)
                    .OrderByDescending(x => x.IsDefault == true)
                    .ThenBy(x => x.Id)
                    .Select(x => x.BusinessId)
                    .FirstOrDefaultAsync()
                    .ConfigureAwait(false)).ConfigureAwait(false);

            if (businessId <= 0)
                throw new UnauthorizedException("No business link found for this user.");

            return businessId;
        }
    }
}
