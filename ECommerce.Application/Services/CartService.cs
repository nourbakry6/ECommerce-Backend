using ECommerce.Application.DTO;
using ECommerce.Application.Interface;
using ECommerce.Domain.entites;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace ECommerce.Application.Services
{
    public class CartService : ICartService
    {
        private readonly ICartRepository _cartRepository;
        private readonly IProductRepository _productRepository;
      private readonly IUnitOfWork _unitOfWork;
        private readonly IDistributedCache _cache;

        public CartService(
            ICartRepository cartRepository,
            IProductRepository productRepository,
            IUnitOfWork unitOfWork,
            IDistributedCache distributedCache)
        {
            _cartRepository = cartRepository;
            _productRepository = productRepository;
         _unitOfWork= unitOfWork;
         _cache= distributedCache;
        } 
        //hn redis msh mtl poduct  wcategory lkl ha ybyn endn nfs chi la hn ha tkun hsab userid

        public async Task<string?> AddItem(
     int userId,
     CartItemAddDTO cartItemAdd)
        {
            var cart = await _cartRepository.GetByUserId(userId);


            if (cart == null)
                throw new KeyNotFoundException(
                    $"Cart for user with ID {userId} not found.");

            var product = await _productRepository.GetById(
                cartItemAdd.ProductId);

            if (product == null)
                throw new KeyNotFoundException(
                    $"Product with ID {cartItemAdd.ProductId} not found.");

            var cartItem = new CartItem
            {
                CartId = cart.Id,
                ProductId = product.Id,
                Quantity = cartItemAdd.Quantity
            };

            await _cartRepository.ItemAdd(cartItem);
            
            await _cache.RemoveAsync($"cart:user:{userId}");
            return null;
        }

        public async Task<bool> ClearCart(int userId)
        {
            var cart = await _cartRepository.GetByUserId(userId);

            if (cart == null)
                throw new KeyNotFoundException(
                $"Cart for user with ID {userId} not found.");

            await _cartRepository.ClearCart(cart.CartItems);

            await _unitOfWork.SaveChangesAsync();
            await _cache.RemoveAsync($"cart:user:{userId}");

            return true;
        }

        public async Task<bool> DeleteItem(int itemId)
        {
            var item = await _cartRepository.GetItemById(itemId);

            
                if (item == null)
                    throw new KeyNotFoundException(
                        $"Cart item with ID {itemId} not found.");
var userid=await _cartRepository.GetUserIdByItemId(itemId);

            if (userid == null)
                throw new KeyNotFoundException(
                    $"User for cart item with ID {itemId} not found.");

        

            await _cartRepository.ItemDelete(item);
            await _cache.RemoveAsync($"cart:user:{userid}");




            return true;
        }

        public async Task<CartDTO?> GetByUserId(int userId)
        {
            var cacheKey = $"cart:user:{userId}";

            // 1. Check Redis
            var cachedCart = await _cache.GetStringAsync(cacheKey);

            if (cachedCart != null)
            {
                return JsonSerializer.Deserialize<CartDTO>(cachedCart)!;
            }

            // 2. If not in Redis → SQL
            var cart = await _cartRepository.GetByUserId(userId);

            if (cart == null)
                throw new KeyNotFoundException(
                    $"Cart for user with ID {userId} not found.");

            // 3. Convert Entity → DTO
            var cartDto = new CartDTO
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

            // 4. Save DTO in Redis
            var option = new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10)
            };

            await _cache.SetStringAsync(
                cacheKey,
                JsonSerializer.Serialize(cartDto),
                option
            );

            return cartDto;
        }

        public async Task<bool> UpdateItem(
            int itemId,
            CartItemUpdate cartItemUpdate)
        {
            var item = await _cartRepository.GetItemById(itemId);

            if (item == null)
                throw new KeyNotFoundException(
                      $"Cart item with ID {itemId} not found.");

                      var userid= await _cartRepository.GetUserIdByItemId(itemId);
            if (userid == null)
                throw new KeyNotFoundException(
                    $"User for cart item with ID {itemId} not found.");
            item.Quantity = cartItemUpdate.Quantity;

            await _cartRepository.ItemUpdate(item);
            await _cache.RemoveAsync($"cart:user:{userid}");
            

            return true;
        }
    }
}