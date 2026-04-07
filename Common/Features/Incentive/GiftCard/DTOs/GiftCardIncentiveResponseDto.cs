using Domain;

namespace Common.Features.Incentive.GiftCard.DTOs
{
    public record GiftCardIncentiveResponseDto(
        long Id,
        int BusinessId,
        string Title,
        string? PhotoUrl,
        string TrackCode,
        string? QRCodeImageUrl,
        string? MarketingText,
        string? FinePrint,
        DateTime? Expiration,
        string? AdminNote,
        string? CashierPOSMessage,
        IncentiveEntitlementStatus Status,
        DateTime? StatusDate,
        string? StatusNote);
}
