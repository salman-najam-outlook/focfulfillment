using LocalDropshipping.Web.Attributes;
using LocalDropshipping.Web.Data.Entities;
using LocalDropshipping.Web.Exceptions;
using LocalDropshipping.Web.Models;
using LocalDropshipping.Web.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System;
using System.Text;

namespace LocalDropshipping.Web.Controllers
{
    public class SellerController : Controller
    {
        private readonly IProfilesService _profileService;
        private readonly IUserService _userService;
        private readonly IAccountService _accountService;
        private readonly IOrderService _orderService;

        public SellerController(IAccountService accountService, IProfilesService profileService, IUserService userService, IOrderService orderService)
        {
            _accountService = accountService;
            _profileService = profileService;
            _userService = userService;
            _orderService = orderService;
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
            if (!_userService.IsUserSignedIn())
                return RedirectToAction("Register", "Seller");

            return View();
        }

        public IActionResult Withdrawal()
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
    }
}
