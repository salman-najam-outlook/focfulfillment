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
		//[HttpPost]
		//public async Task<IActionResult> AdminLogin(AdminLoginViewModel model)
		//{
		//	if (ModelState.IsValid)
		//	{
		//		var result = await service.AdminLoginUser(model.Email, model.Password);

		//		if (result.Succeeded)
		//		{
		//			// Check if the user is an admin
		//			var isAdmin = await service.IsUserAdminAsync(model.Email);

		//			if (isAdmin)
		//			{
		//				// Redirect to the admin dashboard if the user is an admin
		//				return RedirectToAction("us7yhs6tdgv", "Admin");
		//			}
		//			else
		//			{
		//				// Handle non-admin user here, e.g., redirect to a different page
		//				ModelState.AddModelError("", "OOPs. its seem like you're not an admin");
		//				//	return RedirectToAction("NonAdminPage");
		//			}
		//		}

		//		ModelState.AddModelError("", "Invalid username or password.");
		//	}

		//	return View(model);
		//}

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

					// Check if the user is superadmin
					var isSuperAdmin = await service.IsUserSuperAdminAsync(model.Email);

					// Check if the user is active
					var isActive = await service.IsUserActiveAsync(model.Email);

					if (isAdmin)// || isSuperAdmin
					{
						if (isActive)
						{
							// Redirect to the admin dashboard if the user is an admin active
							return RedirectToAction("us7yhs6tdgv", "Admin");
						}
						else
						{
							// Handle inactive admin/superadmin
							ModelState.AddModelError("", "Your account is not active.");
						}
					}
					else
					{
						// Handle non-admin user here, e.g., redirect to a different page
						ModelState.AddModelError("", "Oops, it seems like you're not an admin.");
					}
				}
				else
				{
					ModelState.AddModelError("", "Invalid username or password.");
				}
			}

			return View(model);
		}


		public ActionResult us7yhs6tdgv()
		{
			return View();
		}
		public ActionResult Index()
		{
			return View();
		}
	}
}
