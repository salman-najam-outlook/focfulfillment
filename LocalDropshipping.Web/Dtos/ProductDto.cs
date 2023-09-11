using LocalDropshipping.Web.Data.Entities;
using Newtonsoft.Json;

namespace LocalDropshipping.Web.Dtos
{
    public class ProductDto
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public int Price { get; set; }
        public string? ImageLink { get; set; }
        public int Stock { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsFeatured { get; set; }
        public int CategoryId { get; set; }

        internal Product ToEntity()
        {
            return JsonConvert.DeserializeObject<Product>(JsonConvert.SerializeObject(this))!;
        }
    }
}
