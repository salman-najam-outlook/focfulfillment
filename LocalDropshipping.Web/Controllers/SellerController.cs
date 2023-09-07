using System.Text;
using LocalDropshipping.Web.Data.Entities;
using LocalDropshipping.Web.Models;
using LocalDropshipping.Web.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;

namespace LocalDropshipping.Web.Controllers
{
    public class SellerController : Controller
    {
        private readonly IAccountService service;
        private readonly UserManager<User> _userManager;
        private readonly IAccountService accountService;

        public SellerController(IAccountService service, UserManager<User> userManager,IAccountService accountService)
        {
            this.service = service;
            _userManager = userManager;
            this.accountService = accountService;
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
        public IActionResult ForgotPassword()
        {
            return View();
        }
        public IActionResult UpdatePassword()
        {
            return View();
        }
        public IActionResult UserPasswordMassege()
        {
            return View();
        }
        public IActionResult Index1()
        {
            return View();
        }
        public IActionResult Index2()
        {
            return View();
        }
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var isLoggedin = await service.LoginAsync(model.Email, model.Password);

                if (isLoggedin)
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
				var isSucceeded = await service.RegisterAsync(model.Email, model.Password, string.Join(" ", model.FirstName, model.LastName));
				if (isSucceeded)
				{
					TempData["RegistrationConfirmation"] = "Registration was successful. You can now log in.";
					return RedirectToAction("Index1");
				}
				else
				{
					ModelState.AddModelError("", "Unknown error occurred");
				}
			}

			return View(model);
        }
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdatePassword(string userId, string token, string newPassword)
        {
            var isUpdated = await service.UpdatePasswordAsync(userId, token, newPassword);

            if (isUpdated)
            {
                return RedirectToAction("PasswordUpdated");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Password update failed.");
                return View("UpdatePassword");
            }
        }
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> VerifyEmail(string userId, string token)
        {
            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(token))
            {
                // Handle invalid or missing parameters
                return RedirectToAction("InvalidVerificationLinkk");
            }
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                // Handle user not found
                return RedirectToAction("InvalidVerificationLinke");
            }
            // Decode the token
            var decodedToken = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(token));

            // Confirm the email
            var result = await _userManager.ConfirmEmailAsync(user, decodedToken);
            if (result.Succeeded)
            {
                // Email is confirmed; set EmailConfirmed to true and update the user in the database
                user.EmailConfirmed = true;
                await _userManager.UpdateAsync(user);
                return RedirectToAction("Index2");
            }
            else
            {
                // Handle verification failure
                return RedirectToAction("InvalidVerificationLink");
            }
        }
        [HttpPost]
        public async Task<IActionResult> ForgotPassword(string email)
        {
            var isPasswordResetLinkSent = await service.ForgotPasswordAsync(email);

            if (isPasswordResetLinkSent)
            {
                TempData["RegistrationConfirmation"] = "Registration was successful. You can now log in.";

                return RedirectToAction("UserPasswordMassege");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Email not found.");
                return View("ForgotPassword"); 
            }
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
