using ECommerce.Application.DTO;
using ECommerce.Application.Interface;
using ECommerce.Domain.entites;

namespace ECommerce.Application.Services
{
    public class CartService : ICartService
    {
        private readonly ICartRepository _cartRepository;
        private readonly IProductRepository _productRepository;
      private readonly IUnitOfWork _unitOfWork;

        public CartService(
            ICartRepository cartRepository,
            IProductRepository productRepository,
            IUnitOfWork unitOfWork)
        {
            _cartRepository = cartRepository;
            _productRepository = productRepository;
         _unitOfWork= unitOfWork;
        }

        public async Task<string?> AddItem(
     int userId,
     CartItemAddDTO cartItemAdd)
        {
            var cart = await _cartRepository.GetByUserId(userId);

            if (cart == null)
                return "Cart not found.";

            var product = await _productRepository.GetById(
                cartItemAdd.ProductId);

            if (product == null)
                return "Product not found.";

            var cartItem = new CartItem
            {
                CartId = cart.Id,
                ProductId = product.Id,
                Quantity = cartItemAdd.Quantity
            };

            await _cartRepository.ItemAdd(cartItem);
            await _unitOfWork.SaveChangesAsync();

            return null;
        }

        public async Task<bool> ClearCart(int userId)
        {
            var cart = await _cartRepository.GetByUserId(userId);

            if (cart == null)
                return false;

            await _cartRepository.ClearCart(cart.CartItems);

            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteItem(int itemId)
        {
            var item = await _cartRepository.GetItemById(itemId);

            if (item == null)
                return false;

            await _cartRepository.ItemDelete(item);

         

            return true;
        }

        public async Task<CartDTO?> GetByUserId(int userId)
        {
            var cart = await _cartRepository.GetByUserId(userId);

            if (cart == null)
                return null;

            return new CartDTO
            {
                Id = cart.Id,

                cartItems = cart.CartItems.Select(x => new CartItemDTO
                {
                    ID = x.Id,
                    ProductId = x.ProductId,
                    Quantity = x.Quantity,
                    ProductName = x.Product.Name,
                    ProductPrice = x.Product.Price,
                    IgammeUrl = x.Product.ImageUrl
                }).ToList()
            };
        }

        public async Task<bool> UpdateItem(
            int itemId,
            CartItemUpdate cartItemUpdate)
        {
            var item = await _cartRepository.GetItemById(itemId);

            if (item == null)
                return false;

            item.Quantity = cartItemUpdate.Quantity;

            await _cartRepository.ItemUpdate(item);

            

            return true;
        }
    }
}