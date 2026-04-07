using Common.Logging;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using System.Text.Json;
using System.Threading.Channels;

namespace Infrastructure.Logging
{
    public class PostgresAppLogger : BackgroundService, IAppLogger
    {
        private readonly IDbContextFactory<LoggingDbContext> _loggingDbContextFactory;
        private readonly Channel<AppLogEntry> _logChannel;
        private readonly JsonSerializerOptions _jsonSerializerOptions = new(JsonSerializerDefaults.Web);
        private readonly int _batchSize;

        public PostgresAppLogger(
            IDbContextFactory<LoggingDbContext> loggingDbContextFactory,
            IOptions<PostgresLoggingOptions> options)
        {
            _loggingDbContextFactory = loggingDbContextFactory;

            var resolvedOptions = options.Value;
            _batchSize = resolvedOptions.BatchSize <= 0 ? 200 : resolvedOptions.BatchSize;

            var capacity = resolvedOptions.ChannelCapacity <= 0 ? 20000 : resolvedOptions.ChannelCapacity;
            _logChannel = Channel.CreateBounded<AppLogEntry>(new BoundedChannelOptions(capacity)
            {
                FullMode = BoundedChannelFullMode.DropWrite,
                SingleReader = true,
                SingleWriter = false,
                AllowSynchronousContinuations = false
            });
        }

        public ValueTask LogAsync(LogEntryRequest entry, CancellationToken cancellationToken = default)
        {
            var logRow = new AppLogEntry
            {
                CreatedAtUtc = DateTime.UtcNow,
                Level = (short)entry.Level,
                Message = entry.Message,
                Category = entry.Category,
                EventName = entry.EventName,
                CorrelationId = entry.CorrelationId,
                RequestPath = entry.RequestPath,
                HttpMethod = entry.HttpMethod,
                ExceptionType = entry.Exception?.GetType().FullName,
                ExceptionMessage = entry.Exception?.Message,
                StackTrace = entry.Exception?.StackTrace,
                Metadata = ToJsonDocument(entry.Metadata),
                Data = ToJsonDocument(entry.Data)
            };

            _logChannel.Writer.TryWrite(logRow);
            return ValueTask.CompletedTask;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await EnsureLoggingDatabaseAsync(stoppingToken).ConfigureAwait(false);

            var batch = new List<AppLogEntry>(_batchSize);

            try
            {
                while (!stoppingToken.IsCancellationRequested)
                {
                    var nextLog = await _logChannel.Reader.ReadAsync(stoppingToken).ConfigureAwait(false);
                    batch.Add(nextLog);

                    while (batch.Count < _batchSize && _logChannel.Reader.TryRead(out var bufferedLog))
                    {
                        batch.Add(bufferedLog);
                    }

                    await PersistBatchAsync(batch, stoppingToken).ConfigureAwait(false);
                    batch.Clear();
                }
            }
            catch (OperationCanceledException)
            {
            }
            finally
            {
                await DrainRemainingLogsAsync(batch, stoppingToken).ConfigureAwait(false);
            }
        }

        private async Task EnsureLoggingDatabaseAsync(CancellationToken cancellationToken)
        {
            try
            {
                await using var dbContext = await _loggingDbContextFactory.CreateDbContextAsync(cancellationToken).ConfigureAwait(false);
                await dbContext.Database.EnsureCreatedAsync(cancellationToken).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Logging database initialization failed: {ex.Message}");
            }
        }

        private async Task PersistBatchAsync(List<AppLogEntry> batch, CancellationToken cancellationToken)
        {
            if (batch.Count == 0)
            {
                return;
            }

            try
            {
                await using var dbContext = await _loggingDbContextFactory.CreateDbContextAsync(cancellationToken).ConfigureAwait(false);
                await dbContext.AppLogs.AddRangeAsync(batch, cancellationToken).ConfigureAwait(false);
                await dbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to persist log batch of {batch.Count} rows: {ex.Message}");
            }
        }

        private async Task DrainRemainingLogsAsync(List<AppLogEntry> buffer, CancellationToken cancellationToken)
        {
            while (_logChannel.Reader.TryRead(out var pendingLog))
            {
                buffer.Add(pendingLog);

                if (buffer.Count >= _batchSize)
                {
                    await PersistBatchAsync(buffer, cancellationToken).ConfigureAwait(false);
                    buffer.Clear();
                }
            }

            if (buffer.Count > 0)
            {
                await PersistBatchAsync(buffer, cancellationToken).ConfigureAwait(false);
            }
        }

        private JsonDocument? ToJsonDocument(object? value)
        {
            if (value is null)
            {
                return null;
            }

            var serialized = JsonSerializer.Serialize(value, _jsonSerializerOptions);
            return JsonDocument.Parse(serialized);
        }
    }
}
