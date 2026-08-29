using ECommerce.Application.DTO;
using ECommerce.Application.Interface;
using ECommerce.Domain.entites;
using ECommerce.Domain.enums;

namespace ECommerce.Application.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly ICartRepository _cartRepository;
        private readonly IProductRepository _productRepository;
        private readonly IUnitOfWork _unitOfWork;
        public OrderService(
       IOrderRepository orderRepository,
    ICartRepository cartRepository,
       IProductRepository productRepository,
       IUnitOfWork unitOfWork)
        {
            _orderRepository = orderRepository;
            _cartRepository= cartRepository;
            _productRepository = productRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> CancelOrder(int orderId, int userId)
        {
            var order = await _orderRepository.GetByOrderId(orderId);

            if (order == null)
                return false;

            if (order.UserId != userId)
                return false;

            if (order.Status != OrderStatus.Pending)
                return false;

            order.Status = OrderStatus.Cancelled;

            await _orderRepository.CancelOrder(order);

            return true;
        }
        public async Task<string?> Checkout(int userId)
        {
            var cart = await _cartRepository.GetByUserId(userId);

            if (cart == null)
                return "Cart not found.";

            if (cart.CartItems == null || !cart.CartItems.Any())
                return "Cart is empty.";

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

                foreach (var item in cart.CartItems)
                {
                    var product = await _productRepository.GetById(item.ProductId);

                    if (product == null)
                        return $"Product with ID {item.ProductId} not found.";

                    if (product.Stock < item.Quantity)
                        return $"Not enough stock for product: {product.Name}.";

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
                }

                order.TotalAmount = totalAmount;

                await _orderRepository.AddOrder(order);

                await _cartRepository.ClearCart(cart.CartItems);

                await _unitOfWork.SaveChangesAsync();

                await _unitOfWork.CommitTransactionAsync();

                return null;
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();

                return $"Checkout failed: {ex.Message}";
            }
        }


        public async Task<List<OrderDTO>> GetAllOrder()
        {
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

            return orderDto;
        }

        public async Task<OrderDTO?> GetByOrderId(int orderId, int userId)
        {
            var order = await _orderRepository.GetByOrderId(orderId);

            if (order == null)
                return null;

            if (order.UserId != userId)
                return null;

            return new OrderDTO
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
        }

        public async Task<List<OrderDTO>> GetMyOrders(int userId)
        {
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

            return orderDto;
        }

        public async Task<bool> UpdateStatus(
            int orderId,
            UpdateOrderStatusDTO updateOrderStatusDTO)
        {
            var order = await _orderRepository.GetByOrderId(orderId);

            if (order == null)
                return false;

            order.Status = updateOrderStatusDTO.status;

            await _orderRepository.UpdateOrder(order);

            return true;
        }
    }
}