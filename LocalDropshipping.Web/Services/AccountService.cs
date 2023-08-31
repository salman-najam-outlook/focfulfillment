using Microsoft.AspNetCore.Identity;
using LocalDropshipping.Web.Data.Entities;

namespace LocalDropshipping.Web.Services
{
    public class AccountService : IAccountService
    {
        private readonly UserManager<User> _userManager;
        public AccountService(UserManager<User> userManager)
        {
            _userManager = userManager;
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

    }
}
