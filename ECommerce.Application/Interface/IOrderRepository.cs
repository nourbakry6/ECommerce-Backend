using ECommerce.Application.DTO;
using ECommerce.Domain.entites;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerce.Application.Interface
{
   public interface IOrderRepository
    {
        List<Order> GetAllOrder();
        Order? GetByOrderId(int user);
        void CancelOrder(Order order);
        List<Order> GetMyOrders(int userId);
        void AddOrder(Order order);
        void UpdateOrder(Order order);
    }
}
