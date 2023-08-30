namespace LocalDropshipping.Web.Models
{
    public class AddProductViewModel
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public string? ImageLink { get; set; }
        public int Stock { get; set; }
        public bool IsFeatured { get; set; }
        public int CategoryId { get; set; }
    }
}
