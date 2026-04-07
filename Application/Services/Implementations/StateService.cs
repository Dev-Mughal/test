using Application.Services.Interfaces;
using Application.Utilities.Cache;
using Domain;
using Infrastructure;
using Infrastructure.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Application.Services.Implementations
{
    public class StateService(
        IDbContextFactory<BizyPopDbContext> contextFactory,
        ICacheService cache) : IStateService
    {
        // States are curated, static data — cache for 1 day with no invalidation needed
        private static readonly TimeSpan StatesCacheTtl = TimeSpan.FromHours(24);
        private const string StatesCacheKey = "states:all:lookup";

        public async Task<Dictionary<string, string>> GetStatesLookupAsync() =>
            await cache.GetOrCreateAsync(StatesCacheKey, async () =>
            {
                var states = await contextFactory.QueryWithDbContextAsync(async context =>
                    await context.States
                        .AsNoTracking()
                        .Where(s => s.IsActive)
                        .OrderBy(s => s.Name)
                        .ToListAsync()
                        .ConfigureAwait(false)
                ).ConfigureAwait(false);

                // Transform to { "CA": "California (CA)", "TX": "Texas (TX)", ... }
                return states
                    .ToDictionary(
                        s => s.Code,
                        s => $"{s.Name} ({s.Code})",
                        StringComparer.OrdinalIgnoreCase);
            }, StatesCacheTtl);

        public async Task<State?> ValidateStateCodeAsync(string code) =>
            await contextFactory.QueryWithDbContextAsync(async context =>
                await context.States
                    .AsNoTracking()
                    .FirstOrDefaultAsync(s =>
                        s.Code.ToLower() == code.ToLower() && s.IsActive)
                    .ConfigureAwait(false)
            ).ConfigureAwait(false);
    }
}
