namespace Common.Logging
{
    public interface IAppLogger
    {
        ValueTask LogAsync(LogEntryRequest entry, CancellationToken cancellationToken = default);
    }
}
