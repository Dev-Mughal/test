namespace Common.Features.State.DTOs
{
    /// <summary>
    /// State lookup DTO — minimal data for the client dropdown/reference.
    /// Used in the response format: { "CA": "California (CA)", "TX": "Texas (TX)", ... }
    /// </summary>
    public record StateDto(
        string Code,           // "CA", "TX", "NY"
        string Name,           // "California", "Texas", "New York"
        string? Region         // "West", "South", optional for grouping
    );
}
