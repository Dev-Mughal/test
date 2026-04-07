namespace Common.Features.Business.DTOs
{
    public record BusinessCardDto(
        int Id,
        string Name,
        string? ImageUrl,
        string Address,
        string? AddressLine2,
        string City,
        string State,
        string Country,
        string ZipCode,
        double Latitude,
        double Longitude,
        string Email,
        string Phone,
        string WebsiteUrl,
        string CategoryName,
        int CategoryId,
        DateTime CreatedOn);
}

