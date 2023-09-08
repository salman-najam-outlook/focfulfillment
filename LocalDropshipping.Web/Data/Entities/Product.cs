
using System.ComponentModel.DataAnnotations;

namespace LocalDropshipping.Web.Data.Entities
{
    public class Product
    {
        public int ProductId { get; set; }
        [Required (ErrorMessage ="Product Name required")]
        public string? Name { get; set; }
        [Required(ErrorMessage = "Description is required")]
        public string? Description { get; set; }
        [Required]
        public int Price { get; set; }
        [Required]
        public int CategoryId { get; set; }
        public bool IsNewArravial { get; set; }
        public bool IsBestSelling { get; set; }
        public bool IsFeatured { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public DateTime UpdatedDate { get; set; }= DateTime.Now;
        public string? UpdatedBy { get; set; }
        public string? CreatedBy { get; set; }
        [Required]
        public int Quantity { get; set; }

        [Required(ErrorMessage = "ImageContent is required")]
        public string? ImageContent { get; set; }

        [Required(ErrorMessage = "SKU is required")]
        public string? SKU { get; set; }
        public bool IsDeleted { get; set; }
        public virtual Category? Category { get; set; }

        //public string? ImageLink { get; set; }
        //public int Stock { get; set; }


    }
}