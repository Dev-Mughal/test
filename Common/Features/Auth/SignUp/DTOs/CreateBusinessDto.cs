using Microsoft.AspNetCore.Http;

namespace Common.Features.Auth.SignUp.DTOs
{
    public record CreateBusinessDto
    (
        string BusinessName,
        string BusinessEmail,
        string BusinessPhone,
        string BusinessURL,
        int CountryCode,
        string StreetAddress,
        string? AddressLine2,
        string City,
        string State,
        string ZipCode,
        string Country,
        double Longitude,
        double Latitude,
        string CategoryId,
        IFormFile? BusinessImage
    );
}
