
namespace LocalDropshipping.Web.Data.Entities
{
    public class Product
    {
        public int ProductId { get; set; }

        public string? Name { get; set; }
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public int CategoryId { get; set; }
        public bool IsNewArravial { get; set; }
        public bool IsBestSelling { get; set; }
        public bool IsFeatured { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public string? UpdatedBy { get; set; }
        public string? CreatedBy { get; set; }
        public int Quantity { get; set; }
        public string ImageContent { get; set; }
        public string? SKU { get; set; }
        public bool IsDeleted { get; set; }
        public virtual Category? Category { get; set; }

        //public string? ImageLink { get; set; }
        //public int Stock { get; set; }


    }
}