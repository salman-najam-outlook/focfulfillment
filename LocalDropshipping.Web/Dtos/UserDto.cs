using LocalDropshipping.Web.Data.Entities;
using Newtonsoft.Json;

namespace LocalDropshipping.Web.Dtos
{
    public class UserDto
    {
        public string Name { get; set; }
        public string PhoneNumber { get; set; }

        internal User ToEntity()
        {
            return JsonConvert.DeserializeObject<User>(JsonConvert.SerializeObject(this))!;
        }
    }
}
