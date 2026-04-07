using Common.Features.Incentive.Vip.DTOs;
using Infrastructure.Pagination;

namespace Application.Services.IncentiveServices.Interfaces
{
    public interface IVipIncentiveService
    {
        Task<VipIncentiveResponseDto> CreateAsync(CreateVipIncentiveDto dto, int businessId);
        Task<VipIncentiveResponseDto?> GetByIdAsync(long vipId, int businessId);
        Task<PaginationResponse<VipIncentiveListItemDto>> GetAllAsync(PaginationRequest request, int businessId);
        Task<VipIncentiveResponseDto> UpdateAsync(long vipId, UpdateVipIncentiveDto dto, int businessId);
        Task DeleteAsync(long vipId, int businessId);
    }
}
