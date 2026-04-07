namespace Common.Logging
{
    public sealed record LogEntryRequest(
        AppLogLevel Level,
        string Message,
        string? Category = null,
        string? EventName = null,
        object? Data = null,
        IReadOnlyDictionary<string, object?>? Metadata = null,
        Exception? Exception = null,
        string? CorrelationId = null,
        string? RequestPath = null,
        string? HttpMethod = null
    );
}
