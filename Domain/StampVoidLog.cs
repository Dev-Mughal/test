namespace Domain
{
    /// <summary>Maps to <c>12V_StampVoidLog</c> — audit log for voided stamp card actions.</summary>
    public class StampVoidLog
    {
        public long Id { get; set; }
        public long EntitlementId { get; set; }
        public string Reason { get; set; } = null!;
        public long CashierId { get; set; }

        // Navigation Properties
        public virtual StampUserEnt StampUserEnt { get; set; } = null!;
    }
}
