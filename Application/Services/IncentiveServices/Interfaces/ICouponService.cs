using Common.Features.Coupon.DTOs;
using Common.Features.Customer.Business;
using Infrastructure.Pagination;
using Microsoft.AspNetCore.Http;

namespace Application.Services.IncentiveServices.Interfaces
{
    public interface ICouponService
    {
        Task<IReadOnlyList<IncentiveTypeLookupDto>> GetCouponTypesLookupAsync();
        Task<CouponResponseDto> CreateCouponAsync(CreateCouponDto dto, int? businessId = null);
        Task<CouponResponseDto> UpdateCouponAsync(long couponId, UpdateCouponDto dto, int? businessId = null);
        Task<CouponResponseDto?> GetCouponByIdAsync(long couponId, int? businessId = null);
        Task<PaginationResponse<CouponListItemDto>> GetAllCouponsAsync(CouponPaginationRequest request, int? businessId = null);

        /// <summary>
        /// Customer-facing paginated coupon list for a given business.
        /// Only active, non-expired coupons are returned � featured coupons appear first.
        /// </summary>
        Task<PaginationResponse<CustomerIncentiveItemDto>> GetBusinessIncentivesForCustomerAsync(
            int businessId,
            CouponPaginationRequest request);
    }
}


