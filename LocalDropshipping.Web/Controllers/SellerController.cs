using LocalDropshipping.Web.Attributes;
using LocalDropshipping.Web.Data.Entities;
using LocalDropshipping.Web.Exceptions;
using LocalDropshipping.Web.Extensions;
using LocalDropshipping.Web.Helpers;
using LocalDropshipping.Web.Models;
using LocalDropshipping.Web.Services;
using MailKit.Search;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using System.Security.Claims;
using System.Text;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using static NuGet.Packaging.PackagingConstants;

namespace LocalDropshipping.Web.Controllers
{
    public class SellerController : Controller
    {
        private readonly IProfilesService _profileService;
        private readonly IUserService _userService;
        private readonly IAccountService _accountService;
        private readonly IOrderService _orderService;
        private readonly IProductsService _productsService;
        private readonly IWishListService _wishlistService;
        private readonly IProductVariantService _productVariantService;
        private readonly UserManager<User> _userManager;
        private readonly IConsumerService _consumerService;
        private readonly IOrderItemService _orderItemService;
        private readonly IFocSettingService _focSettingService;

        public SellerController(IAccountService accountService, IProfilesService profileService, IUserService userService, IOrderService orderService, UserManager<User> userManager, IWishListService wishList, IProductsService productsService, IProductVariantService productVariantService, IConsumerService consumerService, IOrderItemService orderItemService, IFocSettingService focSettingService)
        {
            _accountService = accountService;
            _profileService = profileService;
            _userService = userService;
            _orderService = orderService;
            _productsService = productsService;
            _wishlistService = wishList;
            _productVariantService = productVariantService;
            _userManager = userManager;
            _consumerService = consumerService;
            _orderItemService = orderItemService;
            _focSettingService = focSettingService;
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
            return View(cart);
        }
        [HttpPost]
        public JsonResult AddToCart(string id)
        {
            try
            {
                Product productItem = _productsService.GetById(Convert.ToInt32(id == string.Empty ? 0 : id));
                var mainVariant = productItem.Variants.First();
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
                int newIndex = cart.FindIndex(w => w.ProductId == (Convert.ToInt32(id)));
                double itemSubtotal = cart[newIndex].SubTotal;
                var newTotalCost = TotalCost();
                var newgrandTotal=newTotalCost+ShippingCost();
                return Json(data: new { Success = true, newSubtotal = itemSubtotal, totalCost = newTotalCost, grandTotal= newgrandTotal, Counter = cart.Count, Cart = cart });
                //return Json(data: new { Success = true, Counter = cart.Count, Cart = cart });
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
                var mainVariant = productItem.Variants.FirstOrDefault(x => x.VariantType == "MAIN_VARIANT");
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
                            cart[index].SubTotal = cart[index].Quantity * cart[index].Price;
                        }
                        HttpContext.Session.Set<List<OrderItem>>("cart", cart);
                        int newIndex = cart.FindIndex(w => w.ProductId == (Convert.ToInt32(id)));
                        decimal newTotalCost;
                        decimal newGrandTotal;
                        if (newIndex == -1)
                        {
                            newTotalCost = TotalCost();
                            newGrandTotal=newTotalCost+ShippingCost();
                            HttpContext.Session.Set<List<OrderItem>>("cart", cart);
                            return Json(data: new { 
                                Success = true, 
                                newQuantity = 0, 
                                totalCost = newTotalCost,
                                grandTotal=newGrandTotal,
                                Counter = cart.Count, 
                                Cart = cart });
                        }
                        else
                        {
                            double itemSubtotal = cart[newIndex].SubTotal;
                            int itemQuantity = cart[newIndex].Quantity;
                            newTotalCost = TotalCost();
							newGrandTotal = newTotalCost + ShippingCost();
                            HttpContext.Session.Set<List<OrderItem>>("cart", cart);
                            return Json(data: new { 
                                Success = true, 
                                newQuantity = itemQuantity, 
                                newSubtotal = itemSubtotal, 
                                totalCost = newTotalCost,
								grandTotal=newGrandTotal,
								Counter = cart.Count, 
                                Cart = cart });
                            //return Json(data: new { Success = true, Counter = cart.Count, Cart = cart });
                        }
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

        public IActionResult Withdrawal([FromQuery] Pagination pagination)
        {
            try
            {
                //var data = _orderService.GetAll();
                //return View(data);
                var orders = _orderService.GetAll();

                var withdrawalModels = orders.Select(order => new withdrawalModel
                {
                    Id = order.Id,
                    GrandTotal = order.GrandTotal,
                    OrderStatus=order.OrderStatus,

                }).ToList();
                var count = withdrawalModels.Count();
                withdrawalModels = withdrawalModels.Skip((pagination.PageNumber - 1) * pagination.PageSize).Take(pagination.PageSize).ToList();
                return View(new PageResponse<List<withdrawalModel>>(withdrawalModels, pagination.PageNumber, pagination.PageSize, count));
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
                return View();
            }
            catch (Exception ex)
            {
                return View(ex.Message);
            }
        }

        [HttpGet]
        public async Task<IActionResult> WishList([FromQuery] Pagination pagination)
        {
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var cartData = _wishlistService.GetAllbyUserId(userId);            
                List<AddProductVariantViewModel> data = new List<AddProductVariantViewModel>();
                foreach (var item in cartData)
                {
                    var temp = await _productVariantService.GetById(item.ProductId);
                    data.Add(new AddProductVariantViewModel {
                        FeatureImageLink= temp.FeatureImageLink == null?"":temp.FeatureImageLink,
                        Quantity = temp.Quantity == null? 0: temp.Quantity,
                        VariantPrice = temp.VariantPrice == null? 0: temp.VariantPrice,
                        VariantType = temp.VariantType == null?"": temp.VariantType
                    });
                }
                var count = data.Count();
                data = data.Skip((pagination.PageNumber - 1) * pagination.PageSize).Take(pagination.PageSize).ToList();
                return View(new PageResponse<List<AddProductVariantViewModel>>(data, pagination.PageNumber, pagination.PageSize, count));
            }
            catch (Exception ex)
            {
                return View();
            }
            
        }


