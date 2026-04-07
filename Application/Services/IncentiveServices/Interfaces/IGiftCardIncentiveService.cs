using Common.Features.Incentive.GiftCard.DTOs;
using Infrastructure.Pagination;

namespace Application.Services.IncentiveServices.Interfaces
{
    public interface IGiftCardIncentiveService
    {
        Task<GiftCardIncentiveResponseDto> CreateAsync(CreateGiftCardIncentiveDto dto, int businessId);
        Task<GiftCardIncentiveResponseDto?> GetByIdAsync(long giftCardId, int businessId);
        Task<PaginationResponse<GiftCardIncentiveListItemDto>> GetAllAsync(PaginationRequest request, int businessId);
        Task<GiftCardIncentiveResponseDto> UpdateAsync(long giftCardId, UpdateGiftCardIncentiveDto dto, int businessId);
        Task DeleteAsync(long giftCardId, int businessId);
    }
}
