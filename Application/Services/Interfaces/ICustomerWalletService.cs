using Common.Features.Customer.Wallet.DTOs;
using Infrastructure.Pagination;

namespace Application.Services.Interfaces
{
    public interface ICustomerWalletService
    {
        /// <summary>
        /// Saves an incentive to the authenticated customer's wallet.
        /// MVP: only Coupon type (TypeCode = "C") is supported.
        /// </summary>
        Task SaveIncentiveAsync(SaveIncentiveDto dto);

        /// <summary>
        /// Returns a paginated list of all incentives saved in the customer's wallet.
        /// </summary>
        Task<PaginationResponse<WalletIncentiveItemDto>> GetWalletAsync(PaginationRequest request);
    }
}
