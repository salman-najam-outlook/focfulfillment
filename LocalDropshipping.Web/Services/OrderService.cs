using LocalDropshipping.Web.Data;
using LocalDropshipping.Web.Data.Entities;
using LocalDropshipping.Web.Dtos;
using LocalDropshipping.Web.Enums;
using Microsoft.EntityFrameworkCore;

namespace LocalDropshipping.Web.Services
{
    public class OrderService : IOrderService
    {
        private readonly LocalDropshippingContext context;

        public OrderService(LocalDropshippingContext context)
        {
            this.context = context;
        }

        public Order Add(Order order)
        {
            context.Order.Add(order);
            context.SaveChanges();

            return order;
        }

        public Order? Delete(int orderId)
        {
            var order = context.Order.FirstOrDefault(x => x.Id == orderId);
            if (order != null)
            {
                order.IsDeleted = true;
                context.SaveChanges();
            }
            return order;
        }

        public List<Order> GetAll()
        {
            return context.Order.Where(x => x.IsDeleted == true).ToList();
        }

        public Order? GetById(int orderId)
        {
            return context.Order.FirstOrDefault(x => x.Id == orderId);
        }

        public Order? Update(int orderid, OrderDto order)
        {
            var exOrder = context.Order.FirstOrDefault(x => x.Id == orderid);
            if (exOrder != null)
            {
                exOrder.Name = order.Name;
                exOrder.OrderStatus = order.Status;
                exOrder.SpecialInstructions = order.SpecialInstructions;
                exOrder.OrderCode = order.OrderCode;
                exOrder.GrandTotal = order.GrandTotal;
                context.SaveChanges();
            }
            return exOrder;
        }

        public List<Order> GetOrdersByStatus(OrderStatus status)
        {
            return context.Order.Where(x => x.OrderStatus == status).ToList();
        }

        public int CountOrdersByStatus(OrderStatus status)
        {
            return context.Order.Count(x => x.OrderStatus == status);
        }
    }
}
