namespace Common.Features.Customer.Business
{
    /// <summary>
    /// Static business info displayed on the customer-facing business detail page.
    /// All fields are resolved in one query — no lazy loading.
    /// </summary>
    public record BusinessDetailDto(
        int BusinessId,
        string BusinessName,
        string CategoryName,
        string? BusinessImageUrl,

        // Address shown as a one-liner and individual parts for the map link
        string Address,
        string? AddressLine2,
        string City,
        string State,
        string ZipCode,
        string Country,

        string Phone,
        string Email,
        string WebsiteUrl,

        double Latitude,
        double Longitude,

        // Badge count — number of active, non-expired incentives
        int ActiveIncentiveCount);
}
