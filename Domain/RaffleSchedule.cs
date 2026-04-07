namespace Domain
{
    /// <summary>Maps to <c>40B_RaffleSchedule</c> — a generated drawing schedule entry for a raffle.</summary>
    public class RaffleSchedule
    {
        public long Id { get; set; }
        public long RaffleId { get; set; }
        public string QRCode { get; set; } = null!;
        public DateTime DateOfDrawing { get; set; }
        public DateTime ProcessingStartDate { get; set; }
        public DateTime ProcessingEndDate { get; set; }

        // Navigation Properties
        public virtual RaffleDef RaffleDef { get; set; } = null!;
    }
}
