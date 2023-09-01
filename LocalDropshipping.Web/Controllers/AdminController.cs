using LocalDropshipping.Web.Data.Entities;
using LocalDropshipping.Web.Models;
using LocalDropshipping.Web.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace LocalDropshipping.Web.Controllers
{
	public class AdminController : Controller
	{

		private readonly IAdminService service;

		public AdminController(IAdminService service)
		{
			this.service = service;
		}


		public IActionResult AdminLogin()
		{
			return View();
		}
		[HttpPost]
		public async Task<IActionResult> AdminLogin(AdminLoginViewModel model)
		{
			if (ModelState.IsValid)
			{
				var result = await service.AdminLoginUser(model.Email, model.Password);

				if (result.Succeeded)
				{
					// Check if the user is an admin
					var isAdmin = await service.IsUserAdminAsync(model.Email);

					if (isAdmin)
					{
						// Redirect to the admin dashboard if the user is an admin
						return RedirectToAction("us7yhs6tdgv", "Admin");
					}
					else
					{
						// Handle non-admin user here, e.g., redirect to a different page
						ModelState.AddModelError("", "OOPs. its seem like you're not an admin");
					//	return RedirectToAction("NonAdminPage");
					}
				}

				ModelState.AddModelError("", "Invalid username or password.");
			}

			return View(model);
		}


		public IActionResult us7yhs6tdgv()
		{
			return View();
		}
		public IActionResult Index()
		{
			return View();
		}
	}
}
