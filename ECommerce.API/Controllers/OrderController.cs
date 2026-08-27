using ECommerce.Application.DTO;
using ECommerce.Application.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
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
            {
                _orderService = orderService;
            }
        }
        public int getuserid()
        {
            return int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        }

        [HttpPost("chechkoout")]
        public IActionResult Chechkout()
        {
            var userid = getuserid();
            var order = _orderService.Checkout(userid);
            if (!order) return BadRequest("no item");
            return Ok(order);
        }

        [HttpGet("my-orders")]
        public IActionResult GetMyOrder()
        {
            var userid = getuserid();
            var order = _orderService.GetMyOrders(userid);
            return Ok(order);
        }
        [HttpGet("order{orderid}")]
        public IActionResult GetOrders(int orderid)
        {
            var userid = getuserid();
            var order = _orderService.GetByOrderId(orderid,userid);
            if (order == null) return BadRequest();
            return Ok(order);

        }
        [Authorize(Roles = "Admin")]
        [HttpPatch("{orderid}/status")]
        public IActionResult UpdateOrderStatus(int orderid, UpdateOrderStatusDTO updateOrderStatusDTO)
        {
            
                var userid = getuserid();
                var order = _orderService.UpdateStatus(orderid, updateOrderStatusDTO);
                if (!order) return BadRequest();
                return Ok("Order status updated successfully");
            
        }
        [HttpPatch("order{orderid}/cancelled")]
        public IActionResult CancelOrder(int orderid){
        var userid = getuserid();
        var order =_orderService.CancelOrder(orderid, userid);
            if (order == false) return BadRequest("you can not cancelled the order!");
            else return Ok("the order is cancelled");
        }
        [Authorize(Roles="Admin")]
        [HttpGet("Get-All-Order")]
        public IActionResult GetAllOrder(){
            var order = _orderService.GetAllORder();
            return Ok(order);
        }
    }
}
