namespace Domain
{
    /// <summary>Maps to <c>12C_StampAction</c> — a single stamp transaction on a customer's stamp card.</summary>
    public class StampAction
    {
        public long Id { get; set; }
        public long EntitlementId { get; set; }
        public long CashierId { get; set; }
        public DateTime TransactionDate { get; set; }
        public string? Note { get; set; }
        public bool IsVoided { get; set; }

        // Navigation Properties
        public virtual StampUserEnt StampUserEnt { get; set; } = null!;
    }
}
