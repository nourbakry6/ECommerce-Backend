using ECommerce.Application.DTO;
using ECommerce.Application.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ECommerce.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        private int GetUserId()
        {
            return int.Parse(
                User.FindFirst(ClaimTypes.NameIdentifier)!.Value
            );
        }

        [HttpPost("checkout")]
        public async Task<IActionResult> Checkout()
        {
            var userId = GetUserId();

            var result = await _orderService.Checkout(userId);

            

            return Ok("Order created successfully.");
        }

        [HttpGet("my-orders")]
        public async Task<IActionResult> GetMyOrder()
        {
            var userId = GetUserId();

            var orders = await _orderService.GetMyOrders(userId);

            return Ok(orders);
        }

        [HttpGet("order/{orderid}")]
        public async Task<IActionResult> GetOrder(int orderid)
        {
            var userId = GetUserId();

            var order = await _orderService.GetByOrderId(
                orderid,
                userId
            );

            

            return Ok(order);
        }

        [Authorize(Roles = "Admin")]
        [HttpPatch("{orderid}/status")]
        public async Task<IActionResult> UpdateOrderStatus(
            int orderid,
            UpdateOrderStatusDTO updateOrderStatusDTO)
        {
            var result = await _orderService.UpdateStatus(
                orderid,
                updateOrderStatusDTO
            );

          

            return Ok("Order status updated successfully.");
        }

        [HttpPatch("order/{orderid}/cancelled")]
        public async Task<IActionResult> CancelOrder(int orderid)
        {
            var userId = GetUserId();

            var result = await _orderService.CancelOrder(
                orderid,
                userId
            );

        

            return Ok("The order is cancelled.");
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("all")]
        public async Task<IActionResult> GetAllOrder()
        {
            var orders = await _orderService.GetAllOrder();

            return Ok(orders);
        }
    }
}