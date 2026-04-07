using Microsoft.AspNetCore.Http;

namespace Common.Features.Customer.Auth.DTOs
{
    public class CustomerUpdateProfileDto
    {
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public IFormFile? ProfilePhoto { get; set; }
    }
}
