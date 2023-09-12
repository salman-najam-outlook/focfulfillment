namespace LocalDropshipping.Web.Data.Entities
{
    public class Category
    {
        public int CategoryId { get; set; }
        public string? Name { get; set; }
        public DateTime CreatedDate { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime ModifiedDate { get; set; }
        public string? ModifiedBy { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; } = false;
        public string? ImagePath { get; set; }
    }
}
