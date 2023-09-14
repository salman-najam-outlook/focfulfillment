using LocalDropshipping.Web.Enums;
namespace LocalDropshipping.Web.Data.Entities
{
    public class Order
    {
        public int Id { get; set; }
        public string Seller { get; set; }
        public decimal GrandTotal { get; set; }
        public decimal SellPrice { get; set; }
        //public string? Name { get; set; }
        public string? SpecialInstructions { get; set; }
        //public string? OrderCode { get; set; }
        public bool IsDeleted { get; set; } = false;
        public DateTime OrderDate { get; set; } = DateTime.Now;
        public DateTime? UpdatedDate { get; set; }
        public string? UpdatedBy { get; set; }
        public string? CreatedBy { get; set; }
        public OrderStatus OrderStatus { get; set; }
        public virtual List<OrderItem> Orderitems { get; set; }
    }
}
