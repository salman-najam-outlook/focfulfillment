using LocalDropshipping.Web.Data.Entities;
using LocalDropshipping.Web.Enums;
using Newtonsoft.Json;

namespace LocalDropshipping.Web.Dtos
{
    public class OrderDto
    {
        public decimal GrandTotal { get; set; }
        public string SpecialInstructions { get; set; }
        //public string Name { get; set; }
        //public string OrderCode { get; set; }
        public OrderStatus Status { get; set; }

        internal Order ToEntity()
        {
            return JsonConvert.DeserializeObject<Order>(JsonConvert.SerializeObject(this))!;
        }
    }
}
