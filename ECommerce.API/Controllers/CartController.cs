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
    public class CartController : ControllerBase
    {
        private readonly ICartService _cartService;

        public CartController(ICartService cartService)
        {
            _cartService = cartService;
        }

        private int GetUserId()
        {
            return int.Parse(
                User.FindFirst(ClaimTypes.NameIdentifier)!.Value
            );
        }

        [HttpGet]
        public async Task<IActionResult> GetByUserId()
        {
            var userId = GetUserId();

            var cart = await _cartService.GetByUserId(userId);

        

            return Ok(cart);
        }

        [HttpPost("items")]
        public async Task<IActionResult> AddItem(
            CartItemAddDTO cartItem)
        {
            var userId = GetUserId();

          var error=  await _cartService.AddItem(userId, cartItem);
         

            return Ok(cartItem);
        }

        [HttpPut("items/{itemId}")]
        public async Task<IActionResult> UpdateItem(
            int itemId,
            CartItemUpdate cartItem)
        {
            var result = await _cartService.UpdateItem(
                itemId,
                cartItem);

          

            return Ok();
        }

        [HttpDelete("items/{itemId}")]
        public async Task<IActionResult> DeleteItem(int itemId)
        {
            var result = await _cartService.DeleteItem(itemId);

            
            return Ok();
        }

        [HttpDelete("clearcart")]
        public async Task<IActionResult> ClearCart()
        {
            var userId = GetUserId();

            var result = await _cartService.ClearCart(userId);

         

            return Ok("Cart cleared successfully.");
        }
    }
}