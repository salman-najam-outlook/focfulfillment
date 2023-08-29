using LocalDropshipping.Web.Data.Entities;
using LocalDropshipping.Web.Dtos;
using LocalDropshipping.Web.Models;
using LocalDropshipping.Web.Services;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.Contracts;

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

        public IActionResult Post()
        {
            return View();

        }
        [HttpPost]
        public IActionResult SubmitForm(Product model)
        {
            if (model != null)
            {
                productsService.Add(model);
                return RedirectToAction("Index");

            }
            return View("Post");
        }

    }
}
