using ECommerce.Application.Interface;
using ECommerce.Domain.entites;
using ECommerce.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerce.Infrastructure.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly ApplicationDbContext _Context;
        public OrderRepository(ApplicationDbContext applicationDbContext)
        {
            _Context = applicationDbContext;
        }

        public void AddOrder(Order order)
        {
            _Context.Orders.Add(order); ;
            _Context.SaveChanges();
        }

        public void CancelOrder(Order order)
        {
           _Context.Orders.Update(order); 
           _Context.SaveChanges();
        }

        public List<Order> GetAllOrder()
        {
            return _Context.Orders.Include(p => p.OrderItems).ThenInclude(p => p.Product).ToList();
        }

        public Order? GetByOrderId(int orderid)
        {
            return _Context.Orders.Include(p=>p.OrderItems).ThenInclude(p=>p.Product).FirstOrDefault(p => p.Id==orderid);
        }

        public List<Order> GetMyOrders(int userId)
        {
            return _Context.Orders
            .Include(p=>p.OrderItems).ThenInclude(p=>p.Product)
            .Where(p=>p.UserId==userId).ToList();
        }

        public void UpdateOrder(Order order)
        {
            _Context.Orders.Update(order);
            _Context.SaveChanges();
        }
    }
}
