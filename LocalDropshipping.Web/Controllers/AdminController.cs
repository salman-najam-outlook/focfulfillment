
using LocalDropshipping.Web.Data;
using LocalDropshipping.Web.Data.Entities;
using LocalDropshipping.Web.Dtos;
using LocalDropshipping.Web.Enums;
using LocalDropshipping.Web.Models;
using LocalDropshipping.Web.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Reflection.Metadata.Ecma335;
using System.Security.Claims;

namespace LocalDropshipping.Web.Controllers
{
    public class AdminController : Controller
    {
        public ICategoryService CategoryService { get; }

        private readonly IAdminService _service;
        private readonly IProductsService _productsService;
        private readonly IUserService _userService;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly LocalDropshippingContext _context;
        private readonly ICategoryService _categoryService;
        private readonly IAccountService _accountService;
        private readonly IOrderService _orderService;

        public AdminController(IAdminService service, IProductsService productsService, IUserService userService, UserManager<User> userManager, SignInManager<User> signInManager, LocalDropshippingContext context, ICategoryService categoryService, IAccountService accountService, IOrderService orderService)
        {
            _service = service;
            _productsService = productsService;
            _userService = userService;
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
            _categoryService = categoryService;
            _orderService = orderService;
            CategoryService = _categoryService;
            _accountService = accountService;
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

                    if (isAdmin || isSuperAdmin)
                    {
                        if (isActive)
                        {
                            // Redirect to the admin dashboard if the user is an admin active
                            return RedirectToAction("Dashboard", "Admin");
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

        public IActionResult AddNewUser()
        {
            try
            {
                string? currentUserID = _userManager.GetUserId(HttpContext.User);
                var currentUser = _userService.GetById(currentUserID);
                bool isAdmin = currentUser.IsAdmin;
                bool isSuperAdmin = currentUser.IsSuperAdmin;

                var model = new UserViewModel();

                if (isAdmin)
                {
                    model.IsSeller = true;
                }


                TempData["IsAdmin"] = isAdmin;
                TempData["IsSuperAdmin"] = isSuperAdmin;

            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Something went wrong. Please try again later!!!";
                return RedirectToAction("AdminLogin", "Admin");
            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddNewUser(UserViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    // Check Line 137 To 144: In this line, I am getting FullName Using Email & UserName 
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
                        IsActive = true,
                    };

                    // Use the AccountService to register the user and send the email
                    var result = await _accountService.RegisterAsync(user, model.Password);

                    if (result.Succeeded)
                    {
                        TempData["RegistrationConfirmation"] = "Registration was successful. You can now log in.";
                        HttpContext.Session.SetString("AdminSideVerificationEmailSent", "True");
                        return RedirectToAction("AdminSideVerificationEmailSent");
                    }
                    if (result.Succeeded)
                    {

                        _userService.Add(user);

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

                SetRoleByCurrentUser();

                return View(model);
            }
            catch (Exception ex)
            {
                // Handle exceptions
                return View();
            }
        }

        #region AdminSideEmailVerification

        public IActionResult AdminSideVerificationEmailSent()
        {
            return View();
        }

        #endregion
        public IActionResult StaffMember()
        {
            SetRoleByCurrentUser();
            return View(_userService.GetAllStaffMember());
        }
        public IActionResult EditUser()
        {
            return View();
        }


        [HttpPost]
        public IActionResult DeleteUser(string userId)
        {
            _userService.Delete(userId);
            SetRoleByCurrentUser();
            return View("GetAllSellers", _userService.GetAll());
        }

        [HttpPost]
        public IActionResult ActivateUser(string userId)
        {
            if (!string.IsNullOrEmpty(userId))
            {
                _userService.ActivateUser(userId);
            }

            var sellers = _userService.GetAll();


            SetRoleByCurrentUser();

            return View("StaffMember", sellers);
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();

            return RedirectToAction("AdminLogin", "Admin");
        }


        public IActionResult GetAllSellers()
        {
            SetRoleByCurrentUser();
            return View(_userService.GetAll());
        }


        [HttpGet]
        public IActionResult Dashboard()
        {
            SetRoleByCurrentUser();
            return View();
        }



        [HttpGet]
        public IActionResult OrdersList()
        {
            try
            {
                List<Order> orders = _orderService.GetAll();
                return View(orders);
            }
            catch (Exception ex)
            {
                return View(ex.Message); 
            }
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
        public IActionResult Delete(int id)
        {
            var product = _productsService.Delete(id);
            return RedirectToAction("Dashboard");
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

        public IActionResult SubmitForm(Product model)
        {
            if (model != null)
            {
                _productsService.Add(model);
                return RedirectToAction("GetAllProducts");

            }
            return View("Post");
        }

        private void SetRoleByCurrentUser()
        {
            string? currentUserID = _userManager.GetUserId(HttpContext.User);
            var currentUser = _userService.GetById(currentUserID);
            TempData["IsAdmin"] = currentUser.IsAdmin;
            TempData["IsSuperAdmin"] = currentUser.IsSuperAdmin;
        }
    }
}