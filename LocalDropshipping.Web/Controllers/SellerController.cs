﻿using LocalDropshipping.Web.Attributes;
using LocalDropshipping.Web.Data.Entities;
using LocalDropshipping.Web.Exceptions;
using LocalDropshipping.Web.Extensions;
using LocalDropshipping.Web.Models;
using LocalDropshipping.Web.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text;

namespace LocalDropshipping.Web.Controllers
{
    public class SellerController : Controller
    {
        private readonly IProfilesService _profileService;
        private readonly IUserService _userService;
        private readonly IAccountService _accountService;
        private readonly IOrderService _orderService;
        private readonly IProductsService _productsService;

        public SellerController(IAccountService accountService, IProfilesService profileService, IUserService userService, IOrderService orderService, IProductsService productsService)
        {
            _accountService = accountService;
            _profileService = profileService;
            _userService = userService;
            _orderService = orderService;
            _productsService = productsService;
        }

        public IActionResult Register()
        {
            string? isEmailSent = HttpContext.Session.GetString("VerificationEmailSent");
            if (isEmailSent != null && isEmailSent == "True")
                return RedirectToAction("VerificationEmailSent", "Seller");
            return View();
        }

        public IActionResult Login()
        {
            return View();
        }
        public IActionResult ForgotPassword()
        {
            return View();
        }
        public IActionResult UpdatePassword(string userId, string token)
        {
            return View();
        }
        public IActionResult UpdatePasswordMessage()
        {
            return View();
        }
        public IActionResult ForgotPasswordMessage()
        {
            return View();
        }
        public IActionResult VerificationEmailSent()
        {
            return View();
        }
        public IActionResult EmailVerified()
        {
            return View();
        }
        public IActionResult contactUs()
        {
            return View();
        }
        public IActionResult contactUsMessage()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> contactUs(ContactUsViewModel contactUsViewModel)
        {
            if (ModelState.IsValid)
            {
                bool emailSent = await _accountService.SendContactEmailAsync(contactUsViewModel);

                if (emailSent)
                {
                    return RedirectToAction("contactUsMessage");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Failed to send contact email. Please try again later.");
                }
            }
            return View("ContactUs", contactUsViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var user = await _accountService.LoginAsync(model.Email, model.Password);
                    return RedirectToAction("Shop", "Seller");
                }
            }
            catch (IdentityException ex)
            {
                ModelState.AddModelError("Login Failed", ex.Message);
            }
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Register(SignupViewModel model)
        {
            if (ModelState.IsValid)
            {
                User user = new User
                {
                    Email = model.Email,
                    UserName = model.Email,
                    Fullname = string.Join(" ", model.FirstName, model.LastName),
                    IsSeller = true
                };

                var result = await _accountService.RegisterAsync(user, model.Password);
                if (result.Succeeded)
                {
                    TempData["RegistrationConfirmation"] = "Registration was successful. You can now log in.";
                    HttpContext.Session.SetString("VerificationEmailSent", "True");
                    return RedirectToAction("VerificationEmailSent");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        if (error.Code == "DuplicateUserName")
                        {
                            ModelState.AddModelError("CustomErrorMessage", string.Join(": ", "DuplicateEmail", "Email already exist"));
                        }
                        else
                        {
                            ModelState.AddModelError("CustomErrorMessage", string.Join(": ", error.Code, error.Description));
                        }
                    }
                }
            }
            return View(model);
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdatePassword(NewPasswordViewModel model)
        {
            var isUpdated = await _accountService.UpdatePasswordAsync(model);

            if (isUpdated)
            {
                return RedirectToAction("UpdatePasswordMessage");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Password update failed.");
                return View("UpdatePassword");
            }
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> EmailVerification(string userId, string token)
        {
            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(token))
            {
                return RedirectToAction("InvalidVerificationLink");
            }

            var isVerified = await _accountService.ConfirmEmailAsync(userId, token);
            if (isVerified)
            {
                return RedirectToAction("EmailVerified");
            }
            else
            {
                return RedirectToAction("InvalidVerificationLink");
            }
        }

        [HttpPost]
        public async Task<IActionResult> ForgotPassword(string email)
        {
            var isPasswordResetLinkSent = await _accountService.ForgotPasswordAsync(email);
            if (isPasswordResetLinkSent)
            {
                TempData["ForgotPassword"] = "Password Forgot successful. You can now log in.";
                return RedirectToAction("ForgotPasswordMessage");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Email not found.");
                return View("ForgotPassword");
            }
        }

        public IActionResult Subscribe()
        {
            var user = _userService.GetCurrentUserAsync().GetAwaiter().GetResult();
            if (user != null && user.IsSubscribed && user.IsActive)
                return RedirectToAction("Shop", "Seller");
            return View();
        }

