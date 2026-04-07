using Domain;

namespace Common.Features.Incentive.GiftCard.DTOs
{
    public record GiftCardIncentiveListItemDto(
        long Id,
        string Title,
        string? PhotoUrl,
        string TrackCode,
        string? QRCodeImageUrl,
        DateTime? Expiration,
        IncentiveEntitlementStatus Status);
}
