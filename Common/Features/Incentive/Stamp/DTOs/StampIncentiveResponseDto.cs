namespace Common.Features.Incentive.Stamp.DTOs
{
    public record StampIncentiveResponseDto(
        long Id,
        int BusinessId,
        string RewardDesc,
        string? PhotoUrl,
        string TrackCode,
        string? QRCodeImageUrl,
        int StampGoal,
        string? GoalReachedMessage,
        string? FinePrint,
        string? AdminNote,
        string? CashierPOSMessage,
        int? MaxStampPerDay);
}
