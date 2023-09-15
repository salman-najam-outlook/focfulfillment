using LocalDropshipping.Web.Data.Entities;
using System.ComponentModel.DataAnnotations;

namespace LocalDropshipping.Web.Models
{
    public class OrderSuccessModel
    {
        public int OrderId { get; set; }
        public List<OrderItem>? OrderItems { get; set; }
        public int TotalItems { get; set; }
        public decimal TotalItemsAmount { get; set; }
        public decimal ShippingCharges { get; set;}
        public decimal GrandTotal { get; set; }
        public string ShippingAddress { get; set; }
    }
}
