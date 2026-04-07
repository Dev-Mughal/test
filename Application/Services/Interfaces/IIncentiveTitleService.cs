using Common.Features.Incentive.DTOs;

namespace Application.Services.Interfaces
{
    /// <summary>
    /// Checks for duplicate incentive titles across different incentive types.
    /// Titles are shared responsibility across all "A" tables — helps prevent duplication.
    /// </summary>
    public interface IIncentiveTitleService
    {
        /// <summary>
        /// Checks if a title already exists for the given incentive type.
        /// Returns a warning (not an error) — allows business users to proceed if they choose.
        /// </summary>
        Task<IncentiveTitleCheckResponse> CheckTitleAsync(string title, int incentiveTypeId, int? businessId = null);
    }
}
