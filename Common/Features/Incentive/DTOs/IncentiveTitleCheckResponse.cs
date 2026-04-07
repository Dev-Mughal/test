namespace Common.Features.Incentive.DTOs
{
    /// <summary>
    /// Response from checking if an incentive title already exists.
    /// <c>IsDuplicate</c> = true when a duplicate is found (warning, not error).
    /// <c>Message</c> = null if no duplicate; user-facing warning if duplicate found.
    /// </summary>
    public record IncentiveTitleCheckResponse(
        bool IsDuplicate,
        string? Message);
}
