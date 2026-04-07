namespace Infrastructure.Pagination
{
    /// <summary>
    /// Extends <see cref="PaginationRequest"/> with coupon-specific filter parameters.
    /// Passed directly to <c>PaginateAsync</c> — fully compatible because it inherits
    /// <see cref="PaginationRequest.ValidPageNumber"/> and <see cref="PaginationRequest.ValidPageSize"/>.
    /// </summary>
    public record CouponPaginationRequest(
        int PageNumber = 1,
        int PageSize = 10) : PaginationRequest(PageNumber, PageSize);
}
