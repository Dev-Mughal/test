namespace Application.Utilities.Cache
{
    /// <summary>
    /// Thin abstraction over IMemoryCache with group-based bulk invalidation.
    /// <para>
    /// Two cache modes are supported:
    /// <list type="bullet">
    ///   <item><see cref="GetOrCreateAsync{T}"/> — standalone entry, expires by time.</item>
    ///   <item><see cref="GetOrCreateInGroupAsync{T}"/> — entry linked to a named group;
    ///     calling <see cref="InvalidateGroup"/> evicts every entry in that group at once.</item>
    /// </list>
    /// </para>
    /// </summary>
    public interface ICacheService
    {
        /// <summary>Returns a cached value or executes <paramref name="factory"/> and stores the result.</summary>
        Task<T> GetOrCreateAsync<T>(string key, Func<Task<T>> factory, TimeSpan absoluteExpiration);

        /// <summary>
        /// Same as <see cref="GetOrCreateAsync{T}"/> but links the entry to a named group.
        /// All entries in the group are evicted together when <see cref="InvalidateGroup"/> is called.
        /// </summary>
        Task<T> GetOrCreateInGroupAsync<T>(string key, string groupKey, Func<Task<T>> factory, TimeSpan absoluteExpiration);

        /// <summary>Removes a single entry by key.</summary>
        void Remove(string key);

        /// <summary>Cancels the group token, evicting every entry linked to <paramref name="groupKey"/>.</summary>
        void InvalidateGroup(string groupKey);
    }
}