        [Authorize]
        [TypeFilter(typeof(CustomAuthorizationFilter))]
        public IActionResult Shop()
        {
            ViewBag.products = _productsService.GetAll();
            var cart = HttpContext.Session.Get<List<OrderItem>>("cart");
            ViewBag.total = TotalCost();
            ViewBag.totalCost = ViewBag.total + ViewBag.shipping;
            return View(cart);
        }
        [HttpPost]
        public JsonResult AddToCart(string id)
        {
            try
            {
                Product productItem = _productsService.GetById(Convert.ToInt32(id == string.Empty ? 0 : id));
                var mainVariant = productItem.Variants.FirstOrDefault(x => x.VariantType == "MAIN_VARIANT");
                var cart = HttpContext.Session.Get<List<OrderItem>>("cart");
                int quantity = 1;
                if (cart == null) //no item in the cart
                {
                    cart = new List<OrderItem>();
                    cart.Add(new OrderItem
                    {
                        ProductId = productItem.ProductId,
                        Name = productItem.Name,
                        Image = mainVariant.FeatureImageLink,
                        Quantity = quantity,
                        Price = mainVariant.VariantPrice,
                        SubTotal = quantity * mainVariant.VariantPrice
                    });
                }
                else
                {
                    int index = cart.FindIndex(w => w.ProductId == (Convert.ToInt32(id)));
                    if (index != -1) //if item already in the 
                    {
                        cart[index].Quantity++; //increment by 1
                        cart[index].SubTotal = cart[index].Quantity * cart[index].Price;
                    }
                    else
                    {
                        cart.Add(new OrderItem
                        {
                            ProductId = productItem.ProductId,
                            Name = productItem.Name,
                            Image = mainVariant.FeatureImageLink,
                            Quantity = quantity,
                            Price = mainVariant.VariantPrice,
                            SubTotal = quantity * mainVariant.VariantPrice
                        });
                    }
                }
                HttpContext.Session.Set<List<OrderItem>>("cart", cart);
                return Json(data: new { Success = true, Counter = cart.Count, Cart = cart });
            }
            catch (Exception ex)
            {
                return Json(data: new { Error = ex.ToString() });
            }
        }

        public JsonResult Minus(string id)
        {

            try
            {
                Product productItem = _productsService.GetById(Convert.ToInt32(id == string.Empty ? 0 : id));
                var cart = HttpContext.Session.Get<List<OrderItem>>("cart");

                if (cart != null && cart.Any())
                {
                    int index = cart.FindIndex(w => w.ProductId == (Convert.ToInt32(id == string.Empty ? 0 : id)));
                    if (index == -1)
                    {
                        return Json(data: new { error = false, Counter = cart.Count, Cart = cart });
                    }
                    else
                    {
                        if (cart[index].Quantity == 1) //last item of a product
                        {
                            cart.RemoveAt(index); //remove it
                        }
                        else
                        {
                            cart[index].Quantity--; //reduce by 1
                        }
                        HttpContext.Session.Set<List<OrderItem>>("cart", cart);
                        return Json(data: new { Success = true, Counter = cart.Count, Cart = cart });
                    }

                }
                else
                {
                    return Json(data: new { error = false });
                }

            }
            catch (Exception ex)
            {
                return Json(data: new { Error = ex.ToString() });
            }
        }
        public PartialViewResult GetCartItems()
        {
            var cart = HttpContext.Session.Get<List<OrderItem>>("cart");
            ViewBag.total = TotalCost();
            return PartialView("_cartItem", cart);
        }

        public IActionResult Withdrawal()
        {
            try
            {
                //var data = _orderService.GetAll();
                //return View(data);
                var orders = _orderService.GetAll();

                var withdrawalModels = orders.Select(order => new withdrawalModel
                {
                    Id = order.Id,
                    Name = order.Name,
                    GrandTotal = order.GrandTotal,
                    OrderCode = order.OrderCode,
                    OrderStatus=order.OrderStatus,

                }).ToList();

                return View(withdrawalModels);

            }
            catch (Exception ex)
            {
                return View(ex.Message);
            }
        }

        [HttpGet]
        public IActionResult SellerDashboard()
        {
            try
            {
                var data = _orderService.GetAll();
                return View(data);
            }
            catch (Exception ex)
            {
                return View(ex.Message);
            }
        }

        [HttpGet]
        public IActionResult WishList()
        {
            return View();
        }

        public IActionResult Productleftthumbnail()
        {
            return View();
        }

        public IActionResult Cart()
        {
            var cart = HttpContext.Session.Get<List<OrderItem>>("cart");
            ViewBag.total = TotalCost();
            ViewBag.shipping = 250;
            ViewBag.totalCost = ViewBag.total + ViewBag.shipping;
            return View(cart);
        }

        public IActionResult Remove(int id)
        {
            var cart = HttpContext.Session.Get<List<OrderItem>>("cart");
            int index = cart.FindIndex(w => w.ProductId == id);
            cart.RemoveAt(index);
            HttpContext.Session.Set<List<OrderItem>>("cart", cart);
            return RedirectToAction("Cart");
        }


        public IActionResult Checkout()
        {
            var cart = HttpContext.Session.Get<List<OrderItem>>("cart");
            ViewBag.total = TotalCost();
            ViewBag.shipping = 250;
            ViewBag.totalCost = ViewBag.total + ViewBag.shipping;
            return View(cart);
        }

        public IActionResult ProfileVerification()
        {
            return View();
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> ProfileVerificationAsync(ProfileVerificationViewModel profileVerificationViewModel)
        {
            if (ModelState.IsValid == false)
            {
                return View(profileVerificationViewModel);
            }

            var user = (await _userService.GetCurrentUserAsync())!;
            user.IsProfileCompleted = true;

            var profile = profileVerificationViewModel.ToEntity();
            profile.UserId = user.Id;

            await _userService.UpdateUserAsync(user);
            _profileService.Add(profile);

            return RedirectToAction("Shop", "Seller");
        }

        public IActionResult SellerOrders()
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

        private decimal TotalCost()
        {
            try
            {
                decimal totalRecord;
                var cart = HttpContext.Session.Get<List<OrderItem>>("cart");
                if (cart != null)
                {
                    totalRecord = (decimal)cart.Sum(s => s.Quantity * s.Price);
                }
                else
                {
                    cart = new List<OrderItem>();
                    totalRecord = 0;
                }

                return totalRecord;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }
    }
}
