namespace Application.Services.Interfaces
{
    public interface IStateService
    {
        /// <summary>
        /// Returns all active states as a dictionary keyed by 2-character code.
        /// Format: { "CA" => "California (CA)", "TX" => "Texas (TX)", ... }
        /// </summary>
        Task<Dictionary<string, string>> GetStatesLookupAsync();

        /// <summary>
        /// Validates that the given 2-character state code exists in L52_Geo_States.
        /// Returns the matching State entity or null if not found.
        /// </summary>
        Task<Domain.State?> ValidateStateCodeAsync(string code);
    }
}
