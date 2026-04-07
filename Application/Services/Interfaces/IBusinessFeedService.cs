using Common.Features.Business.DTOs;
using Infrastructure.Pagination;

namespace Application.Services.Interfaces
{
    /// <summary>
    /// Handles all business feed retrieval logic: search, geo-filter, location-based,
    /// and timezone-based modes. Isolated here so feed behaviour can evolve
    /// (e.g. new incentive types, ranking algorithms) without touching the
    /// general <see cref="IBusinessService"/>.
    /// </summary>
    public interface IBusinessFeedService
    {
        /// <summary>
        /// Returns a paginated feed of businesses with their featured coupon preview
        /// and active-incentive badge count.
        ///
        /// Decision priority:
        ///   1. <c>request.Search</c> provided ? title (business name) search mode.
        ///   2. <c>request.City</c>, <c>request.State</c>, or <c>request.ZipCode</c> provided
        ///      ? geo-filter mode: GeoCity/GeoZipCode tables are queried first to resolve
        ///      matching primary keys, then used to filter businesses.
        ///   3. <c>request.Lat</c> + <c>request.Lng</c> provided ? radius mode;
        ///      radius defaults to 15 miles when <c>request.RadiusMiles</c> is omitted.
        ///   4. <paramref name="timeZone"/> provided (from JWT) ? timezone-relevance mode.
        ///   5. Nothing provided ? empty page returned.
        /// </summary>
        Task<PaginationResponse<BusinessFeedItemDto>> GetBusinessFeedAsync(
            BusinessFeedRequest request,
            string? timeZone);
    }
}
