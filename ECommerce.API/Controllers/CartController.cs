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
   
    //kml usal lal taken
   
    public class CartController : ControllerBase
    {
        private readonly ICartService _cartService;
        public CartController(ICartService cartService) {
        
        _cartService = cartService;
        }

        private int getuserid(){
            return int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        }
        [HttpGet]
       public IActionResult GetByUserId(){
       var userid=getuserid();
       var cart= _cartService.GetByUserId(userid);
       return Ok(cart);
       }


       [HttpPost("items")]
       public IActionResult AddItem(CartItemAddDTO cartItem){
            var userId = getuserid();

            _cartService.AddItem(userId, cartItem);

            return Ok(cartItem);
        } 


       [HttpPut("items/{itemid}")]
       public IActionResult Updateitem(int itemid,CartItemUpdate cartItem){
       var item=_cartService.UpdateItem(itemid,cartItem);
            if (!item) return NotFound();
            return Ok(item);
       }


       [HttpDelete("items/{itemid}")]
       public IActionResult DeleteItem(int itemid){
       var item=_cartService.DeleteItem(itemid);
            if (!item) return NotFound();
            return Ok(item);

        }
        [HttpDelete("clearcart")]
        public IActionResult ClearCart()
        {
            var user = getuserid();
            var cart = _cartService.ClearCart(user);
            if (!cart) return NotFound("Cart not found.");

            return Ok("Cart cleared successfully.");
        }

        }
}
