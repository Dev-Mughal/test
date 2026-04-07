using Microsoft.AspNetCore.Http;

namespace Common.Exceptions
{
    /// <summary>
    /// Thrown when the submitted City/State or ZipCode combination cannot be found
    /// in L50_Geo_Cities or L51_Geo_ZipCodes.
    /// Returns HTTP 422 so the client can prompt the user to confirm and re-submit
    /// with <c>ForceCreate = true</c> to insert a new user-submitted geo record.
    /// </summary>
    public class GeoLocationNotFoundException : AppException
    {
        public override int StatusCode => StatusCodes.Status422UnprocessableEntity;

        public GeoLocationNotFoundException(string city, string state, string? zipCode = null)
            : base(BuildMessage(city, state, zipCode), "GEO_LOCATION_NOT_FOUND")
        {
        }

        private static string BuildMessage(string city, string state, string? zipCode) =>
            zipCode is null
                ? $"'{city}, {state}' is not a known City/State combination. Continue Anyways?"
                : $"'{city}, {state} {zipCode}' is not a known City, State or ZipCode combination. Continue Anyways?";
    }
}
