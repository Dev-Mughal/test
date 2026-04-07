namespace Application.Services.Interfaces
{
    public interface IGeoService
    {
        /// <summary>
        /// Resolves the <c>State_City_ID</c> (L50) and <c>State_City_Zip_ID</c> (L51)
        /// primary keys for the given address components.
        /// <para>
        /// When <paramref name="forceCreate"/> is <c>false</c> (default) and either
        /// combination is not found, a <see cref="Common.Exceptions.GeoLocationNotFoundException"/>
        /// is thrown so the API can return 422 and prompt the user to confirm.
        /// </para>
        /// <para>
        /// When <paramref name="forceCreate"/> is <c>true</c>, missing records are
        /// inserted into L50/L51 with <c>UserInput = true</c> for later admin review.
        /// </para>
        /// </summary>
        Task<(long StateCityId, long StateCityZipId)> ResolveGeoIdsAsync(
            string city, string state, string zipCode, bool forceCreate);

        /// <summary>
        /// Searches geo lookup tables and returns UI-friendly dropdown options.
        /// Alphabetic input searches city/state; numeric input searches zip data.
        /// </summary>
        Task<IReadOnlyList<Common.Features.Business.DTOs.LocationLookupItemDto>> SearchLocationsAsync(
            string search,
            int take = 15);

        /// <summary>
        /// Resolves a location key (<c>C-{id}</c> or <c>Z-{id}</c>) to business geo foreign keys.
        /// </summary>
        Task<(long? StateCityId, long? StateCityZipId)> ResolveLocationKeyAsync(string locationKey);
    }
}
