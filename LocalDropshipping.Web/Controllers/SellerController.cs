using LocalDropshipping.Web.Data.Entities;
using LocalDropshipping.Web.Models;
using LocalDropshipping.Web.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using System.Text;

namespace LocalDropshipping.Web.Controllers
{
    public class SellerController : Controller
    {
        private readonly IAccountService service;
        private readonly UserManager<User> _userManager;
        private readonly IAccountService accountService;

        public SellerController(IAccountService service, UserManager<User> userManager, IAccountService accountService)
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

        public IActionResult Login()
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
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var user = await service.LoginAsync(model.Email, model.Password);
                    if (!user.IsProfileCompleted)
                    {
                        return RedirectToAction("ProfileVerification", "Seller");
                    }
                    else if (!user.IsSubscribed)
                    {
                        return RedirectToAction("Subscribe", "Seller");
                    }
                    else
                    {
                        return RedirectToAction("Shop", "Seller");
                    }
                }
            }
            catch (Exception ex)
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

                var result = await service.RegisterAsync(user, model.Password);
                if (result.Succeeded)
                {
                    TempData["RegistrationConfirmation"] = "Registration was successful. You can now log in.";
                    return RedirectToAction("Index1");
                }
                else
                {
                    // TODO: Error messages of identity must not be shown to any user
                    // Currently doing because its a development dependency
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("CustomErrorMessage", string.Join(": ", error.Code, error.Description));
                    }
                }
            }
            return View(model);
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

                // Redirect to a success page
                return RedirectToAction("Index2");
            }
            else
            {
                // Handle verification failure
                return RedirectToAction("InvalidVerificationLink");
            }
        }
        #endregion

        #region Shop
        [Authorize]
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
