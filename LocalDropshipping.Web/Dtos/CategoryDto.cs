using LocalDropshipping.Web.Data.Entities;
using Newtonsoft.Json;

namespace LocalDropshipping.Web.Dtos
{
    public class CategoryDto
    {
        public string? Name { get; set; }
		public bool IsActive { get; set; }

		internal Category ToEntity()
        {
            return JsonConvert.DeserializeObject<Category>(JsonConvert.SerializeObject(this))!;
        }
    }
}
