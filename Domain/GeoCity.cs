namespace Domain
{
    public class GeoCity
    {
        public long Id { get; set; }
        public string City { get; set; } = null!;
        public string State { get; set; } = null!;

        // Flagged true when the record was submitted by a user and not yet
        // verified/curated — allows admins to review and remove bad entries.
        public bool UserInput { get; set; }

        // Navigation Properties
        public virtual ICollection<Business> Businesses { get; set; } = [];
    }
}
