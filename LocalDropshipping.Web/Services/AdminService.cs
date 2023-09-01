using LocalDropshipping.Web.Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace LocalDropshipping.Web.Services
{
	public class AdminService : IAdminService
	{
		private readonly SignInManager<User> _signInManager;
		private readonly UserManager<User> userManager;

		public AdminService(SignInManager<User> signInManager,UserManager<User> userManager)
		{
			_signInManager = signInManager;
			this.userManager = userManager;
		}
		async Task<Microsoft.AspNetCore.Identity.SignInResult> IAdminService.AdminLoginUser(string email, string password)
		{
			var result = await _signInManager.PasswordSignInAsync(email, password, false, lockoutOnFailure: false);

			return result;
		}


		public async Task<bool> IsUserAdminAsync(string email)
		{
			var user = await userManager.FindByEmailAsync(email);
			return user != null && user.IsAdmin;
		}
	}
}
