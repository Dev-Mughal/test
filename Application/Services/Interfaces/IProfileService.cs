using Common.Features.Profile.DTOs;
using Microsoft.AspNetCore.Http;

namespace Application.Services.Interfaces
{
    public interface IProfileService
    {
        Task<UserSummaryDto> GetUserSummaryAsync(HttpRequest request, int? businessId = null);
        Task<BusinessProfileDto> GetBusinessProfileAsync(int? businessId = null);
    }
}
