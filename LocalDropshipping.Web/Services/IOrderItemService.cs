using LocalDropshipping.Web.Data.Entities;

namespace LocalDropshipping.Web.Services
{
    public interface IOrderItemService
    {
        List<OrderItem> GetOrderItemsById(int orderId);

    }
}

