using LocalDropshipping.Web.Data;
using LocalDropshipping.Web.Data.Entities;
using LocalDropshipping.Web.Exceptions;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace LocalDropshipping.Web.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly LocalDropshippingContext _context;

        public UserService(UserManager<User> userManager, SignInManager<User> signInManager, LocalDropshippingContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
        }

        public async Task<User?> GetCurrentUserAsync()
        {
            var user = await _userManager.GetUserAsync(_signInManager.Context.User);
            return user;
        }

        public async Task<bool> IsUserSignedIn()
        {
            return _signInManager.IsSignedIn(_signInManager.Context.User);
        }

        public async Task UpdateUserAsync(User user)
        {
            var existingUser = _context.Users.FirstOrDefault(x => x.Id == user.Id);
            if (existingUser != null)
            {
                existingUser.Fullname = user.Fullname;
                existingUser.IsSeller = user.IsSeller;
                existingUser.IsAdmin = user.IsAdmin;
                existingUser.IsSuperAdmin = user.IsSuperAdmin;
                existingUser.IsActive = user.IsActive;
                existingUser.IsDeleted = user.IsDeleted;
                existingUser.IsSubscribed = user.IsSubscribed;
                existingUser.EmailConfirmed = user.EmailConfirmed;
                existingUser.PhoneNumber = user.PhoneNumber;
                existingUser.PhoneNumberConfirmed = user.PhoneNumberConfirmed;
                existingUser.IsProfileCompleted = user.IsProfileCompleted;
                await _context.SaveChangesAsync();
            }
            else
            {
                throw new UserNotFoundException("User not found");
            }
        }
    }
}
