namespace Infrastructure.Logging
{
    public class PostgresLoggingOptions
    {
        public const string SectionName = "PostgresLogging";

        public int ChannelCapacity { get; set; } = 20000;
        public int BatchSize { get; set; } = 200;
        public bool CaptureFrameworkLogs { get; set; } = false;
        public List<string> ExcludedCategoryPrefixes { get; set; } =
        [
            "Infrastructure.Logging.PostgresAppLogger",
            "Infrastructure.Logging.PostgresLogger",
            "Infrastructure.Logging.PostgresLoggerProvider"
        ];
    }
}
