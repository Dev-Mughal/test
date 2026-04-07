using Common.Logging;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Diagnostics;

namespace Infrastructure.Logging
{
    public sealed class PostgresLogger : ILogger
    {
        private readonly string _categoryName;
        private readonly IAppLogger _appLogger;
        private readonly IOptions<PostgresLoggingOptions> _options;
        private readonly Func<IExternalScopeProvider> _scopeProviderAccessor;

        public PostgresLogger(
            string categoryName,
            IAppLogger appLogger,
            IOptions<PostgresLoggingOptions> options,
            Func<IExternalScopeProvider> scopeProviderAccessor)
        {
            _categoryName = categoryName;
            _appLogger = appLogger;
            _options = options;
            _scopeProviderAccessor = scopeProviderAccessor;
        }

        public IDisposable BeginScope<TState>(TState state) where TState : notnull =>
            _scopeProviderAccessor().Push(state);

        public bool IsEnabled(LogLevel logLevel) =>
            logLevel != LogLevel.None;

        public void Log<TState>(
            LogLevel logLevel,
            EventId eventId,
            TState state,
            Exception? exception,
            Func<TState, Exception?, string> formatter)
        {
            if (!IsEnabled(logLevel) || ShouldSkipCategory(_categoryName))
            {
                return;
            }

            var message = formatter(state, exception);
            if (string.IsNullOrWhiteSpace(message))
            {
                message = exception?.Message ?? string.Empty;
            }

            if (string.IsNullOrWhiteSpace(message))
            {
                return;
            }

            var metadata = BuildMetadata(eventId, state);

            _ = _appLogger.LogAsync(new LogEntryRequest(
                Level: ToAppLogLevel(logLevel),
                Message: message,
                Category: _categoryName,
                EventName: eventId.Name,
                Metadata: metadata,
                Exception: exception,
                CorrelationId: Activity.Current?.TraceId.ToString()
            ));
        }

        private bool ShouldSkipCategory(string categoryName)
        {
            var options = _options.Value;

            if (!options.CaptureFrameworkLogs &&
                (categoryName.StartsWith("Microsoft.", StringComparison.OrdinalIgnoreCase)
                || categoryName.StartsWith("System.", StringComparison.OrdinalIgnoreCase)))
            {
                return true;
            }

            var excludedPrefixes = options.ExcludedCategoryPrefixes;
            if (excludedPrefixes is null || excludedPrefixes.Count == 0)
            {
                return false;
            }

            return excludedPrefixes.Any(prefix =>
                !string.IsNullOrWhiteSpace(prefix)
                && categoryName.StartsWith(prefix, StringComparison.OrdinalIgnoreCase));
        }

        private IReadOnlyDictionary<string, object?> BuildMetadata<TState>(EventId eventId, TState state)
        {
            var metadata = new Dictionary<string, object?>
            {
                ["EventId"] = eventId.Id
            };

            if (!string.IsNullOrWhiteSpace(eventId.Name))
            {
                metadata["EventName"] = eventId.Name;
            }

            AddStructuredState(metadata, state);
            AddScopes(metadata);

            return metadata;
        }

        private static void AddStructuredState<TState>(Dictionary<string, object?> metadata, TState state)
        {
            if (state is not IEnumerable<KeyValuePair<string, object?>> values)
            {
                return;
            }

            foreach (var (key, value) in values)
            {
                if (key.Equals("{OriginalFormat}", StringComparison.Ordinal))
                {
                    continue;
                }

                metadata[key] = value;
            }
        }

        private void AddScopes(Dictionary<string, object?> metadata)
        {
            var scopes = new List<object?>();
            _scopeProviderAccessor().ForEachScope((scope, state) => state.Add(scope), scopes);

            if (scopes.Count > 0)
            {
                metadata["Scopes"] = scopes;
            }
        }

        private static AppLogLevel ToAppLogLevel(LogLevel logLevel) => logLevel switch
        {
            LogLevel.Trace => AppLogLevel.Trace,
            LogLevel.Debug => AppLogLevel.Debug,
            LogLevel.Information => AppLogLevel.Information,
            LogLevel.Warning => AppLogLevel.Warning,
            LogLevel.Error => AppLogLevel.Error,
            LogLevel.Critical => AppLogLevel.Critical,
            _ => AppLogLevel.Information
        };
    }
}
