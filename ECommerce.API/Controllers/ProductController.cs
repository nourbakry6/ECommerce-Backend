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
        public IActionResult GetAll()
        {
            var products = _productService.GetAll();
            return Ok(products);
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult Add(ProductCreateDTO products) {
            _productService.Add(products);
            return Ok(products);
        }
        [HttpGet("{id}")]
        public IActionResult GetById(int id) {
            var product = _productService.GetById(id);
            if (product == null) return NotFound();
            return Ok(product);
        }
        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public IActionResult Update(int id, ProductUpdateDTO products)
        {

            var result = _productService.Update(id, products);

            if (!result)
                return NotFound();

            return Ok();
        }
        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public IActionResult Delete(int id){
        var product=_productService.DeleteById(id);
       if(!product)return NotFound();
      
            return Ok();
        }
    }

}
