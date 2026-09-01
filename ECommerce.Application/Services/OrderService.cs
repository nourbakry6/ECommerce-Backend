using ECommerce.Application.DTO;
using ECommerce.Application.Interface;
using ECommerce.Domain.entites;
using ECommerce.Domain.enums;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;
namespace ECommerce.Application.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly ICartRepository _cartRepository;
        private readonly IProductRepository _productRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IDistributedCache _cache;
        private readonly IOrderNotification _orderNotification;
        private readonly IStockNotification _stock;
        public OrderService(
       IOrderRepository orderRepository,
    ICartRepository cartRepository,
       IProductRepository productRepository,
       IUnitOfWork unitOfWork,
       IDistributedCache distributedCache,IOrderNotification orderNotification,
       IStockNotification stock)
        {
            _orderRepository = orderRepository;
            _cartRepository= cartRepository;
            _productRepository = productRepository;
            _unitOfWork = unitOfWork;
            _cache = distributedCache;
            _orderNotification = orderNotification;
            _stock = stock;
        }

        public async Task<bool> CancelOrder(int orderId, int userId)
        {
            var order = await _orderRepository.GetByOrderId(orderId);

            if (order == null)
                throw new KeyNotFoundException(
                    $"Order with ID {orderId} not found.");

            if (order.UserId != userId)
                throw new UnauthorizedAccessException(
                    "You are not allowed to cancel this order.");

            if (order.Status != OrderStatus.Pending)
                throw new InvalidOperationException(
                    "Only pending orders can be cancelled.");


            order.Status = OrderStatus.Cancelled;

            await _orderRepository.UpdateOrder(order);
            await _cache.RemoveAsync($"order:{orderId}:user:{userId}");
            await _cache.RemoveAsync($"order:user:{userId}");
            await _cache.RemoveAsync("order");
            return true;
        }
        public async Task<bool> Checkout(int userId)
        {
            var cart = await _cartRepository.GetByUserId(userId);

            if (cart == null)
                throw new KeyNotFoundException("Cart not found.");

            if (cart.CartItems == null || !cart.CartItems.Any())
                throw new InvalidOperationException("Cart is empty.");

            await _unitOfWork.BeginTransactionAsync();
           
            try
            {
                decimal totalAmount = 0;

                var order = new Order
                {
                    UserId = userId,
                    OrderDate = DateTime.Now,
                    OrderItems = new List<OrderItem>()
                };
                var productIds = new List<int>();
                var stockUpdates = new List<(int ProductId, int Stock)>();
                foreach (var item in cart.CartItems)
                {
                    var product = await _productRepository.GetById(item.ProductId);
                  
                    if (product == null)
                        throw new KeyNotFoundException(
                            $"Product with ID {item.ProductId} not found.");

                    if (product.Stock < item.Quantity)
                        throw new InvalidOperationException(
                            $"Not enough stock for product: {product.Name}.");

                    
                    var orderItem = new OrderItem
                    {
                        ProductId = product.Id,
                        Price = product.Price,
                        Quantity = item.Quantity
                    };

                    order.OrderItems.Add(orderItem);

                    totalAmount += product.Price * item.Quantity;

                    product.Stock -= item.Quantity;

                    await _productRepository.Update(product);
                  
                    productIds.Add(product.Id);
                    stockUpdates.Add((product.Id, product.Stock));
                }

             
                order.TotalAmount = totalAmount;

                await _orderRepository.AddOrder(order);

                await _cartRepository.ClearCart(cart.CartItems);

                await _unitOfWork.SaveChangesAsync();

                await _unitOfWork.CommitTransactionAsync();

                foreach(var productid in productIds){
                    await _cache.RemoveAsync($"product:{productid}");
                }
                foreach(var update in stockUpdates){
                    await _stock.SendStockUpdate(update.ProductId, update.Stock);
                }
                await _cache.RemoveAsync($"order:user:{userId}");
                await _cache.RemoveAsync("order");
                await _cache.RemoveAsync($"cart:user:{userId}");
                await _cache.RemoveAsync("products");
               
                return true;
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();

                throw;
            }
        }


        public async Task<List<OrderDTO>> GetAllOrder()
        {
            var cachekey = "order";
            var cachedorder = await _cache.GetStringAsync(cachekey);
            if(cachedorder != null){
                return JsonSerializer.Deserialize<List<OrderDTO>>(cachedorder)!;
            }

            var orders = await _orderRepository.GetAllOrder();

            var orderDto = orders.Select(p => new OrderDTO
            {
                Id = p.Id,
                TotalAmount = p.TotalAmount,
                OrderDate = p.OrderDate,
                Status = p.Status,

                OrderItems = p.OrderItems.Select(o => new OrderItemDTO
                {
                    ProductId = o.ProductId,
                    ProdactName = o.Product.Name,
                    Price = o.Price,
                    Quantity = o.Quantity
                }).ToList()

            }).ToList();

            var option = new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10)
            };
            await _cache.SetStringAsync(
            cachekey, JsonSerializer.Serialize(orderDto), option
            );


            return orderDto;
        }

        public async Task<OrderDTO?> GetByOrderId(int orderId, int userId)
        {
            var cachekey = $"order:{orderId}:user:{userId}";
            var cachedorder= await _cache.GetStringAsync(cachekey);
            if (cachedorder != null) {
                return JsonSerializer.Deserialize<OrderDTO>(cachedorder);
            }
            var order = await _orderRepository.GetByOrderId(orderId);
            if (order == null)
                throw new KeyNotFoundException(
                    $"Order with ID {orderId} not found.");

            if (order.UserId != userId)
                throw new UnauthorizedAccessException(
                    "You are not allowed to access this order.");

            var orderDto= new OrderDTO
            {
                Status = order.Status,
                Id = order.Id,
                OrderDate = order.OrderDate,
                TotalAmount = order.TotalAmount,

                OrderItems = order.OrderItems.Select(p => new OrderItemDTO
                {
                    Id = p.Id,
                    ProductId = p.ProductId,
                    ProdactName = p.Product.Name,
                    Quantity = p.Quantity,
                    Price = p.Price
                }).ToList()
            };

            var option = new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10)
            };
            await _cache.SetStringAsync(
            cachekey, JsonSerializer.Serialize(orderDto), option
            );
            return orderDto;
        }

        public async Task<List<OrderDTO>> GetMyOrders(int userId)
        {
        var cachekey=$"order:user:{userId}";
            var cachedorder = await _cache.GetStringAsync(cachekey);
            if(cachedorder!=null){
                return JsonSerializer.Deserialize<List<OrderDTO>>(cachedorder)!;
            }
            var orderList = await _orderRepository.GetMyOrders(userId);

            var orderDto = orderList.Select(p => new OrderDTO
            {
                Id = p.Id,
                OrderDate = p.OrderDate,
                Status = p.Status,
                TotalAmount = p.TotalAmount,

                OrderItems = p.OrderItems.Select(item => new OrderItemDTO
                {
                    ProductId = item.ProductId,
                    Id = item.Id,
                    ProdactName = item.Product.Name,
                    Quantity = item.Quantity,
                    Price = item.Price
                }).ToList()

            }).ToList();
            var option = new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10)
            };
            await _cache.SetStringAsync(
            cachekey, JsonSerializer.Serialize(orderDto), option
            );
            return orderDto;
        }

        public async Task<bool> UpdateStatus(
            int orderId,
            UpdateOrderStatusDTO updateOrderStatusDTO)
        {
            var order = await _orderRepository.GetByOrderId(orderId);

            if (order == null)
                throw new KeyNotFoundException($"Order with ID {orderId} not found.");

            order.Status = updateOrderStatusDTO.status;
            var userid = order.UserId;

            await _orderRepository.UpdateOrder(order);
            await _orderNotification.SendOrderStatusUpdate(
    order.UserId,
    order.Id,
    order.Status.ToString());
            await _cache.RemoveAsync($"order:{orderId}:user:{userid}");
            await _cache.RemoveAsync($"order:user:{userid}");
            await _cache.RemoveAsync("order");
            return true;
        }
    }
}