using LocalDropshipping.Web.Services;
using Microsoft.AspNetCore.Mvc;

namespace LocalDropshipping.Web.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductsService productsService;

        public ProductController(IProductsService productsService)
        {
            this.productsService = productsService;
        }

        public IActionResult Index()
        {
            return View(productsService.GetAll());
        }
    }
}
