namespace Domain
{
    /// <summary>Maps to <c>22C_StorePointAction</c> — a single store point transaction.</summary>
    public class StorePointAction
    {
        public long Id { get; set; }
        public long EntitlementId { get; set; }
        public int PointAmount { get; set; }
        public long CashierId { get; set; }
        public DateTime TransactionDate { get; set; }
        public bool IsTransfer { get; set; }

        // Navigation Properties
        public virtual StorePointUserEnt StorePointUserEnt { get; set; } = null!;
    }
}
