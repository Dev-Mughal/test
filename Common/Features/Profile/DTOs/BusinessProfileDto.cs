namespace Common.Features.Profile.DTOs
{
    public record BusinessProfileDto(
        // Business Info
        int BusinessId,
        string BusinessName,
        string BusinessEmail,
        string BusinessPhone,
        string BusinessURL,
        short CountryCode,
        string StreetAddress,
        string? AddressLine2,
        string City,
        string State,
        string ZipCode,
        string Country,
        double Longitude,
        double Latitude,
        string CategoryName
    );
}
