namespace Common.Features.Incentive.Promo.DTOs
{
    public record PromoIncentiveResponseDto(
        long Id,
        int BusinessId,
        string PromotionDesc,
        string? PhotoUrl,
        string TrackCode,
        string? QRCodeImageUrl,
        DateTime StartDate,
        DateTime ExpirationDate,
        string? FinePrint,
        string? AdminNote,
        string? CashierPOSMessage,
        string? VoidedReason);
}
