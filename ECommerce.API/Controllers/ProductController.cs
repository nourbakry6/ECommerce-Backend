using ECommerce.Application.DTO;
using ECommerce.Application.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
namespace ECommerce.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    
    public class ProductController : ControllerBase {
        private readonly IProductService _productService;
        public ProductController(IProductService productService) {
            _productService = productService;
        }

        [HttpGet]
        //iactionresult noe imeh l ha tnrak bdl ma ht list aw productt  laen mch druri ha ynrd hk
        public async Task<IActionResult> GetAll()
        {
            var products = await _productService.GetAll();
            return Ok(products);
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Add(ProductCreateDTO products) {
           await _productService.Add(products);
            return Ok(products);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id) {
            var product = await _productService.GetById(id);
            if (product == null) return NotFound();
            return Ok(product);
        }
        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, ProductUpdateDTO products)
        {

            var result = await _productService.Update(id, products);

            if (!result)
                return NotFound();

            return Ok();
        }
        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id){
        var product= await _productService.DeleteById(id);
       if(!product)return NotFound();
      
            return Ok();
        }
    }

}
