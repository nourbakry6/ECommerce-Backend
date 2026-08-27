using ECommerce.Application.Interface;
using ECommerce.Domain.entites;
using ECommerce.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerce.Infrastructure.Repositories
{

    public class CartRepository :ICartRepository
    {
        private readonly ApplicationDbContext _context;
        public CartRepository(ApplicationDbContext context) {
        _context = context;
        }

       public void ClearCart(IEnumerable<CartItem> cartItem)
        {
            

            _context.CartItems.RemoveRange(cartItem);

            _context.SaveChanges();
        }

        public Cart? GetByUserId(int userId)
        {
            return _context.Carts
                .Include(c => c.CartItems)
                .ThenInclude(ci => ci.Product)
                .FirstOrDefault(c => c.UserId == userId);
        }

        public CartItem? GetItemById(int id)
        {
            return _context.CartItems.Include(p=>p.Product).FirstOrDefault(p=>p.Id==id);
        }

        public void ItemAdd(CartItem cartItem)
        {
             _context.CartItems.Add(cartItem);
             _context.SaveChanges();

        }

        public void ItemDelete(CartItem cartItem)
        {
            _context.CartItems.Remove(cartItem);
            _context.SaveChanges();
        }

        public void ItemUpdate(CartItem cartItem)
        {
            _context.CartItems.Update(cartItem);
            _context.SaveChanges();
        }
    }
}
