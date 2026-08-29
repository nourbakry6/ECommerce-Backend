using ECommerce.Application.DTO;
using ECommerce.Domain.entites;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerce.Application.Interface
{
   public interface IOrderService

    {
        Task<List<OrderDTO>> GetAllOrder();
        Task<bool> UpdateStatus(int orderId, UpdateOrderStatusDTO updateOrderStatusDTO);
        Task<OrderDTO?> GetByOrderId(int orderId, int userId);
        Task<bool> CancelOrder(int orderId, int userId);
        Task<List<OrderDTO>> GetMyOrders(int userId);
        Task<bool> Checkout(int userId);


    }
}
