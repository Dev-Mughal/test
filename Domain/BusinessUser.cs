using Microsoft.AspNetCore.Identity;

namespace Domain
{
    public class BusinessUser : IdentityUser<long>
    {
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public DateTime CreatedOn { get; set; }
        public string TimeZone { get; set; } = null!;

        // Refresh token management
        public string? RefreshToken { get; set; }
        public DateTime? RefreshTokenExpiryTime { get; set; }

        // Navigation Properties
        public virtual ICollection<BusinessUserBusiness> BusinessUserBusinesses { get; set; } = [];
    }
}