        [HttpPost]
        public IActionResult WishList(int ProductId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (_wishlistService.ValidateWishlistProduct(userId, ProductId))
            {
                _wishlistService.Add(userId, ProductId);
            }
            else
            {
               
                _wishlistService.Delete(ProductId); 
            }

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
            ViewBag.shipping = ShippingCost();
			ViewBag.totalCost = ViewBag.total + ViewBag.shipping;
            return View(cart);
        }

        public IActionResult RemoveFromCart(int id)
        {
            var cart = HttpContext.Session.Get<List<OrderItem>>("cart");
            int index = cart.FindIndex(w => w.ProductId == id);
            cart.RemoveAt(index);
            HttpContext.Session.Set<List<OrderItem>>("cart", cart);
            return RedirectToAction("Cart");
        }


        public IActionResult Checkout(CheckoutViewModel model)
        {
            string? currentUserID = _userManager.GetUserId(HttpContext.User);
            var currentUser = _userService.GetById(currentUserID);
            var cart = HttpContext.Session.Get<List<OrderItem>>("cart");
            ViewBag.total = TotalCost();
            ViewBag.shipping = ShippingCost();
            ViewBag.totalCost = ViewBag.total + ViewBag.shipping;
            var checkoutViewModel = new CheckoutViewModel
            {
                Cart = cart,
            };
            ViewBag.currentUser = currentUser;
            return View(checkoutViewModel);
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

        public IActionResult SellerOrders([FromQuery] Pagination pagination)
        {
            try
            {
                List<Order> orders = _orderService.GetAll();
                var count = orders.Count();
                orders = orders.Skip((pagination.PageNumber - 1) * pagination.PageSize).Take(pagination.PageSize).ToList();
                return View(new PageResponse<List<Order>>(orders, pagination.PageNumber, pagination.PageSize, count));
            }
            catch (Exception ex)
            {

                return View(ex.Message);
            }
        }
        [HttpPost]
        public IActionResult PlaceOrder(CheckoutViewModel customer)
        {
            
            var cart = HttpContext.Session.Get<List<OrderItem>>("cart");
            string? currentUserID = _userManager.GetUserId(HttpContext.User);
            var currentUser = _userService.GetById(currentUserID);
            string email = currentUser.Email;
            var primaryPhone = customer.PrimaryPhoneNumber;
            var secondaryPhone = customer.SecondaryPhoneNumber;
            decimal sellingPrice = Convert.ToDecimal(customer.SellingPrice);

            var checkConsumer = _consumerService.CheckConsumer(primaryPhone, secondaryPhone);
            Order order;
            Consumer consumer;
            if (!checkConsumer)
            {
                order = _orderService.AddOrder(cart, email, sellingPrice);
                var orderId = order.Id;
                consumer=_consumerService.AddConsumer(customer, orderId, email);
                HttpContext.Session.Remove("cart");
            }
            else
            {
                return RedirectToAction("checkout");
            }
            var orderItems = _orderItemService.GetOrderItemsById(order.Id);
            OrderSuccessModel model = new OrderSuccessModel()
            {
                OrderId=order.Id,
                OrderItems = orderItems,
                TotalItems = orderItems.Count(),
                TotalItemsAmount = order.GrandTotal,
                ShippingCharges= ShippingCost(),
                ShippingAddress=consumer.Address,
                ShippingCity=consumer.City,
                GrandTotal=order.GrandTotal+ ShippingCost(),

            };
            return View(model);
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
        private decimal ShippingCost()
        {
			string shippingCost = "Shipping Cost";
			return Convert.ToDecimal(_focSettingService.GetShippingCost(shippingCost));
		}
    }
}
