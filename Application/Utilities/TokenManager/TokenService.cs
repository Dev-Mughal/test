using Common.Models;
using Domain;
using Infrastructure;
using Infrastructure.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
namespace Application.Utilities.TokenManager
{

    public class TokenService : ITokenService
    {
        private readonly JwtSettings _jwtSettings;
        private readonly IDbContextFactory<BizyPopDbContext> _contextFactory;
        private readonly ILogger<TokenService> _logger;

        public TokenService(IOptions<JwtSettings> jwtSettings, IDbContextFactory<BizyPopDbContext> contextFactory, ILogger<TokenService> logger)
        {
            _jwtSettings = jwtSettings.Value;
            _contextFactory = contextFactory;
            _logger = logger;
        }

        public TokenResponse GenerateTokens(AuthorizedUserDto user, bool rememberMe = false)
        {
            var accessToken = GenerateAccessToken(user, rememberMe);
            var refreshToken = GenerateRefreshToken();
            var expiresAt = GetAccessTokenExpiryUtc(rememberMe);

            return new TokenResponse(
                accessToken,
                refreshToken,
                expiresAt
            );
        }
        public TokenResponse GenerateTokens(BusinessUser user, int businessId, bool rememberMe = false)
        {
            return GenerateTokens(new AuthorizedUserDto
            {
                Id = user.Id,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                BusinessId = businessId,
                TimeZone = user.TimeZone
            }, rememberMe);
        }

        private string GenerateAccessToken(AuthorizedUserDto user, bool rememberMe)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.SecretKey));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.GivenName, user.FirstName),
                new Claim(ClaimTypes.Surname, user.LastName),
                new Claim("BusinessId", user.BusinessId.ToString()),
                new Claim("TimeZone", user.TimeZone)
            };

            var token = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                claims: claims,
                expires: GetAccessTokenExpiryUtc(rememberMe),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private string GenerateRefreshToken()
        {
            var randomNumber = new byte[64];
            using var rng = System.Security.Cryptography.RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }

        public async Task SaveRefreshTokenAsync(long userId, string refreshToken, bool rememberMe = false)
        {
            await _contextFactory.WriteWithDbContextAsync(async context =>
            {
                var user = await context.Users.FirstOrDefaultAsync(u => u.Id == userId);
                if (user != null)
                {
                    user.RefreshToken = refreshToken;
                    user.RefreshTokenExpiryTime = GetRefreshTokenExpiryUtc(rememberMe);
                    await context.SaveChangesAsync().ConfigureAwait(false);
                }
            }).ConfigureAwait(false);
        }

        public async Task RevokeRefreshTokenAsync(long userId)
        {
            await _contextFactory.WriteWithDbContextAsync(async context =>
            {
                var user = await context.Users.FirstOrDefaultAsync(u => u.Id == userId);
                if (user != null)
                {
                    user.RefreshToken = null;
                    user.RefreshTokenExpiryTime = null;
                    await context.SaveChangesAsync().ConfigureAwait(false);
                }
            }).ConfigureAwait(false);
        }

        // ── Customer token methods ────────────────────────────────────────────

        public TokenResponse GenerateCustomerTokens(Customer customer, bool rememberMe = false)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.SecretKey));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            // Customer tokens have no BusinessId claim — "UserType" distinguishes
            // them from BusinessUser tokens on the receiving end.
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, customer.CustomerId.ToString()),
                new Claim(ClaimTypes.Email, customer.Email),
                new Claim(ClaimTypes.GivenName, customer.FirstName),
                new Claim(ClaimTypes.Surname, customer.LastName),
                new Claim("TimeZone", customer.TimeZone),
                new Claim("UserType", "Customer")
            };

            var expiresAt = GetAccessTokenExpiryUtc(rememberMe);

            var token = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                claims: claims,
                expires: expiresAt,
                signingCredentials: credentials);

            var refreshToken = GenerateRefreshToken();

            return new TokenResponse(
                new JwtSecurityTokenHandler().WriteToken(token),
                refreshToken,
                expiresAt);
        }

        public async Task SaveCustomerRefreshTokenAsync(long customerId, string refreshToken, bool rememberMe = false)
        {
            await _contextFactory.WriteWithDbContextAsync(async context =>
            {
                var customer = await context.Customers
                    .FirstOrDefaultAsync(c => c.CustomerId == customerId)
                    .ConfigureAwait(false);

                if (customer is not null)
                {
                    customer.RefreshToken = refreshToken;
                    customer.RefreshTokenExpiryTime = GetRefreshTokenExpiryUtc(rememberMe);
                    customer.UpdatedAt = DateTime.UtcNow;
                    await context.SaveChangesAsync().ConfigureAwait(false);
                }
            }).ConfigureAwait(false);
        }

        public bool IsRememberMeSession(DateTime? refreshTokenExpiryTime)
        {
            if (refreshTokenExpiryTime is null)
                return false;

            var shortExpiryWindow = TimeSpan.FromMinutes(GetTokenExpiryMinutes());
            return refreshTokenExpiryTime.Value > DateTime.UtcNow.Add(shortExpiryWindow);
        }

        private DateTime GetAccessTokenExpiryUtc(bool rememberMe)
        {
            return rememberMe
                ? DateTime.UtcNow.AddDays(GetRememberMeTokenExpiryDays())
                : DateTime.UtcNow.AddMinutes(GetTokenExpiryMinutes());
        }

        private DateTime GetRefreshTokenExpiryUtc(bool rememberMe)
        {
            return rememberMe
                ? DateTime.UtcNow.AddDays(GetRememberMeTokenExpiryDays())
                : DateTime.UtcNow.AddMinutes(GetTokenExpiryMinutes());
        }

        private int GetTokenExpiryMinutes() =>
            _jwtSettings.TokenExpiryMinutes > 0
                ? _jwtSettings.TokenExpiryMinutes
                : _jwtSettings.AccessTokenExpirationMinutes;

        private int GetRememberMeTokenExpiryDays() =>
            _jwtSettings.RememberMeTokenExpiryDays > 0
                ? _jwtSettings.RememberMeTokenExpiryDays
                : _jwtSettings.RefreshTokenExpirationDays;

        public async Task RevokeCustomerRefreshTokenAsync(long customerId)
        {
            await _contextFactory.WriteWithDbContextAsync(async context =>
            {
                var customer = await context.Customers
                    .FirstOrDefaultAsync(c => c.CustomerId == customerId)
                    .ConfigureAwait(false);

                if (customer is not null)
                {
                    customer.RefreshToken = null;
                    customer.RefreshTokenExpiryTime = null;
                    customer.UpdatedAt = DateTime.UtcNow;
                    await context.SaveChangesAsync().ConfigureAwait(false);
                }
            }).ConfigureAwait(false);
        }
    }
    public record TokenResponse(string AccessToken, string RefreshToken, DateTime ExpiresAt);
}
