using ECommerce.Application.DTO;
using ECommerce.Domain.entites;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerce.Application.Interface
{
    public interface ICartRepository
    {
        Cart? GetByUserId(int id);
        void ItemUpdate(CartItem cartItem);
        void ItemDelete(CartItem cartItem);
        void ClearCart(IEnumerable<CartItem> cartItem);
        void ItemAdd(CartItem cartItem);
        CartItem? GetItemById(int id);

    }
}
