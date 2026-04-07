namespace Domain
{
    /// <summary>
    /// L52_Geo_States — US state reference data.
    /// Stores the authoritative list of valid state codes (2-char) and names.
    /// Used to validate City/State/ZipCode submissions and provide dropdowns to the client.
    /// </summary>
    public class State
    {
        public long Id { get; set; }

        // Two-character postal abbreviation (e.g., "CA", "TX", "NY")
        public string Code { get; set; } = null!;

        // Full state name (e.g., "California", "Texas", "New York")
        public string Name { get; set; } = null!;

        // Region for grouping (e.g., "West", "South", "Northeast") — optional for UI organization
        public string? Region { get; set; }

        public bool IsActive { get; set; } = true;
        public DateTime CreatedOn { get; set; }
    }
}
