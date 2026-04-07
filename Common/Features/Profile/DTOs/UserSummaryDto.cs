namespace Common.Features.Profile.DTOs
{
    public record UserSummaryDto(
        string FirstName,
        string LastName,
        string Email,
        string BusinessName,
        string? BusinessImageUrl
    );
}
