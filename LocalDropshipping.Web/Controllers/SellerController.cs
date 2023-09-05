using LocalDropshipping.Web.Data.Entities;
using LocalDropshipping.Web.Models;
using LocalDropshipping.Web.Services;
using Microsoft.AspNetCore.Mvc;

namespace LocalDropshipping.Web.Controllers
{
    public class SellerController : Controller
    {
        private readonly IAccountService service;

        public SellerController(IAccountService service)
        {
            this.service = service;
        }
        #region Seller Register and Login
        public IActionResult Register()
        {
            return View();
        }
        //Seller Account login
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await service.LoginUser(model.Email, model.Password);

                if (result.Succeeded)
                {
                    // Redirect to the desired page after successful login
                    return RedirectToAction("ShopPage", "ShopPage");
                }

                ModelState.AddModelError("", "Invalid username or password.");
            }

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Register(SignupViewModel model)
        {
            if (ModelState.IsValid)
            {
                User user = new User()
                {
                    Fullname = string.Join(" ", model.FirstName, model.LastName),
                    Email = model.Email,
                    UserName = model.Email,
                    IsSeller = true,
                };

                var result = await service.RegisterUser(user, model.Password);
                if (!result.Succeeded)
                {
                    foreach (var errorMessage in result.Errors)
                    {
                        ModelState.AddModelError("", errorMessage.Description);
                    }

                    return View(model);
                }

                ModelState.Clear();
                return RedirectToAction("Account", "Login");
            }

            return View(model);
        }
        #endregion

        #region Shop
        public IActionResult Shop()
        {
            return View();
        }
        #endregion

        #region SellerDashboard
        public IActionResult SellerDashboard()
        {
            return View();
        }
        #endregion

        #region Productleftthumbnail
        public IActionResult Productleftthumbnail()
        {

            return View();

        }
        #endregion

        #region Cart
        public IActionResult Cart()
        {
            return View();
        }
        #endregion

        #region Checkout
        public IActionResult Checkout()
        {
            return View();
        }
        #endregion

    }
}
