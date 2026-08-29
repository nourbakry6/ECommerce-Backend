using ECommerce.Application.DTO;
using ECommerce.Domain.entites;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerce.Application.Interface
{
    public interface ICartService
    {
        Task<bool> ClearCart(int userId);

        Task<CartDTO?> GetByUserId(int userId);

        Task<bool> UpdateItem(int itemId, CartItemUpdate cartItemUpdate);

        Task<bool> DeleteItem(int itemId);

        Task <string?>AddItem(int userId, CartItemAddDTO cartItemAdd);

    }
}
