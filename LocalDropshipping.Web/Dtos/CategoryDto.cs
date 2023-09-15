using LocalDropshipping.Web.Data.Entities;
using Newtonsoft.Json;

namespace LocalDropshipping.Web.Dtos
{
    public class CategoryDto
    {
        public string? Name { get; set; }
        public DateTime CreatedDate { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime ModifiedDate { get; set; }
        public string? ModifiedBy { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; } = false;
        public string? ImagePath { get; set; }
       

		internal Category ToEntity()
        {
            return JsonConvert.DeserializeObject<Category>(JsonConvert.SerializeObject(this))!;
        }
    }
}
