
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
        private readonly IAdminService _service;
        private readonly IProductsService _productsService;
        public ICategoryService CategoryService { get; }

        public AdminController(IAdminService service, IProductsService productsService, ICategoryService _categoryService)
        {
            _service = service;
            _productsService = productsService;
            CategoryService = _categoryService;
        }

        #region Admin Login
        public IActionResult AdminLogin()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AdminLogin(AdminLoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _service.AdminLoginUser(model.Email, model.Password);

                if (result.Succeeded)
                {
                    // Check if the user is an admin
                    var isAdmin = await _service.IsUserAdminAsync(model.Email);

                    // Check if the user is superadmin
                    var isSuperAdmin = await _service.IsUserSuperAdminAsync(model.Email);

                    // Check if the user is active
                    var isActive = await _service.IsUserActiveAsync(model.Email);

                    if (isAdmin)// || isSuperAdmin
                    {
                        if (isActive)
                        {
                            // Redirect to the admin dashboard if the user is an admin active
                            return RedirectToAction("Dashboard", "Admin");
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
        #endregion

        [HttpGet]
        public IActionResult ProductsList()
        {

            var data = _productsService.GetAll();
            return View(data);
        }

        [HttpGet]
        public IActionResult GetById(int id)
        {
            return View(_productsService.GetById(id));
        }

        [HttpGet]
        public IActionResult AddUpdateProduct(int? Id = 0)
        {
            int id = Convert.ToInt32(Id);
            if (Id == 0)
            {
                return View();
            }
            return View(_productsService.GetById(id));
        }

        [HttpPost]
        public IActionResult AddUpdateProduct(Product product)
        {
            ModelState.Remove("ProductId");
            if (ModelState.IsValid)
            {
                if (product.ProductId != 0)
                {
                    ProductDto data = new ProductDto
                    {
                        Name = product.Name,
                        Description = product.Description,
                        Price = product.Price,
                        Stock = product.Quantity,
                        ImageLink = product.ImageContent,
                        UpdatedDate = DateTime.Now,
                        CreatedDate = DateTime.Now,
                        SKU = product.SKU
                    };

                    if (product.Price > 0)
                    {
                        _productsService.Update(product.ProductId, data);
                        TempData["updated"] = "Product updated successfully";
                        return RedirectToAction("ProductsList");
                    }
                    else
                    {
                        ModelState.AddModelError("Price", "Price cannot be negative.");
                    }
                }
                else
                {
                    if (product.Price > 0)
                    {
                        _productsService.Add(product);
                        TempData["addded"] = "Product added successfully";
                        return RedirectToAction("ProductsList");
                    }
                    else
                    {
                        ModelState.AddModelError("Price", "Price cannot be negative.");
                    }
                }
            }
            return View(product);
        }

        [HttpPost]
        public IActionResult Deleted(int id)
        {
            try
            {
                var product = _productsService.Delete(id);
                TempData["Message"] = "Product deleted successfully.";
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                TempData["Message"] = "no";
            }
            return RedirectToAction("ProductsList");
        }
    }
}

