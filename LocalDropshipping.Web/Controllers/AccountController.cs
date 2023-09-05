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
		private readonly IEmailService emailService;

		public AccountController(IAccountService service, IEmailService emailService)
        {
            this.service = service;
			this.emailService = emailService;
		}

        public async Task<ViewResult> Index() 
        {
            UserEmailOptions options = new UserEmailOptions
            {
                ToEmail = "usamahaseeb777@gmail.com",
                Placeholders = new List<KeyValuePair<string,string> >() 
                {
                    new KeyValuePair<string,string>("{{UserName}}","Usama")
                }
            };
            await emailService.SendTestEmail(options);
			return View();

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
    }
}
