using LocalDropshipping.Web.Data;
using LocalDropshipping.Web.Data.Entities;
using LocalDropshipping.Web.Dtos;
using LocalDropshipping.Web.Enums;
using Microsoft.EntityFrameworkCore;

namespace LocalDropshipping.Web.Services
{
	public class OrderService : IOrderService
	{
		private readonly LocalDropshippingContext _context;

		public OrderService(LocalDropshippingContext context)
		{
			_context = context;
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

        //Order? IOrderService.AddOrder(List<OrderItem> cart, string email)
        //{
        //    throw new NotImplementedException();
        //}

        Order? IOrderService.Add(Order order)
        {
            throw new NotImplementedException();
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
    }
}
