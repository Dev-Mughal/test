namespace Domain
{
    /// <summary>Maps to <c>41W_RaffleWinner</c> — records the winner and prize awarded for a raffle drawing.</summary>
    public class RaffleWinner
    {
        public long Id { get; set; }
        public long RaffleId { get; set; }
        public long UserId { get; set; }
        public decimal StoreCreditAmount { get; set; }
        public decimal GiftCardAmount { get; set; }
        public DateTime Created { get; set; }

        // Navigation Properties
        public virtual RaffleDef RaffleDef { get; set; } = null!;
        public virtual Customer Customer { get; set; } = null!;
    }
}
