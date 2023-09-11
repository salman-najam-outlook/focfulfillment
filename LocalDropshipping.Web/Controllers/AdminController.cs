
using LocalDropshipping.Web.Attributes;
using LocalDropshipping.Web.Data;
using LocalDropshipping.Web.Data.Entities;
using LocalDropshipping.Web.Enums;
using LocalDropshipping.Web.Models;
using LocalDropshipping.Web.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

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

        public AdminController(IAdminService service, IProductsService productsService, IUserService userService, UserManager<User> userManager, SignInManager<User> signInManager, LocalDropshippingContext context, ICategoryService categoryService)
        {
            _service = service;
            _productsService = productsService;
            _userService = userService;
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
            _categoryService = categoryService;
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
                Microsoft.AspNetCore.Identity.SignInResult result = await _service.AdminLoginUser(model.Email, model.Password);

                if (result.Succeeded)
                {
                    // Check if the user is an admin
                    bool isAdmin = await _service.IsUserAdminAsync(model.Email);

                    // Check if the user is superadmin
                    bool isSuperAdmin = await _service.IsUserSuperAdminAsync(model.Email);

                    // Check if the user is active
                    bool isActive = await _service.IsUserActiveAsync(model.Email);

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

            List<User?> sellers = _userService.GetAll();

            //string? currentUserID = _userManager.GetUserId(HttpContext.User);
            //var currentUser = _userService.GetById(currentUserID);
            //TempData["IsAdmin"] = currentUser.IsAdmin;
            //TempData["IsSuperAdmin"] = currentUser.IsSuperAdmin;
            SetRoleByCurrentUser();

            return View("StaffMember", sellers);
        }

        public IActionResult AddNewUser()
        {
            try
            {
                string? currentUserID = _userManager.GetUserId(HttpContext.User);
                User? currentUser = _userService.GetById(currentUserID);
                bool isAdmin = currentUser.IsAdmin;
                bool isSuperAdmin = currentUser.IsSuperAdmin;

                UserViewModel model = new UserViewModel();

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
                    User user = new User
                    {
                        Fullname = emailUsername,
                        UserName = model.UserName,
                        Email = model.Email,
                        PhoneNumber = model.PhoneNumber,
                        IsAdmin = model.IsAdmin,
                        IsSeller = model.IsSeller,
                        IsActive = true,
                    };

                    IdentityResult result = await _userManager.CreateAsync(user, model.Password);


                    if (result.Succeeded)
                    {
                        _userService.Add(user);

                        return RedirectToAction("StaffMember", "Admin");
                    }
                    else
                    {
                        foreach (IdentityError error in result.Errors)
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
                return View();
            }
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
        [Authorize]
        [AuthorizeOnly(Roles.Admin | Roles.SuperAdmin, "AdminLogin", "Admin")]
        public IActionResult Dashboard()
        {
            SetRoleByCurrentUser();
            return View();
        }
        #endregion

        [HttpGet]
        [Authorize]
        [AuthorizeOnly(Roles.SuperAdmin | Roles.Admin)]
        public IActionResult Products()
        {
            List<Product> data = _productsService.GetAll();
            return View(data);
        }


        [HttpGet]
        [Authorize]
        [AuthorizeOnly(Roles.SuperAdmin | Roles.Admin)]
        public IActionResult AddUpdateProduct(int id = 0)
        {
            ViewBag.Categories = _categoryService.GetAll();
            var productVeiwModel = new ProductViewModel(_productsService.GetById(id));
            return View(productVeiwModel);
        }


        [HttpPost]
        [Authorize]
        [AuthorizeOnly(Roles.SuperAdmin | Roles.Admin)]
        public IActionResult AddUpdateProduct(ProductViewModel model)
        {
            ViewBag.Categories = _categoryService.GetAll();
            ModelState.Remove("ProductId");
            if (ModelState.IsValid)
            {
                if (model.ProductId != 0)
                {
                    var product = _productsService.GetById(model.ProductId);
                    product.Name = model.Name;
                    product.Description = model.Description;
                    product.Price = model.Price;
                    product.CategoryId = model.CategoryId;
                    product.IsNewArravial = model.IsNewArravial;
                    product.IsBestSelling = model.IsBestSelling;
                    product.IsFeatured = model.IsFeatured;
                    product.Quantity = model.Quantity;
                    product.SKU = model.SKU;

                    _productsService.Update(model.ProductId, product);
                    TempData["updated"] = "Product updated successfully";
                    return RedirectToAction("Products");
                }
                else
                {
                    var product = model.ToEntity();
                    _productsService.Add(product);
                    TempData["ProductAdded"] = "Product added successfully";
                    return RedirectToAction("Products");
                }
            }
            return View(model);
        }


        [HttpPost]
        [Authorize]
        [AuthorizeOnly(Roles.SuperAdmin | Roles.Admin)]
        public IActionResult DeleteProduct(int id)
        {
            try
            {
                Product product = _productsService.Delete(id);
                TempData["Message"] = "Product deleted successfully.";
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                TempData["Message"] = "no";
            }
            return RedirectToAction("Products");
        }


        private void SetRoleByCurrentUser()
        {
            string? currentUserID = _userManager.GetUserId(HttpContext.User);
            User? currentUser = _userService.GetById(currentUserID);
            TempData["IsAdmin"] = currentUser.IsAdmin;
            TempData["IsSuperAdmin"] = currentUser.IsSuperAdmin;
        }
    }
}