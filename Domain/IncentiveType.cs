namespace Domain
{
    public class IncentiveType
    {
        public int Id { get; set; }
        public string TypeDescription { get; set; } = null!;
        public string TypeCode { get; set; } = null!;
        public bool IsActive { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
