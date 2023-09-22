﻿using LocalDropshipping.Web.Data.Entities;
using LocalDropshipping.Web.Dtos;
using LocalDropshipping.Web.Models;

namespace LocalDropshipping.Web.Services
{
    public interface IOrderService
    {
        Order? AddOrder(List<OrderItem> cart, string email, decimal sellPrice);
        Order? Add(Order order);
        List<Order?> GetAll();
        Order? GetById(int orderId);
        List<Order> GetByEmail(string sellerEMail);
        Order? Delete(int orderId);
        Order? Update(int orderid, OrderDto order);
        Order UpdateOrder(OrderViewModel orderViewModel);
    }
}
