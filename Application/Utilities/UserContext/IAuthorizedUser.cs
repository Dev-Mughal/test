using Common.Models;

namespace Application.Utilities.UserContext
{
    /// <summary>
    /// Provides strongly-typed access to the current authenticated user's claims
    /// without coupling services to <see cref="Microsoft.AspNetCore.Http.IHttpContextAccessor"/>
    /// or raw <see cref="System.Security.Claims.ClaimsPrincipal"/> parsing.
    /// <para>
    /// Register as <c>Scoped</c> so the claims are resolved once per HTTP request.
    /// Use only inside request-scoped services and endpoints — never in singletons or
    /// background jobs where no HTTP context exists.
    /// </para>
    /// </summary>
    public interface IAuthorizedUser
    {
        /// <summary>The authenticated user's primary key (from <c>NameIdentifier</c> claim).</summary>
        long Id { get; }

        /// <summary>The authenticated user's email address.</summary>
        string Email { get; }

        /// <summary>The authenticated user's first name.</summary>
        string FirstName { get; }

        /// <summary>The authenticated user's last name.</summary>
        string LastName { get; }

        /// <summary>
        /// The authenticated user's preferred IANA timezone string (e.g. <c>"America/New_York"</c>).
        /// Defaults to <c>"UTC"</c> if the claim is absent.
        /// </summary>
        string TimeZone { get; }

        /// <summary>
        /// Returns all claims as a fully-populated <see cref="AuthorizedUserDto"/>.
        /// Use this when more than one claim property is needed to avoid multiple individual reads.
        /// </summary>
        AuthorizedUserDto Current { get; }
    }
}
