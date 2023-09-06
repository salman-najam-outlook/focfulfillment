using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using LocalDropshipping.Web.Data.Entities;
using LocalDropshipping.Web.Models;
using LocalDropshipping.Web.Services;
using Microsoft.AspNetCore.Authorization;

namespace LocalDropshipping.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAccountService service;

        public AccountController(IAccountService service)
        {
            this.service = service;
        }

        public IActionResult Register()
        {
            return View();
        }

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
                var isLoggedIn = await service.LoginAsync(model.Email, model.Password);

                if (isLoggedIn)
                {
                    // Redirect to the desired page after successful login
                    return RedirectToAction("Shop", "Shop");
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
                var isRegistered = await service.RegisterAsync(model.Email, model.Password, string.Join(" ", model.FirstName, model.LastName));
                if (!isRegistered)
                {
                    ModelState.AddModelError("", "Unknow error occured");
                    return View(model);
                }

                ModelState.Clear();
                return RedirectToAction("Account", "Login");
            }

            return View(model);
        }
    }
}
