using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LocalDropshipping.Web.Data.Entities
{
    public class ProductVariant
    {
#nullable disable
        [Key]
     
        public int VariantId { get; set; }
        public int ProductId { get; set; }
        public string VariantType { get; set; } = "MAIN_VARIANT";
        public int VariantPrice { get; set; }
        public string FeatureImageLink { get; set; }
        public int Quantity { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string UpdatedBy { get; set; }

    }
}
