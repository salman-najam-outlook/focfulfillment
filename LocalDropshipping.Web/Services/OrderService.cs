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
            context.Orders.Add(order);
            context.SaveChanges();

            return order;
        }

        public Order? Delete(int orderId)
        {
            var order = context.Orders.FirstOrDefault(x => x.Id == orderId);
            if (order != null)
            {
                order.IsDeleted = true;
                context.SaveChanges();
            }
            return order;
        }

        public List<Order> GetAll()
        {
            var orderlist= new List<Order>();
               orderlist = context.Orders.Where(x => x.IsDeleted == false).ToList();
            if (orderlist != null)
            {
                return orderlist;
            }
            return new List<Order>();
           
        }

        public Order? GetById(int orderId)
        {
            return context.Orders.FirstOrDefault(x => x.Id == orderId);
        }

        public Order? Update(int orderid, OrderDto order)
        {
            var exOrder = context.Orders.FirstOrDefault(x => x.Id == orderid);
            if (exOrder != null)
            {
               // exOrder.Name = order.Name;
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
            return context.Orders.Where(x => x.OrderStatus == status).ToList();
        }

        public int CountOrdersByStatus(OrderStatus status)
        {
            return context.Orders.Count(x => x.OrderStatus == status);
        }
    }
}
