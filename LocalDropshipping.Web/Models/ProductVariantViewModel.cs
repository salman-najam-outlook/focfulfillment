namespace LocalDropshipping.Web.Models
{
    public class ProductVariantViewModel
    {
        public int VariantId { get; set; }
        public string VariantType { get; set; }
        public int VariantPrice { get; set; }
        public string FeatureImageLink { get; set; }
        public int Quantity { get; set; }
    }
}
