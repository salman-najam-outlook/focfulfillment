
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
using System.Reflection.Metadata.Ecma335;
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

        public AdminController(IAdminService service, IProductsService productsService, IUserService userService, UserManager<User> userManager, SignInManager<User> signInManager, LocalDropshippingContext context)
        {
            this.service = service;
            this.productsService = productsService;
            this.userService = userService;
            _userManager = userManager;
            _signInManager = signInManager;
            this.context = context;
        }


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


        //public ActionResult us7yhs6tdgv()
        //{
        //	return View();
        //}

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
                userService.DisableUser(userId);
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
                        //Fullname = "Zeeshan",
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
            try
            {
                return View(userService.GetAll());
            }
            catch (Exception ex)
            {

                return View();
            }
           
        }


        [HttpGet]
        public IActionResult Dashboard()
        {
            return View();
        }

        [HttpGet]
        public IActionResult ProductsLists()
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
        public IActionResult Create()
        {
            return View();
        }


        [HttpPost]
        public IActionResult Create(Product product)
        {
            if (ModelState.IsValid)
            {
                this.productsService.Add(product);
                return RedirectToAction("Dashboard");

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
                this.productsService.Update(id, product);
                return RedirectToAction("Dashboard");
            }
            return View(product);
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            var product = this.productsService.Delete(id);
            return RedirectToAction("Dashboard");
        }
    }
}

