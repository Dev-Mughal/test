namespace Domain
{
    /// <summary>Lookup table for store credit transaction reasons.</summary>
    public class StoreCreditReason
    {
        public int Id { get; set; }
        public string ReasonDescription { get; set; } = null!;
    }
}
