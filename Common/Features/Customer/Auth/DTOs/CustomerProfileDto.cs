namespace Common.Features.Customer.Auth.DTOs
{
    public record CustomerProfileDto(
        string FirstName,
        string LastName,
        string Email,
        string TimeZone,
        string? ProfilePhotoUrl);
}
