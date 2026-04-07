namespace Common.Features.Business.DTOs
{
    /// <summary>
    /// Represents a single business card shown in the customer feed.
    /// Designed to match the UI card: business info + one featured coupon preview + incentive badge.
    /// </summary>
    public record BusinessFeedItemDto(
        int BusinessId,
        string BusinessName,
        string BusinessTypeName,
        string BusinessAddress,
        string? AddressLine2,
        string? FeaturedCouponTitle,
        string? FeaturedCouponPhotoUrl,
        int ActiveIncentiveCount,
        double Latitude,
        double Longitude,
        string? BusinessImageUrl,
        double? DistanceMiles);
}
