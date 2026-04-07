using Common.Logging;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Infrastructure.Logging
{
    public sealed class PostgresLoggerProvider : ILoggerProvider, ISupportExternalScope
    {
        private readonly IAppLogger _appLogger;
        private readonly IOptions<PostgresLoggingOptions> _options;
        private IExternalScopeProvider _scopeProvider = new LoggerExternalScopeProvider();

        public PostgresLoggerProvider(IAppLogger appLogger, IOptions<PostgresLoggingOptions> options)
        {
            _appLogger = appLogger;
            _options = options;
        }

        public ILogger CreateLogger(string categoryName) =>
            new PostgresLogger(categoryName, _appLogger, _options, () => _scopeProvider);

        public void SetScopeProvider(IExternalScopeProvider scopeProvider)
        {
            _scopeProvider = scopeProvider;
        }

        public void Dispose()
        {
        }
    }
}
