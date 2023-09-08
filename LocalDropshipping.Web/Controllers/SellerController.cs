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
        private readonly IProfilesService _profileService;
        private readonly IUserService _userService;
        private readonly IAccountService service;
        private readonly UserManager<User> _userManager;
        private readonly IAccountService accountService;

        public SellerController(IAccountService service, UserManager<User> userManager, IAccountService accountService, IProfilesService profileService, IUserService userService)
        {
            this.service = service;
            _userManager = userManager;
            this.accountService = accountService;
            _profileService = profileService;
            this._userService = userService;
        }

        #region Seller Register and Login
        public IActionResult Register()
        {
            return View();
        }

        public async Task<IActionResult> LoginAsync()
        {
            if (await _userService.IsUserSignedIn())
            {
                var user = (await _userService.GetCurrentUserAsync())!;
                return RedirectToCorrespondingPage(user);
            }
            return View();
        }
        public IActionResult ForgotPassword()
        {
            return View();
        }
        public IActionResult UpdatePassword(string userId ,string token)
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
                    return RedirectToCorrespondingPage(user);
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
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("CustomErrorMessage", string.Join(": ", error.Code, error.Description));
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
            var isUpdated = await service.UpdatePasswordAsync(model);

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
                TempData["ForgotPassword"] = "Password Forgot successful. You can now log in.";

                return RedirectToAction("ForgotPasswordMessage");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Email not found.");
                return View("ForgotPassword"); 
            }
        }

        [Authorize]
        public async Task<IActionResult> Subscribe()
        {
            return RedirectToCorrespondingPage((await _userService.GetCurrentUserAsync())!);
        }
        #endregion

        #region Shop
        [Authorize]
        public async Task<IActionResult> Shop()
        {
            var user = (await _userService.GetCurrentUserAsync())!;
            if (user.IsSubscribed && user.IsActive)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Login", "Seller");
            }
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

        [Authorize]
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
        #endregion


        private IActionResult RedirectToCorrespondingPage(User user)
        {
            if (!user.IsProfileCompleted && user.IsActive)
            {
                return RedirectToAction("ProfileVerification", "Seller");
            }

            if (!user.IsSubscribed && user.IsActive)
            {
                return RedirectToAction("Subscribe", "Seller");
            }

            if (user.IsSubscribed && !user.IsActive)
            {
                return RedirectToAction("AccountSuspended", "Seller");
            }

            return RedirectToAction("Shop", "Seller");

        }

    }
}
