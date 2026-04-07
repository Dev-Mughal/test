using System.Text.Json;

namespace Infrastructure.Logging
{
    public class AppLogEntry
    {
        public long Id { get; set; }
        public DateTime CreatedAtUtc { get; set; }
        public short Level { get; set; }
        public string Message { get; set; } = string.Empty;
        public string? Category { get; set; }
        public string? EventName { get; set; }
        public string? CorrelationId { get; set; }
        public string? RequestPath { get; set; }
        public string? HttpMethod { get; set; }
        public string? ExceptionType { get; set; }
        public string? ExceptionMessage { get; set; }
        public string? StackTrace { get; set; }
        public JsonDocument? Metadata { get; set; }
        public JsonDocument? Data { get; set; }
    }
}
