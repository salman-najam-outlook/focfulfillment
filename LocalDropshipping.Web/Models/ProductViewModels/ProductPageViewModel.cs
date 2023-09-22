using LocalDropshipping.Web.Data.Entities;

namespace LocalDropshipping.Web.Models.ProductViewModels
{
    public class ProductPageViewModel
    {
        public Product Product { get; set; }
        public List<Product> RelatedProducts { get; set;  }
    }
}
