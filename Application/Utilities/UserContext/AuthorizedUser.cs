using Common.Mappers;
using Common.Models;
using Common.Exceptions;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace Application.Utilities.UserContext
{
    /// <inheritdoc cref="IAuthorizedUser"/>
    public class AuthorizedUser(IHttpContextAccessor httpContextAccessor) : IAuthorizedUser
    {
        private ClaimsPrincipal Claims =>
            httpContextAccessor.HttpContext?.User
            ?? throw new UnauthorizedException("No authenticated HTTP context is available.");

        public long Id =>
            long.TryParse(Claims.FindFirst(ClaimTypes.NameIdentifier)?.Value, out var id) ? id : 0;

        public string Email =>
            Claims.FindFirst(ClaimTypes.Email)?.Value ?? string.Empty;

        public string FirstName =>
            Claims.FindFirst(ClaimTypes.GivenName)?.Value ?? string.Empty;

        public string LastName =>
            Claims.FindFirst(ClaimTypes.Surname)?.Value ?? string.Empty;

        public string TimeZone =>
            Claims.FindFirst("TimeZone")?.Value ?? "UTC";

        public AuthorizedUserDto Current => Claims.ToAuthorizedUserDto();
    }
}
