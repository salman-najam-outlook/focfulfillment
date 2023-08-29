using LocalDropshipping.Web.Data.Entities;
using LocalDropshipping.Web.Dtos;
using LocalDropshipping.Web.Services;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace LocalDropshipping.Web.Controllers.API
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductsService productService;

        public ProductsController(IProductsService productService)
        {
            this.productService = productService;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(productService.GetAll());
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            return Ok(productService.GetById(id));
        }

        [HttpPost]
        public IActionResult Post(ProductDto productDto)
        {
            Product product = productDto.ToEntity();
            productService.Add(product);
            return Ok();
        }
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            productService.Delete(id);
            return Ok();
        }
        [HttpPost("{id}")]
        public IActionResult Update(int id, ProductDto productDto)
        {
            Product product = productDto.ToEntity();
            productService.Update(id, productDto);
            return Ok();
        }


    }
}
