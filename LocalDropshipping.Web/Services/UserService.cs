using LocalDropshipping.Web.Data.Entities;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace LocalDropshipping.Web.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<User> _userManager;

        public UserService(UserManager<User> userManager)
        {
            this._userManager = userManager;
        }

        public async Task<User?> GetUserAsync(ClaimsPrincipal userClaimsPrincipal)
        {
            var user = await _userManager.GetUserAsync(userClaimsPrincipal);
            return user;
        }
    }
}
