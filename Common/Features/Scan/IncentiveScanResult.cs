namespace Common.Features.Scan
{
    /// <summary>
    /// Unified scan response returned by <c>POST /api/scan</c>.
    /// <c>Data</c> contains the full incentive definition record for the matched table.
    /// </summary>
    public record IncentiveScanResult(
        string IncentiveType,
        string TableCode,
        long   Id,
        object Data);

    /// <summary>Request body for <c>POST /api/scan</c>.</summary>
    public record IncentiveScanRequest(string Code);
}
