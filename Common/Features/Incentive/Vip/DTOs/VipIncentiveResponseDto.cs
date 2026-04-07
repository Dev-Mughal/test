namespace Common.Features.Incentive.Vip.DTOs
{
    public record VipIncentiveResponseDto(
        long Id,
        int BusinessId,
        string Description,
        string? PhotoUrl,
        string TrackCode,
        string? QRCodeImageUrl,
        string? DesignData,
        string? FinePrint,
        int? DefaultStartDay,
        int? DefaultEndDay,
        int? DefaultDailyStartHour,
        int? DefaultDailyEndHour,
        DateTime? Expiration,
        string? AdminNote,
        string? CashierPOSMessage);
}
