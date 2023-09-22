namespace LocalDropshipping.Web.Models.ProductViewModels
{
    public class ProductVariantViewModel
    {
        public int VariantId { get; set; }
        public string VariantType { get; set; }
        public int VariantPrice { get; set; }
        public int DiscountedPrice { get; set; }
        public string FeatureImageLink { get; set; }
        public int Quantity { get; set; }
        public string Variant { get; set; }
        public bool IsDeleted { get; set; }
        public List<string> Images { get; set; }
        public List<string> Videos { get; set; }
    }
}
