using LocalDropshipping.Web.Data.Entities;
using LocalDropshipping.Web.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LocalDropshipping.Web.Controllers
{
    public class ShopController : Controller
    {
        private readonly IProductsService productsService;

        public ShopController(IProductsService productsService)
        {
            this.productsService = productsService;
        }
        public IActionResult FilterPage()
        {

            return View(productsService.GetAll());
        }
        public IActionResult Filter(string searchstring)
        {
            var allProducts=productsService.GetAll();
            
            if (!string.IsNullOrEmpty(searchstring))
            {
                var filteredresults = productsService.GetProductBySearchFilter(searchstring);
                if(filteredresults !=null)
                {
                    return View("FilterPage", filteredresults);
                }
            }
            return View("FilterPage", allProducts);
        }
    }
}
