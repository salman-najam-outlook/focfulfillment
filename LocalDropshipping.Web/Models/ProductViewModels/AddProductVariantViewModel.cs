using LocalDropshipping.Web.Data.Entities;

namespace LocalDropshipping.Web.Models.ProductViewModels
{
    public class AddProductVariantViewModel
    {
        public string FeatureImageLink { get; set; }
        public int Quantity { get; set; }
        public string VariantType { get; set; }
        public int VariantPrice { get; set; }
        public virtual List<WishList> WishLists { get; set; }

    }
}

