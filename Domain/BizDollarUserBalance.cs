namespace Domain
{
    /// <summary>Maps to <c>01B_BizDollarUserBalance</c> — customer BizyPop dollar balance wallet.</summary>
    public class BizDollarUserBalance
    {
        public long Id { get; set; }
        public long UserId { get; set; }
        public int Balance { get; set; }
        public int CreatedChannel { get; set; }
        public DateTime LastUpdated { get; set; }
        public DateTime Created { get; set; }

        // Navigation Properties
        public virtual Customer Customer { get; set; } = null!;
        public virtual ICollection<BizDollarAction> BizDollarActions { get; set; } = [];
    }
}
