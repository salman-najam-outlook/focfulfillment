using LocalDropshipping.Web.Data.Entities;
using LocalDropshipping.Web.Dtos;

namespace LocalDropshipping.Web.Services
{
    public interface IOrderService
    {
        Order? Add(Order order);
        List<Order?> GetAll();
        Order? GetById(int orderId);
        Order? Delete(int orderId);
        Order? Update(int orderid, OrderDto order);
    }
}
