using ECommerce.Application.DTO;
using ECommerce.Application.Interface;
using ECommerce.Domain.entites;
using ECommerce.Domain.enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerce.Application.Services
{
    public class OrderService : IOrderService
    {
    private readonly IOrderRepository _orderRepository;
        private readonly ICartService _cartService1;
        private readonly IProductRepository _productService;
    public OrderService(IOrderRepository orderRepository,ICartService cart,IProductRepository productService)
        {
            _orderRepository = orderRepository;
           _cartService1 = cart;
            _productService = productService;
          
        }

        public bool CancelOrder(int orderId, int userId)
        {
            var order = _orderRepository.GetByOrderId(orderId);

            if (order == null)
                return false;

            if (order.UserId != userId)
                return false;

            if (order.Status != OrderStatus.Pending)
                return false;

            order.Status = OrderStatus.Cancelled;

            _orderRepository.CancelOrder(order);

            return true;
        }

        public bool Checkout(int UserId)
        {
            var cart = _cartService1.GetByUserId(UserId);
            if (cart == null || cart.cartItems == null) return false;
            if (!cart.cartItems.Any()) return false;
            decimal totalamount = 0;
            var Order = new Order
            {
                UserId = UserId,
                
                OrderDate = DateTime.Now,
                OrderItems = new List<OrderItem>()

            };
            foreach(var item in cart.cartItems){
           var  product=_productService.GetById(item.ProductId);
            if(product == null) return false;
                if (product.Stock < item.Quantity) return false;
                var price = product.Price;
                var orderitem = new OrderItem
                {
                    ProductId = product.Id,
                    Price = price,
                    Quantity = item.Quantity,
                };
               Order.OrderItems.Add(orderitem);
               totalamount+=price*item.Quantity;
                product.Stock -= item.Quantity;
            }
            Order.TotalAmount = totalamount;
            _orderRepository.AddOrder(Order);
            _cartService1.ClearCart(UserId);
            return true;

        }

        public List<OrderDTO> GetAllORder()
        {
            var order = _orderRepository.GetAllOrder();
            var orderdto = order.Select(p => new OrderDTO
            {
                Id = p.Id,
                TotalAmount = p.TotalAmount,
                OrderDate = p.OrderDate,
                OrderItems = p.OrderItems.Select(o => new OrderItemDTO
                {
                    ProductId = o.ProductId,
                    ProdactName = o.Product.Name,
                    Price = o.Price


                }).ToList()

            }).ToList();
            return orderdto;

        }

        public OrderDTO? GetByOrderId(int orderid,int UserId)
        {
            var orderitem = _orderRepository.GetByOrderId(orderid);
            if (orderitem == null)
                return null;
            if (orderitem.UserId != UserId) return null;
            return new OrderDTO
            {
                Status=orderitem.Status,
                Id = orderitem.Id,
                OrderDate = orderitem.OrderDate,
                TotalAmount = orderitem.TotalAmount,
                OrderItems = orderitem.OrderItems.Select(p => new OrderItemDTO
                {
                    Id = p.Id,
                    ProductId = p.ProductId,
                    ProdactName = p.Product.Name,
                    Quantity = p.Quantity,
                    Price = p.Price,

                }).ToList()


            };

        }

        public List<OrderDTO> GetMyOrders(int userId)
        {
       

        var orderlist=_orderRepository.GetMyOrders(userId);
            var orderdto = orderlist.Select(p => new OrderDTO {
                Id = p.Id,
                OrderDate = p.OrderDate,
                Status =p.Status,
                TotalAmount = p.TotalAmount,
                OrderItems = p.OrderItems.Select(p => new OrderItemDTO {
                    ProductId = p.ProductId,
                    Id = p.Id,
                    ProdactName = p.Product.Name,
                    Quantity = p.Quantity,
                    Price = p.Price,
                }).ToList()
            }).ToList();
            return orderdto;
        }

        public bool UpdateStatus(int orderId, UpdateOrderStatusDTO updateOrderStatusDTO)
        {
            var order=_orderRepository.GetByOrderId(orderId);
            if (order == null) return false;
           
            order.Status = updateOrderStatusDTO.status;
            _orderRepository.UpdateOrder(order);
            return true;

        }
    }
}
