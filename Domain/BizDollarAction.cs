namespace Domain
{
    /// <summary>Maps to <c>01C_BizDollarAction</c> — a single BizyPop dollar transaction.</summary>
    public class BizDollarAction
    {
        public long Id { get; set; }
        public long EntitlementId { get; set; }
        public int BusinessId { get; set; }
        public int Amount { get; set; }
        public long CashierId { get; set; }
        public DateTime TransactionDate { get; set; }
        public long UserId { get; set; }

        // Navigation Properties
        public virtual BizDollarUserBalance BizDollarUserBalance { get; set; } = null!;
        public virtual Business Business { get; set; } = null!;
    }
}
