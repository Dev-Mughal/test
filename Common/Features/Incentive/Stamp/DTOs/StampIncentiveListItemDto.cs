namespace Common.Features.Incentive.Stamp.DTOs
{
    public record StampIncentiveListItemDto(
        long Id,
        string RewardDesc,
        string? PhotoUrl,
        string TrackCode,
        string? QRCodeImageUrl,
        int StampGoal);
}
