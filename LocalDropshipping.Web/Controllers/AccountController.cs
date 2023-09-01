using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using LocalDropshipping.Web.Data.Entities;
using LocalDropshipping.Web.Models;
using LocalDropshipping.Web.Services;

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
                return RedirectToAction("Index", "Home");
            }

            return View(model);
        }
    }
}
