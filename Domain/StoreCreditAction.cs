namespace Domain
{
    /// <summary>Maps to <c>BP_LogStoreCredit</c> — store credit transaction log.</summary>
    public class StoreCreditAction
    {
        public long Id { get; set; }
        public long EntitlementId { get; set; }
        public int TransAmount { get; set; }
        public long CashierId { get; set; }
        public DateTime TransDate { get; set; }
        public string? Note { get; set; }
        public int? ReasonId { get; set; }

        // Navigation Properties
        public virtual StoreCreditUserEnt StoreCreditUserEnt { get; set; } = null!;
    }
}
