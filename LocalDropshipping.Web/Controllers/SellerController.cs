using LocalDropshipping.Web.Attributes;
using LocalDropshipping.Web.Data.Entities;
using LocalDropshipping.Web.Exceptions;
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
        private readonly IProfilesService _profileService;
        private readonly IUserService _userService;
        private readonly IAccountService _accountService;

        public SellerController(IAccountService accountService, IProfilesService profileService, IUserService userService)
        {
            _accountService = accountService;
            _profileService = profileService;
            _userService = userService;
        }

        public IActionResult Register()
        {
            string? isEmailSent = HttpContext.Session.GetString("VerificationEmailSent");
            if (isEmailSent != null && isEmailSent == "True")
                return RedirectToAction("VerificationEmailSent", "Seller");
            return View();
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

        public IActionResult VerificationEmailSent()
        {
            return View();
        }

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

        public IActionResult EmailVerified()
        {
            return View();
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

        public IActionResult Subscribe()
        {
            var user = _userService.GetCurrentUserAsync().GetAwaiter().GetResult();
            if (user != null && user.IsSubscribed && user.IsActive)
                return RedirectToAction("Shop", "Seller");
            return View();
        }

        public IActionResult Login()
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

        [Authorize]
        [TypeFilter(typeof(CustomAuthorizationFilter))]
        public IActionResult Shop()
        {
            if (!_userService.IsUserSignedIn())
                return RedirectToAction("Register", "Seller");

            return View();
        }

        public IActionResult SellerDashboard()
        {
            return View();
        }

        public IActionResult Productleftthumbnail()
        {

            return View();

        }

        public IActionResult Cart()
        {
            return View();
        }

        public IActionResult Checkout()
        {
            return View();
        }
    }
}
