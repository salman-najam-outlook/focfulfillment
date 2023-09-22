using LocalDropshipping.Web.Data;
using LocalDropshipping.Web.Data.Entities;
using LocalDropshipping.Web.Dtos;
using LocalDropshipping.Web.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace LocalDropshipping.Web.Services
{
    public class OrderService : IOrderService
    {
        private readonly LocalDropshippingContext _context;
        private readonly IFocSettingService _focSettingService;

        public OrderService(LocalDropshippingContext context, IFocSettingService focSettingService)
        {
            _context = context;
            _focSettingService = focSettingService;
        }

		public Order AddOrder(List<OrderItem> cart, string email, decimal sellPrice)
		{
			
			Order order = new Order()
			{
				//OrderCode = abcd,
				Id = GenerateOrderId(),
				Seller = email,
				CreatedDate = DateTime.Now,
				CreatedBy = email,
				GrandTotal = Convert.ToDecimal(cart.Sum(s => s.Quantity * s.Price)),
				OrderStatus= OrderStatus.Pending,
				SellPrice=sellPrice
				
			};
			_context.Orders.Add(order);
			_context.SaveChanges();

            foreach (var items in cart)
            {
                var orderItems = new OrderItem()
                {
                    OrderId = order.Id,
                    ProductId = items.ProductId,
                    Image = items.Image,
                    Name = items.Name,
                    Price = items.Price,
                    Quantity = items.Quantity,
                    SubTotal = items.Quantity * items.Price,
                    CreatedDate = DateTime.Now,
                    CreatedBy = email
                };
                _context.OrderItems.Add(orderItems);
            }
            _context.SaveChanges();
            return order;

        }

        public Order? Delete(int orderId)
        {
            var order = _context.Orders.FirstOrDefault(x => x.Id == orderId);
            if (order != null)
            {
                order.IsDeleted = true;
                _context.SaveChanges();
            }
            return order;
        }

        public List<Order> GetAll()
        {
            var orderlist = new List<Order>();
            orderlist = _context.Orders.Where(x => x.IsDeleted == false).ToList();
            if (orderlist != null)
            {
                return orderlist;
            }
            return new List<Order>();

        }

        public Order? GetById(int orderId)
        {
            return _context.Orders.FirstOrDefault(x => x.Id == orderId);
        }

        public List<Order> GetByEmail(string userEmail)
        {
            return _context.Orders.Where(x => x.Seller == userEmail).ToList();
        }

        public Order? Update(int orderid, OrderDto order)
        {
            var exOrder = _context.Orders.FirstOrDefault(x => x.Id == orderid);
            if (exOrder != null)
            {
                // exOrder.Name = order.Name;
                exOrder.OrderStatus = order.Status;
                exOrder.SpecialInstructions = order.SpecialInstructions;
                //exOrder.OrderCode = order.OrderCode;
                exOrder.GrandTotal = order.GrandTotal;
                _context.SaveChanges();
            }
            return exOrder;
        }

        public List<Order> GetOrdersByStatus(OrderStatus status)
        {
            return _context.Orders.Where(x => x.OrderStatus == status).ToList();
        }

        public int CountOrdersByStatus(OrderStatus status)
        {
            return _context.Orders.Count(x => x.OrderStatus == status);
        }

        Order? IOrderService.Add(Order order)
        {
            throw new NotImplementedException();
        }
        public decimal? GetProfit(string email)
        {
            decimal profit = 0;
            decimal cost = 0;
            int totalPendingOrders = 0;
            var orders=_context.Orders.Where(o=>o.OrderStatus==OrderStatus.Pending && o.Seller==email).ToList();
            if (orders.Any())
            {
                profit = orders.Sum(o => o.SellPrice - o.GrandTotal - ShippingCost());
            }
            cost = _context.Orders.Where(o => o.OrderStatus == OrderStatus.Return && o.Seller == email).Sum(o => -ShippingCost());
            totalPendingOrders = _context.Orders.Count(o => o.OrderStatus == 0 && o.Seller == email);
            profit -= cost;
            return profit;
        }
        private int GenerateOrderId()
        {
			int orderId;
			do
			{
				Random random = new Random();
				orderId = random.Next(10000000, 99999999);
			}
			while (GetById(orderId)!= null ? true:false);

            return orderId;
        }
        private decimal ShippingCost()
        {
            string shippingCost = "Shipping Cost";
            return Convert.ToDecimal(_focSettingService.GetShippingCost(shippingCost));
        }
    }
}
