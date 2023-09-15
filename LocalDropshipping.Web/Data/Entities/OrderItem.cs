using System.Security.Principal;

namespace LocalDropshipping.Web.Data.Entities
{
    public class OrderItem
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public string Image { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public int Quantity { get; set; }
        public double SubTotal { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public DateTime UpdatedDate { get; set; }
        public string? UpdatedBy { get; set; }
        public string? CreatedBy { get; set; }
    }
}
