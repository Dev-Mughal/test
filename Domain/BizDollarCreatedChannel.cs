namespace Domain
{
    /// <summary>Lookup table for BizyPop Dollar creation channels.</summary>
    public class BizDollarCreatedChannel
    {
        public int Id { get; set; }
        public int ChannelCode { get; set; }
        public string ChannelDescription { get; set; } = null!;
    }
}
