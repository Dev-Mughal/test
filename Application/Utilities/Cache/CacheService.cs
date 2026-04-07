using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Primitives;

namespace Application.Utilities.Cache
{
    /// <summary>
    /// IMemoryCache-backed implementation of <see cref="ICacheService"/>.
    ///
    /// Group invalidation works via <see cref="CancellationChangeToken"/>:
    /// every entry in a group is linked to a shared <see cref="CancellationTokenSource"/>.
    /// Cancelling the token causes the memory cache to evict all linked entries automatically.
    /// </summary>
    public sealed class CacheService(IMemoryCache cache) : ICacheService
    {
        private readonly Dictionary<string, CancellationTokenSource> _groups = [];
        private readonly Lock _lock = new();

        public async Task<T> GetOrCreateAsync<T>(
            string key,
            Func<Task<T>> factory,
            TimeSpan absoluteExpiration)
        {
            if (cache.TryGetValue(key, out T? cached))
                return cached!;

            var value = await factory().ConfigureAwait(false);
            cache.Set(key, value, absoluteExpiration);
            return value;
        }

        public async Task<T> GetOrCreateInGroupAsync<T>(
            string key,
            string groupKey,
            Func<Task<T>> factory,
            TimeSpan absoluteExpiration)
        {
            if (cache.TryGetValue(key, out T? cached))
                return cached!;

            var value = await factory().ConfigureAwait(false);

            var options = new MemoryCacheEntryOptions()
                .SetAbsoluteExpiration(absoluteExpiration)
                .AddExpirationToken(new CancellationChangeToken(GetOrCreateGroupCts(groupKey).Token));

            cache.Set(key, value, options);
            return value;
        }

        public void Remove(string key) => cache.Remove(key);

        public void InvalidateGroup(string groupKey)
        {
            lock (_lock)
            {
                if (!_groups.TryGetValue(groupKey, out var cts))
                    return;

                cts.Cancel();
                cts.Dispose();
                _groups.Remove(groupKey);
            }
        }

        // ??? Private helpers ?????????????????????????????????????????????????

        private CancellationTokenSource GetOrCreateGroupCts(string groupKey)
        {
            lock (_lock)
            {
                if (!_groups.TryGetValue(groupKey, out var cts) || cts.IsCancellationRequested)
                {
                    cts = new CancellationTokenSource();
                    _groups[groupKey] = cts;
                }
                return cts;
            }
        }
    }
}
