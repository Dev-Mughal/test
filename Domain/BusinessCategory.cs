namespace Domain
{
    public class BusinessCategory
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; } = null!;
        public DateTime CreatedOn { get; set; }
        public string CategorySlug { get; set; } = null!;
        public bool IsActive { get; set; }
        public short DisplayOrder { get; set; }
        public short DisplayColumn { get; set; }

        // Navigation Properties
        public virtual ICollection<Business> Businesses { get; set; } = [];
    }
}
