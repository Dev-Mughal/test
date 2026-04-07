namespace Domain
{
    public class BusinessUserBusiness
    {
        public long Id { get; set; }
        public int BusinessId { get; set; }
        public long UserId { get; set; }
        public bool? IsDefault { get; set; }

        // Navigation Properties
        public virtual Business Business { get; set; } = null!;
        public virtual BusinessUser BusinessUser { get; set; } = null!;
    }
}
