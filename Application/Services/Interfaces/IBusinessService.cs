using Common.Features.Business.DTOs;
using Common.Features.Customer.Business;
using Infrastructure.Pagination;

namespace Application.Services.Interfaces
{
    public interface IBusinessService
    {
        Task<BusinessCardDto?> GetBusinessByIdAsync(int businessId);
        Task<BusinessCardDto> UpdateBusinessAsync(int businessId, UpdateBusinessDto dto);

        /// <summary>
        /// Customer-facing business detail: full contact info + active incentive count.
        /// Returns <see langword="null"/> when no business with <paramref name="businessId"/> exists.
        /// </summary>
        Task<BusinessDetailDto?> GetBusinessDetailAsync(int businessId);
    }
}

