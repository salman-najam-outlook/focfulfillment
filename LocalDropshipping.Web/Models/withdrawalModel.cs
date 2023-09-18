using LocalDropshipping.Web.Data.Entities;
using LocalDropshipping.Web.Enums;

namespace LocalDropshipping.Web.Models
{
    public class withdrawalModel
    {
        public int Id { get; set; }
        //  public int User_Id { get; set; }
        public decimal GrandTotal { get; set; }
        public string? Name { get; set; }
        public string? SpecialInstructions { get; set; }
        public string? OrderCode { get; set; }
        public bool IsDeleted { get; set; } = false;
        public DateTime OrderDate { get; set; } = DateTime.Now;
        public OrderStatus OrderStatus { get; set; }

        public virtual List<OrderItem> Orderitems { get; set; }
    }
}
