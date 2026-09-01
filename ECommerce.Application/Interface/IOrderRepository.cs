using ECommerce.Application.DTO;
using ECommerce.Domain.entites;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerce.Application.Interface
{
   public interface IOrderRepository
    {
        Task<List<Order>> GetAllOrder();

        Task<Order?> GetByOrderId(int orderId);

        Task<List<Order>> GetMyOrders(int userId);

        Task AddOrder(Order order);

        Task UpdateOrder(Order order);

        
    }
}
