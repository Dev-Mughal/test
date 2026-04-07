namespace Common.Features.Incentive.Vip.DTOs
{
    public record VipIncentiveListItemDto(
        long Id,
        string Description,
        string? PhotoUrl,
        string TrackCode,
        string? QRCodeImageUrl,
        DateTime? Expiration);
}
