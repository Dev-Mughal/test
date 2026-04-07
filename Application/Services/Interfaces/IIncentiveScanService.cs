using Common.Features.Scan;

namespace Application.Services.Interfaces
{
    public interface IIncentiveScanService
    {
        /// <summary>
        /// Parses the incentive code, routes to the correct table, and returns a unified scan result.
        /// </summary>
        Task<IncentiveScanResult> ScanAsync(string code);
    }
}
