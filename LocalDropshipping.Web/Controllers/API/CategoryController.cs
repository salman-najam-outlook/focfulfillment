using LocalDropshipping.Web.Data.Entities;
using LocalDropshipping.Web.Dtos;
using LocalDropshipping.Web.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LocalDropshipping.Web.Controllers.API
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService categoryService;

        public CategoryController(ICategoryService categoryService) 
        {
            this.categoryService = categoryService;
        }

        [HttpGet]
        public IActionResult GetAll()
        
        {
            return Ok(categoryService.GetAll());
        }


        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            return Ok(categoryService.GetById(id));
        }



        [HttpPost]
        public IActionResult AddCategory(CategoryDto categoryDto)
        {
            Category category = categoryDto.ToEntity();
            categoryService.Add(category);
            return Ok();
        }
        [HttpDelete("{id}")]
        public IActionResult DeleteCategory(int id)
        {
            categoryService.Delete(id)
;
            return Ok();
        }
        [HttpPost("{id}")]
        public IActionResult UpdateCategory(int id, CategoryDto categoryDto)
        {
            Category category = categoryDto.ToEntity();
            categoryService.Update(id, categoryDto);
            return Ok();
        }
    }
}
