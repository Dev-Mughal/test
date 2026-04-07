namespace Infrastructure.Pagination
{
    /// <summary>
    /// Query parameters accepted by the business feed endpoint.
    /// Inherits PageNumber / PageSize from <see cref="PaginationRequest"/>.
    /// </summary>
    public record BusinessFeedRequest(
        /// <summary>User's current latitude. Required for location-based feed.</summary>
        double? Lat = null,

        /// <summary>User's current longitude. Required for location-based feed.</summary>
        double? Lng = null,

        /// <summary>
        /// Radius in miles for the location-based feed. Defaults to 15 miles when not provided.
        /// Only applied when <see cref="Lat"/> and <see cref="Lng"/> are supplied.
        /// </summary>
        double? RadiusMiles = null,

        /// <summary>
        /// Free-text search term matched against business name / title only.
        /// When provided, location radius and geo filters are ignored.
        /// </summary>
        string? Search = null,

        /// <summary>
        /// Selected geo key returned by the location dropdown lookup endpoint.
        /// Format: <c>C-{GeoCityId}</c> for city/state entries, <c>Z-{GeoZipCodeId}</c> for zip entries.
        /// </summary>
        string? LocationKey = null,

        /// <summary>Optional category filter. Compatible with all feed modes.</summary>
        int? CategoryId = null,

        int PageNumber = 1,
        int PageSize = 10) : PaginationRequest(PageNumber, PageSize);
}

