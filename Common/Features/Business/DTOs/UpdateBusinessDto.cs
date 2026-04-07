using Microsoft.AspNetCore.Http;

namespace Common.Features.Business.DTOs
{
    public class UpdateBusinessDto
    {
        public string BusinessName { get; set; } = null!;
        public string BusinessEmail { get; set; } = null!;
        public string BusinessPhone { get; set; } = null!;
        public string BusinessURL { get; set; } = null!;
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
        public bool ForceCreate { get; set; }
        public IFormFile? BusinessImage { get; set; }
    }
}

