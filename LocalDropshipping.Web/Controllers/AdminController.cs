
using LocalDropshipping.Web.Data;
using LocalDropshipping.Web.Data.Entities;
using LocalDropshipping.Web.Dtos;
using LocalDropshipping.Web.Enums;
using LocalDropshipping.Web.Models;
using LocalDropshipping.Web.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace LocalDropshipping.Web.Controllers
{
    public class AdminController : Controller
    {

        private readonly IAdminService service;
        private readonly IProductsService productsService;
        private readonly IUserService userService;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly LocalDropshippingContext context;
        private readonly ICategoryService categoryService;

        public AdminController(IAdminService service, IProductsService productsService, IUserService userService, UserManager<User> userManager, SignInManager<User> signInManager, LocalDropshippingContext context)
        {
            _service = service;
            _productsService = productsService;
            _userService = userService;
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
            _categoryService = categoryService;
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
                var result = await service.AdminLoginUser(model.Email, model.Password);

                if (result.Succeeded)
                {
                    var isAdmin = await service.IsUserAdminAsync(model.Email);

                    var isSuperAdmin = await service.IsUserSuperAdminAsync(model.Email);

                    var isActive = await service.IsUserActiveAsync(model.Email);

                    if (isAdmin)// || isSuperAdmin
                    {
                        if (isActive)
                        {
                            return RedirectToAction("us7yhs6tdgv", "Admin");
                        }
                        else
                        {
                            ModelState.AddModelError("", "Your account is not active.");
                        }
                    }
                    else
                    {
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

        public IActionResult StaffMember()
        {
            return View(userService.GetAllStaffMember());
        }
        public IActionResult EditUser()
        {
            return View();
        }


        [HttpPost]
        public IActionResult DeleteUser(string userId)
        {
            userService.Delete(userId);
            return View("GetAllSellers", userService.GetAll());
        }
        [HttpPost]
        public IActionResult DisableUser(string userId)
        {
            if (!string.IsNullOrEmpty(userId))
            {
                var user = userService.DisableUser(userId);
            }

            var sellers = userService.GetAll();

            return View("StaffMember", sellers);
        }

        public IActionResult AddNewUser()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddNewUser(UserViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    // Check Line 137 To 144 : In this line i am getting FullName Using Email & UserName 
                    string[] emailParts = model.Email.Split('@');
                    string emailUsername = emailParts.Length > 0 ? emailParts[0] : string.Empty;
                    string[] usernameWords = model.UserName.Split(' ');
                    emailUsername = string.Join(" ", emailUsername.Split(' ').Except(usernameWords));
                    emailUsername = emailUsername.Trim();
                    var user = new User
                    {
                        Fullname = emailUsername,
                        UserName = model.UserName,
                        Email = model.Email,
                        PhoneNumber = model.PhoneNumber,
                        IsAdmin = model.IsAdmin,
                        IsSeller = model.IsSeller,
                        IsActive = true
                        //DeletedBy = User.Identity.Name 
                    };

                    var result = await _userManager.CreateAsync(user, model.Password);

                    if (result.Succeeded)
                    {
                        userService.Add(user);

                        return RedirectToAction("StaffMember", "Admin");
                    }
                    else
                    {
                        foreach (var error in result.Errors)
                        {
                            ModelState.AddModelError(string.Empty, error.Description);
                        }
                    }
                }

                return View(model);
            }
            catch (Exception ex)
            {
                return View();
            }
        }
        public IActionResult GetAllSellers()
        {
            return View(userService.GetAll());
        }


        [HttpGet]
        public IActionResult Dashboard()
        {
            return View();
        }
        #endregion

        #region Products
        public IActionResult GetAllProducts()
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
                return RedirectToAction("GetAllProducts");

            }
            return View("Post");
        }
        #endregion

        #region Categories
        public IActionResult GetAllCategories()
        {
            return View(categoryService.GetAll());
        }
        #endregion

        [HttpPost]
        public IActionResult Delete(int id)
        {
            var product = this.productsService.Delete(id);
            return RedirectToAction("Dashboard");
        }
    }
}

