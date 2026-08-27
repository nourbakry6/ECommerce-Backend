using ECommerce.Application.DTO;
using ECommerce.Domain.entites;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerce.Application.Interface
{
   public interface IOrderService

    {
        List<OrderDTO> GetAllORder();
        bool UpdateStatus(int  orderId,UpdateOrderStatusDTO updateOrderStatusDTO);
        OrderDTO? GetByOrderId(int Orderid,int userid);
        bool CancelOrder(int orderid,int userid);
        List<OrderDTO> GetMyOrders(int userId);
        bool Checkout(int UserId);


    }
}
