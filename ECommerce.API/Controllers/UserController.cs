using ECommerce.Application.DTO;
using ECommerce.Application.Interface;
using ECommerce.Application.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    
    public class UserController : ControllerBase
    {
    private readonly IUserServercs _userServercs;
    public UserController(IUserServercs userServercs){
    _userServercs = userServercs;
    }
    [HttpGet]
    public async Task< IActionResult> GetAll(){
            var user =await _userServercs.GetAll();
            return Ok(user);
    }
        

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDTO dto)
        {
            var result = await _userServercs.Register(dto);

            if (!result)
                return BadRequest("Registration failed.");

            return Ok("Registered successfully.");
        }

        [HttpPost("login")]
        public async Task< IActionResult> Login(LoginDTO dto)
        {
            var result = await _userServercs.Login(dto);
            if (!result.Success) return BadRequest(result.Error);
            return Ok(new
            {
                token = result.Token


            });


          
        }

  
        [HttpGet("user{id}")]
        public async Task< IActionResult> GetById(int id) {
            var user = await _userServercs.GetById(id);
            if (user == null) return NotFound();
            return Ok(user);
        
        }
        [HttpDelete("delete/{id}")]
        public async Task< IActionResult> Delete(int id){
            var user =await _userServercs.Delete(id);
            if(user==false)return NotFound();
            return Ok(user);
        }
        [HttpPut("update/{id}")]
        public async Task< IActionResult> Update(int id , UserUpdateDTO User){
            var user =await _userServercs.Update(id, User);
            if(user==false)return NotFound();
            return Ok(user);

        }

    
    }
}
