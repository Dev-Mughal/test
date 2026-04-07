using Microsoft.AspNetCore.Http;

namespace Common.Features.Customer.Auth.DTOs
{
    public class CustomerSignUpDto
    {
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string TimeZone { get; set; } = null!;
        public IFormFile? ProfilePhoto { get; set; }
    }
}
