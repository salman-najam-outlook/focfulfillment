using Microsoft.AspNetCore.Identity;
using LocalDropshipping.Web.Data.Entities;

namespace LocalDropshipping.Web.Services
{
    public class AccountService : IAccountService
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public AccountService(UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public async Task<IdentityResult> RegisterUser(User user, string password)
        {
            var result = await _userManager.CreateAsync(user, password);
            if (result.Succeeded)
            {
                // Email Confirmation Code
            }
            return result;
        }



        public async Task<SignInResult> LoginUser(string email, string password)
        {
            var user = await _userManager.FindByEmailAsync(email);

            //var user = await _userManager.FindByEmailAsync(email);

            if (user != null && user.IsActive && !user.IsAdmin && !user.IsSuperAdmin)
            {
                var result = await _signInManager.PasswordSignInAsync(user, password, false, lockoutOnFailure: false);

                return result;
            }

            return SignInResult.Failed; // User not found
        }

		
	}
}
