using LocalDropshipping.Web.Data.Entities;
using Newtonsoft.Json;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace LocalDropshipping.Web.Models
{
    public class ProductViewModel
    {
        public ProductViewModel() { }
        public ProductViewModel(Product? product)
        {
            if (product != null)
            {
                ProductId = product.ProductId;
                Name = product.Name;
                Description = product.Description;
                Price = product.Price;
                CategoryId = product.CategoryId;
                IsNewArravial = product.IsNewArravial;
                IsBestSelling = product.IsBestSelling;
                IsFeatured = product.IsFeatured;
                Quantity = product.Quantity;
                SKU = product.SKU;
            }
        }


        public int ProductId { get; set; }

        [Required]
        public string? Name { get; set; }


        [Required]
        public string? Description { get; set; }


        [Required]
        [Range(0, int.MaxValue)]
        public int Price { get; set; }


        [Required]
        [DisplayName("Category")]
        public int CategoryId { get; set; }

        public bool IsNewArravial { get; set; }
        public bool IsBestSelling { get; set; }
        public bool IsFeatured { get; set; }


        [Required]
        public int Quantity { get; set; }


        [Required(ErrorMessage = "SKU is required")]
        public string? SKU { get; set; }

        public Product ToEntity()
        {
            return JsonConvert.DeserializeObject<Product>(JsonConvert.SerializeObject(this))!;
        }

    }
}
