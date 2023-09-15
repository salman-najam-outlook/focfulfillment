namespace LocalDropshipping.Web.Data.Entities
{
    public class ProductVariantImage
    {
        public int Id { get; set; }
        public int ProductVariantId { get; set; }
        public string Link { get; set; }
    }
}
