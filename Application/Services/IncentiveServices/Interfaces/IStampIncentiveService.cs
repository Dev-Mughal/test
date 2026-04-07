using Common.Features.Incentive.Stamp.DTOs;
using Infrastructure.Pagination;

namespace Application.Services.IncentiveServices.Interfaces
{
    public interface IStampIncentiveService
    {
        Task<StampIncentiveResponseDto> CreateAsync(CreateStampIncentiveDto dto, int businessId);
        Task<StampIncentiveResponseDto?> GetByIdAsync(long stampId, int businessId);
        Task<PaginationResponse<StampIncentiveListItemDto>> GetAllAsync(PaginationRequest request, int businessId);
        Task<StampIncentiveResponseDto> UpdateAsync(long stampId, UpdateStampIncentiveDto dto, int businessId);
        Task DeleteAsync(long stampId, int businessId);
    }
}
