using ECommerce.Application.DTO;
using ECommerce.Application.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography.X509Certificates;

namespace ECommerce.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;
        public CategoryController(ICategoryService categoryService) {
            _categoryService = categoryService;
        }
        [HttpGet]
        public IActionResult GetAll() {
            var category = _categoryService.GetAll();
            return Ok(category);
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult Add(CategoryCreateDTO categoryDTO) {
            _categoryService.Add(categoryDTO);
            return Ok(categoryDTO);
        }
        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public IActionResult Delete(int id) {
            var category = _categoryService.Delete(id);
            if (category == false) return NotFound();
           
            return Ok();
        }
        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public IActionResult Update(int id,CategoryUpdateDTO categoryUpdateDTO){
        var category=_categoryService.Update(id,categoryUpdateDTO);
        if(category == false) return NotFound();
            return Ok(category);
        }
        [HttpGet("{id}")]
        public IActionResult GetById(int id){
        var category=_categoryService.GetById(id);
        if(category==null) return NotFound();
            return Ok(category);
        }

       
    }
}
