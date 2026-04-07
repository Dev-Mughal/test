namespace Application.Utilities.UserContext
{
    /// <summary>
    /// Provides strongly-typed access to the current authenticated customer's claims.
    /// Mirrors <see cref="IAuthorizedUser"/> but has no <c>BusinessId</c> — customers
    /// are not tied to any business.
    /// <para>
    /// Register as <c>Scoped</c> — resolved once per HTTP request.
    /// </para>
    /// </summary>
    public interface IAuthorizedCustomer
    {
        /// <summary>The authenticated customer's primary key (from <c>NameIdentifier</c> claim).</summary>
        long Id { get; }

        /// <summary>The authenticated customer's email address.</summary>
        string Email { get; }

        /// <summary>The authenticated customer's first name.</summary>
        string FirstName { get; }

        /// <summary>The authenticated customer's last name.</summary>
        string LastName { get; }

        /// <summary>
        /// The authenticated customer's preferred IANA timezone string (e.g. <c>"America/New_York"</c>).
        /// Defaults to <c>"UTC"</c> if the claim is absent.
        /// </summary>
        string TimeZone { get; }
    }
}
