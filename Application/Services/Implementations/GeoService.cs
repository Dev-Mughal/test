using Application.Services.Interfaces;
using Common.Exceptions;
using Common.Features.Business.DTOs;
using Domain;
using Infrastructure;
using Infrastructure.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Application.Services.Implementations
{
    public class GeoService(IDbContextFactory<BizyPopDbContext> contextFactory) : IGeoService
    {
        public async Task<(long StateCityId, long StateCityZipId)> ResolveGeoIdsAsync(
            string city, string state, string zipCode, bool forceCreate)
        {
            // Normalise input so lookups are case-insensitive without a DB collation change.
            var cityNorm  = city.Trim();
            var stateNorm = state.Trim();
            var zipNorm   = zipCode.Trim();

            var (geoCity, geoZip) = await contextFactory.QueryWithDbContextAsync(async context =>
            {
                var foundCity = await context.GeoCities
                    .AsNoTracking()
                    .FirstOrDefaultAsync(g =>
                        g.City.ToLower()  == cityNorm.ToLower() &&
                        g.State.ToLower() == stateNorm.ToLower())
                    .ConfigureAwait(false);

                var foundZip = await context.GeoZipCodes
                    .AsNoTracking()
                    .FirstOrDefaultAsync(g =>
                        g.City.ToLower()    == cityNorm.ToLower()  &&
                        g.State.ToLower()   == stateNorm.ToLower() &&
                        g.ZipCode.ToLower() == zipNorm.ToLower())
                    .ConfigureAwait(false);

                return (foundCity, foundZip);
            }).ConfigureAwait(false);

            // Neither found — neither found. Determine which part is missing for the message.
            if (geoCity is null && geoZip is null && !forceCreate)
                throw new GeoLocationNotFoundException(cityNorm, stateNorm, zipNorm);

            // City/State not found but zip is (edge case) — still a bad combination.
            if (geoCity is null && !forceCreate)
                throw new GeoLocationNotFoundException(cityNorm, stateNorm);

            // Zip not found but city/state is — check the zip specifically.
            if (geoZip is null && !forceCreate)
                throw new GeoLocationNotFoundException(cityNorm, stateNorm, zipNorm);

            // forceCreate = true path: insert any missing records with UserInput flag.
            return await contextFactory.WriteWithDbContextAsync(async context =>
            {
                GeoCity resolvedCity;
                if (geoCity is null)
                {
                    resolvedCity = new GeoCity
                    {
                        City      = cityNorm,
                        State     = stateNorm,
                        UserInput = true   // flagged for admin review
                    };
                    await context.GeoCities.AddAsync(resolvedCity).ConfigureAwait(false);
                    await context.SaveChangesAsync().ConfigureAwait(false);
                }
                else
                {
                    // Re-attach the tracked entity for this write context.
                    resolvedCity = await context.GeoCities.FindAsync(geoCity.Id).ConfigureAwait(false)
                                   ?? geoCity;
                }

                GeoZipCode resolvedZip;
                if (geoZip is null)
                {
                    resolvedZip = new GeoZipCode
                    {
                        City      = cityNorm,
                        State     = stateNorm,
                        ZipCode   = zipNorm,
                        UserInput = true   // flagged for admin review
                    };
                    await context.GeoZipCodes.AddAsync(resolvedZip).ConfigureAwait(false);
                    await context.SaveChangesAsync().ConfigureAwait(false);
                }
                else
                {
                    resolvedZip = await context.GeoZipCodes.FindAsync(geoZip.Id).ConfigureAwait(false)
                                  ?? geoZip;
                }

                return (resolvedCity.Id, resolvedZip.Id);
            }).ConfigureAwait(false);
        }

        public Task<IReadOnlyList<LocationLookupItemDto>> SearchLocationsAsync(string search, int take = 80)
        {
            if (string.IsNullOrWhiteSpace(search))
                return Task.FromResult<IReadOnlyList<LocationLookupItemDto>>([]);

            var term = search.Trim();
            var hasNumeric = term.Any(char.IsDigit);

            return hasNumeric
                ? contextFactory.QueryWithDbContextAsync(async context =>
                    (IReadOnlyList<LocationLookupItemDto>)await context.GeoZipCodes
                        .AsNoTracking()
                        .Where(z => z.ZipCode.StartsWith(term))
                        .OrderBy(z => z.ZipCode)
                        .ThenBy(z => z.City)
                        .Take(take)
                        .Select(z => new LocationLookupItemDto(
                            $"Z-{z.Id}",
                            $"{z.City}, {FormatState(z.State)}, {z.ZipCode}"))
                        .ToListAsync()
                        .ConfigureAwait(false))
                : contextFactory.QueryWithDbContextAsync(async context =>
                    (IReadOnlyList<LocationLookupItemDto>)await context.GeoCities
                        .AsNoTracking()
                        .Where(g =>
                            g.City.ToLower().StartsWith(term.ToLower()) 
                            //||
                            //g.State.ToLower().Contains(term.ToLower())
                            )
                        .OrderBy(g => g.City)
                        .ThenBy(g => g.State)
                        .Take(take)
                        .Select(g => new LocationLookupItemDto(
                            $"C-{g.Id}",
                            $"{g.City}, {FormatState(g.State)}"))
                        .ToListAsync()
                        .ConfigureAwait(false));
        }

        public async Task<(long? StateCityId, long? StateCityZipId)> ResolveLocationKeyAsync(string locationKey)
        {
            if (string.IsNullOrWhiteSpace(locationKey))
                return (null, null);

            var parts = locationKey.Trim().Split('-', 2, StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length != 2 || !long.TryParse(parts[1], out var id))
                throw new ResourceNotFoundException($"Invalid location key '{locationKey}'.");

            if (parts[0].Equals("C", StringComparison.OrdinalIgnoreCase))
            {
                var cityId = await contextFactory.QueryWithDbContextAsync(async context =>
                    await context.GeoCities
                        .AsNoTracking()
                        .Where(g => g.Id == id)
                        .Select(g => (long?)g.Id)
                        .FirstOrDefaultAsync()
                        .ConfigureAwait(false)).ConfigureAwait(false);

                return cityId is null
                    ? throw new ResourceNotFoundException($"Location key '{locationKey}' was not found.")
                    : (cityId, null);
            }

            if (parts[0].Equals("Z", StringComparison.OrdinalIgnoreCase))
            {
                var zipId = await contextFactory.QueryWithDbContextAsync(async context =>
                    await context.GeoZipCodes
                        .AsNoTracking()
                        .Where(g => g.Id == id)
                        .Select(g => (long?)g.Id)
                        .FirstOrDefaultAsync()
                        .ConfigureAwait(false)).ConfigureAwait(false);

                return zipId is null
                    ? throw new ResourceNotFoundException($"Location key '{locationKey}' was not found.")
                    : (null, zipId);
            }

            throw new ResourceNotFoundException($"Invalid location key prefix in '{locationKey}'.");
        }

        private static string FormatState(string state) =>
            state.Length <= 2 ? state.ToUpperInvariant() : state[..2].ToUpperInvariant();
    }
}
