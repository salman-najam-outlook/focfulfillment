using LocalDropshipping.Web.Services;
using Microsoft.AspNetCore.Mvc;

namespace LocalDropshipping.Web.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ICategoryService categoryService;

        public CategoryController(ICategoryService categoryService) 
        {
            this.categoryService = categoryService;
        }
       
        public IActionResult Index()
        {
            return View(categoryService.GetAll());
        }
    }
}
