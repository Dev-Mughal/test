namespace Domain
{
    /// <summary>
    /// Maps to <c>40A_RaffleDef</c> — the business-facing raffle program definition.
    /// <c>ScheduleType</c> determines which scheduling column is active:
    /// 1 = day-of-week, 2 = month day, 3 = specific date.
    /// </summary>
    public class RaffleDef
    {
        public long Id { get; set; }
        public int BusinessId { get; set; }
        public bool Enabled { get; set; }
        public string Name { get; set; } = null!;
        public string QRCode { get; set; } = null!;
        public int? MinimumEntry { get; set; }
        public decimal? GiftCardValue { get; set; }
        public decimal? StoreCreditValue { get; set; }
        public string? CustomPrize { get; set; }
        public decimal? CustomPrizeValue { get; set; }
        public int? ScheduleType { get; set; }

        // Maps to "2_Day of the week" — weekly drawing schedule.
        public int? DrawingDayOfWeek { get; set; }

        // Maps to "3_DrawingMonthDay" — monthly drawing schedule.
        public int? DrawingMonthDay { get; set; }

        // Maps to "4_DateOfDrawing" — one-time specific drawing date.
        public DateTime? DateOfDrawing { get; set; }

        public TimeOnly? DrawingTime { get; set; }
        public DateTime? LastUpdateTime { get; set; }
        public int? TicketUsageType { get; set; }
        public int? PreviousLastDaysToUse { get; set; }

        // Navigation Properties
        public virtual Business Business { get; set; } = null!;
        public virtual ICollection<RaffleSchedule> RaffleSchedules { get; set; } = [];
        public virtual ICollection<RaffleTicket> RaffleTickets { get; set; } = [];
        public virtual ICollection<RaffleWinner> RaffleWinners { get; set; } = [];
    }
}
