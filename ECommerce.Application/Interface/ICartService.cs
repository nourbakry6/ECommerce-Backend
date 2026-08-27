using ECommerce.Application.DTO;
using ECommerce.Domain.entites;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerce.Application.Interface
{
    public interface ICartService
    {
        bool ClearCart(int userid);
    CartDTO? GetByUserId(int userId);
     bool UpdateItem(int itemid,CartItemUpdate cartItemUpdate);
     bool DeleteItem(int itemid);
     void AddItem(int userid,CartItemAddDTO cartItemAdd);
    

    }
}
