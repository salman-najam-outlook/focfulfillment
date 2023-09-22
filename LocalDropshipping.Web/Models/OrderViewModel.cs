using LocalDropshipping.Web.Data.Entities;
using LocalDropshipping.Web.Enums;

namespace LocalDropshipping.Web.Models
{
    public class OrderViewModel
    {
        public int OrderId { get; set; }
        public string OrderTrackingId { get; set; }
        public OrderStatus OrderStatus { get; set; }
        public CourierServiceType CourierServiceType { get; set; }
        public string? SpecialInstructions { get; set; }
        public string UpdatedBy { get; set; }
    }
}
