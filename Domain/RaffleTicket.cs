namespace Domain
{
    /// <summary>Maps to <c>41C_RaffleTicket</c> — a single raffle ticket issued for a drawing.</summary>
    public class RaffleTicket
    {
        public long Id { get; set; }
        public long RaffleId { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public string CreationCode { get; set; } = null!;

        // Navigation Properties
        public virtual RaffleDef RaffleDef { get; set; } = null!;
    }
}
