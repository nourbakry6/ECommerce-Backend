using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerce.Application.Interface
{
  public interface IStockNotification
    {

        Task SendStockUpdate(int productId, int stock);
    }
}
