namespace Common.Features.Incentive.Promo.DTOs
{
    public record PromoIncentiveListItemDto(
        long Id,
        string PromotionDesc,
        string? PhotoUrl,
        string TrackCode,
        string? QRCodeImageUrl,
        DateTime StartDate,
        DateTime ExpirationDate);
}
