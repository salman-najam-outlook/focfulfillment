namespace LocalDropshipping.Web.Data.Entities
{
    public class ProductVariant
    {
#nullable disable
        public int ProductVariantId { get; set; }
        public int ProductId { get; set; }
        public string? VariantType { get; set; }
        public string Variant { get; set; }
        public int VariantPrice { get; set; }
        public string FeatureImageLink { get; set; }
        public int Quantity { get; set; }
        public bool IsMainVariant { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public virtual List<ProductVariantImage> Images { get; set; }
        public virtual List<ProductVariantVideo> Videos { get; set; }
    }
}
