using ECommerce.Application.DTO;
using ECommerce.Domain.entites;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerce.Application.Interface
{
    public interface ICartRepository
    {
        Task<Cart?> GetByUserId(int id);

        Task ItemUpdate(CartItem cartItem);

        Task ItemDelete(CartItem cartItem);

        Task ClearCart(IEnumerable<CartItem> cartItems);

        Task ItemAdd(CartItem cartItem);

        Task<CartItem?> GetItemById(int id);

    }
}
