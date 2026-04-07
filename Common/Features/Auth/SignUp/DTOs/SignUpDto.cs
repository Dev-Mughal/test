using Microsoft.AspNetCore.Http;

namespace Common.Features.Auth.SignUp.DTOs
{
    public class SignUpDto
    {
        // Business properties
        public string BusinessName { get; set; } = null!;
        public string? BusinessEmail { get; set; }
        public string BusinessPhone { get; set; } = null!;
        public string? BusinessURL { get; set; }
        public short CountryCode { get; set; }
        public string StreetAddress { get; set; } = null!;
        public string? AddressLine2 { get; set; }
        public string City { get; set; } = null!;
        public string State { get; set; } = null!;
        public string ZipCode { get; set; } = null!;
        public string Country { get; set; } = null!;
        public double Longitude { get; set; }
        public double Latitude { get; set; }
        public int CategoryId { get; set; }
        public IFormFile? BusinessImage { get; set; }

        /// <summary>
        /// When true, an unrecognised City/State/ZipCode combination is inserted
        /// into L50/L51 with the UserInput flag set, and sign-up proceeds.
        /// The client should only send true after the user confirms the
        /// "Continue Anyways?" prompt returned by a prior 422 response.
        /// </summary>
        public bool ForceCreate { get; set; }

        // User properties
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string TimeZone { get; set; } = null!;
    }
}

