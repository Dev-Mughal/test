namespace Common.Features.Customer.Business
{
    /// <summary>
    /// Generic incentive card displayed in the customer-facing incentives grid.
    /// Contains only customer-visible fields shared across incentive types.
    /// </summary>
    public record CustomerIncentiveItemDto(
        long Id,
        int IncentiveTypeId,
        string IncentiveTypeCode,
        string Title,
        string Description,
        string? PhotoUrl,
        string TrackCode,
        DateTime EndDateTime,
        bool IsActive,
        bool IsFeatured);
}
