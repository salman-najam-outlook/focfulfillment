using LocalDropshipping.Web.Data;
using LocalDropshipping.Web.Data.Entities;
using LocalDropshipping.Web.Exceptions;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using LocalDropshipping.Web.Dtos;
using Microsoft.EntityFrameworkCore;

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
        public async Task<bool> IsCurrentUserAdminAsync(ClaimsPrincipal user)
        {
            if (user == null)
            {
                return false; // User is not logged in
            }

            var currentUser = await _userManager.GetUserAsync(user);

            if (currentUser == null)
            {
                return false; // User not found
            }

            return currentUser.IsAdmin;
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

        public User Add(User user)
        {
            user.IsActive = true;

            _context.Users.Add(user);

            return user;
        }

        public User Delete(string userId)
        {
            try
            {
                var user = _context.Users.Find(userId);
                if (user != null)
                {
                    user.IsDeleted = true;

                    _context.SaveChanges();
                    return user;
                }
            }
            catch (Exception ex)
            {

            }
            return null;
        }

        public bool DisableUser(string userId)
        {
            try
            {
                var user = _context.Users.Find(userId);
                if (user != null)
                {
                    user.IsActive = false;
                    _context.SaveChanges();
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public bool? ActivateUser(string userId)
        {
            try
            {
                var user = _context.Users.Find(userId);
                if (user != null && user.IsActive == false)
                {

                    user.IsActive = true;

                    _context.SaveChanges();
                }
                else
                {
                    user.IsActive = false;
                    _context.SaveChanges();
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }

        }

        public List<User> GetAllStaffMember()
        {
            return _context.Users.Where(x => x.IsAdmin == true && x.IsDeleted == false).ToList();
        }

        public List<User> GetAll()
        {
            return _context.Users.Where(x => x.IsAdmin == false && x.IsSuperAdmin == false && x.IsDeleted == false).ToList();
        }

        public User? GetById(string userId)
        {
            return _context.Users.FirstOrDefault(c => c.Id == userId);
        }

        public User? Update(string userId, UserDto userDto)
        {
            var user = _context.Users.FirstOrDefault(x => x.Id == userId);
            if (user != null)
            {
                user.UserName = userDto.Name;
                user.PhoneNumber = userDto.PhoneNumber;
                _context.SaveChanges();
            }
            return user;
        }


    }
}
