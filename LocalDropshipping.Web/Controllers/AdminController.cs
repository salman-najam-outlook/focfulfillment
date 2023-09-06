
using LocalDropshipping.Web.Data.Entities;
using LocalDropshipping.Web.Dtos;
using LocalDropshipping.Web.Models;
using LocalDropshipping.Web.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata.Ecma335;

namespace LocalDropshipping.Web.Controllers
{
    public class AdminController : Controller
    {

        private readonly IAdminService service;
        private readonly IProductsService productsService;
        public ICategoryService CategoryService { get; }

        public AdminController(IAdminService service, IProductsService productsService, ICategoryService _categoryService)
        {
            this.service = service;
            this.productsService = productsService;
            CategoryService = _categoryService;
        }


        public IActionResult AdminLogin()
        {
            return View();
        }
        //[HttpPost]
        //public async Task<IActionResult> AdminLogin(AdminLoginViewModel model)
        //{
        //	if (ModelState.IsValid)
        //	{
        //		var result = await service.AdminLoginUser(model.Email, model.Password);

        //		if (result.Succeeded)
        //		{
        //			// Check if the user is an admin
        //			var isAdmin = await service.IsUserAdminAsync(model.Email);

        //			if (isAdmin)
        //			{
        //				// Redirect to the admin dashboard if the user is an admin
        //				return RedirectToAction("us7yhs6tdgv", "Admin");
        //			}
        //			else
        //			{
        //				// Handle non-admin user here, e.g., redirect to a different page
        //				ModelState.AddModelError("", "OOPs. its seem like you're not an admin");
        //				//	return RedirectToAction("NonAdminPage");
        //			}
        //		}

        //		ModelState.AddModelError("", "Invalid username or password.");
        //	}

        //	return View(model);
        //}

        [HttpPost]
        public async Task<IActionResult> AdminLogin(AdminLoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await service.AdminLoginUser(model.Email, model.Password);

                if (result.Succeeded)
                {
                    // Check if the user is an admin
                    var isAdmin = await service.IsUserAdminAsync(model.Email);

                    // Check if the user is superadmin
                    var isSuperAdmin = await service.IsUserSuperAdminAsync(model.Email);

                    // Check if the user is active
                    var isActive = await service.IsUserActiveAsync(model.Email);

                    if (isAdmin)// || isSuperAdmin
                    {
                        if (isActive)
                        {
                            // Redirect to the admin dashboard if the user is an admin active
                            return RedirectToAction("us7yhs6tdgv", "Admin");
                        }
                        else
                        {
                            // Handle inactive admin/superadmin
                            ModelState.AddModelError("", "Your account is not active.");
                        }
                    }
                    else
                    {
                        // Handle non-admin user here, e.g., redirect to a different page
                        ModelState.AddModelError("", "Oops, it seems like you're not an admin.");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Invalid username or password.");
                }
            }

            return View(model);
        }



        [HttpGet]
        public IActionResult Dashboard()
        {
            return View();
        }

        [HttpGet]
        public IActionResult ProductsList()
        {

            var data = this.productsService.GetAll();
            return View(data);
        }

        [HttpGet]
        public IActionResult GetById(int id)
        {
            return View(this.productsService.GetById(id));
        }


        [HttpGet]
        public IActionResult AddProduct()
        {

            return View();
        }


        [HttpPost]
        public IActionResult AddProduct(Product product)
        {
            if (ModelState.IsValid)
            {
                this.productsService.Add(product);
                return RedirectToAction("ProductsList");
            }
            return View();
        }

        public IActionResult Edit(int id)
        {
            var product = this.productsService.GetById(id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }


        [HttpPost]
        public IActionResult Edit(int id, ProductDto product)
        {

        if (ModelState.IsValid)
        {
		    this.productsService.Update(id,product);
            return RedirectToAction("Dashboard");
        }
        return View(product);
        }

        [HttpPost]
        public IActionResult Deleted(int id)
        {
            var product = this.productsService.Delete(id);
            return RedirectToAction("ProductsList");
        }
    }



}

