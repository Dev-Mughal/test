using Common.Features.Incentive.Promo.DTOs;
using Infrastructure.Pagination;

namespace Application.Services.IncentiveServices.Interfaces
{
    public interface IPromoIncentiveService
    {
        Task<PromoIncentiveResponseDto> CreateAsync(CreatePromoIncentiveDto dto, int businessId);
        Task<PromoIncentiveResponseDto?> GetByIdAsync(long promoId, int businessId);
        Task<PaginationResponse<PromoIncentiveListItemDto>> GetAllAsync(PaginationRequest request, int businessId);
        Task<PromoIncentiveResponseDto> UpdateAsync(long promoId, UpdatePromoIncentiveDto dto, int businessId);
        Task DeleteAsync(long promoId, int businessId);
    }
}
