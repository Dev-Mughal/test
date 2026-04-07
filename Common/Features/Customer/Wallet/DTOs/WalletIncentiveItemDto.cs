using Domain;

namespace Common.Features.Customer.Wallet.DTOs
{
    /// <summary>
    /// A single saved incentive entry in the customer's wallet.
    /// MVP: only Coupon type is supported — additional types (Stamp, GiftCard, etc.) will extend this shape.
    /// </summary>
    public record WalletIncentiveItemDto(
        long IncentiveId,
        string BusinessName,
        string Title,
        string Description,
        string? PhotoUrl,
        string TrackCode,
        DateTime EndDateTime,
        IncentiveEntitlementStatus Status,
        DateTime? DateRedeemed,
        DateTime Created
    );
}
