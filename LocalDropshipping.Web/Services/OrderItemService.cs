using LocalDropshipping.Web.Data;
using LocalDropshipping.Web.Data.Entities;

namespace LocalDropshipping.Web.Services
{
    public class OrderItemService : IOrderItemService
    {
        private readonly LocalDropshippingContext _context;

        public OrderItemService(LocalDropshippingContext context)
        {
            _context = context;
        }
        public List<OrderItem> GetOrderItemsById(int orderId)
        {
            var orderItems = _context.OrderItems.Where(x => x.OrderId == orderId).ToList();
            return orderItems;
        }
    }
}
