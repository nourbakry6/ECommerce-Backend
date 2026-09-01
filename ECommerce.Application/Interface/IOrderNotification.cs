using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerce.Application.Interface
{
   public interface IOrderNotification
    {
        Task SendOrderStatusUpdate(
                int userId,
                int orderId,
                string status);
    }
}
