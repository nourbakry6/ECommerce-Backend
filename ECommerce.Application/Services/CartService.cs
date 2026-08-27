using ECommerce.Application.DTO;
using ECommerce.Application.Interface;
using ECommerce.Domain.entites;
using System;
using System.Collections.Generic;
using System.Text;


namespace ECommerce.Application.Services
{
    public class CartService : ICartService
    {
        private readonly ICartRepository _cartRepository;
        private readonly IProductRepository productRepository;
        public CartService(ICartRepository cartRepository, IProductRepository productRepository )
        {
            _cartRepository = cartRepository;
            this.productRepository = productRepository;
        }

        public void AddItem(int userid, CartItemAddDTO cartItemAdd)
        {
            var item =_cartRepository.GetByUserId(userid);

            if (item == null)
                throw new Exception("Cart not found");

            var product = productRepository.GetById(cartItemAdd.ProductId);
            if (product == null)
                throw new Exception("Product not found");
            var cartitem = new CartItem { 
                CartId=item.Id,
                ProductId=cartItemAdd.ProductId,
               
                Quantity = cartItemAdd.Quantity,
            };
            _cartRepository.ItemAdd(cartitem);


        }

        public bool ClearCart(int userid)
        {
            var cart= _cartRepository.GetByUserId(userid);
            if (cart == null) return false;
            _cartRepository.ClearCart(cart.CartItems);
            return true;

        }

        public bool DeleteItem(int itemid)
        {
           var item=_cartRepository.GetItemById(itemid);
            if (item == null) return false;
            _cartRepository.ItemDelete(item);
            return true;
        }

        public CartDTO? GetByUserId(int userId)
        {
            var item = _cartRepository.GetByUserId(userId);
            if (item == null) return null;
            return new CartDTO
            {
                Id = item.Id,

                cartItems = item.CartItems.Select(x => new CartItemDTO
                {   ID= x.Id,
                    ProductId = x.ProductId,
                    Quantity = x.Quantity,
                    ProductName = x.Product.Name,
                    ProductPrice = x.Product.Price,
                    IgammeUrl = x.Product.ImageUrl
                }).ToList()
            };

        }

        public bool UpdateItem(int itemid, CartItemUpdate cartItemUpdate)
        {
            var item = _cartRepository.GetItemById(itemid);
            if (item == null) return false;
            item.Quantity = cartItemUpdate.Quantity;
            _cartRepository.ItemUpdate(item);
            return true;
        }
    }
}
