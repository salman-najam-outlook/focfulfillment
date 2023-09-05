using Microsoft.AspNetCore.Identity;
using LocalDropshipping.Web.Data.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using System.Text;
using System.Linq.Expressions;
using LocalDropshipping.Web.Models;
using Microsoft.EntityFrameworkCore.Metadata;
using LocalDropshipping.Web.Enums;
using System.Data;

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

        public async Task<bool> RegisterAsync(string email, string password, string? fullname = "", string? username = "")
        {
            var isCreated = false;

            User user = new User
            {
                UserName = username,
                Fullname = fullname,
                Email = email
            };

            var result = await _userManager.CreateAsync(user, password);
            if (result.Succeeded)
            {
                string token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                string base64Token = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));
                // TODO: Send Confirmation Email(usama) 

                isCreated = true;
            }
            return isCreated;
        }
        public async Task<bool> ConfirmEmailAsync(string userId, string base64Token)
        {
            var isVerified = false;

            var user = await _userManager.FindByIdAsync(userId);
            if (user != null)
            {
                var token = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(base64Token));
                var result = await _userManager.ConfirmEmailAsync(user, token);
                if (result.Succeeded)
                {
                    isVerified = true;
                }
            }

            return isVerified;
        }
        public async Task<bool> LoginAsync(string email, string password)
        {
            var result = await _signInManager.PasswordSignInAsync(email, password, isPersistent: false, lockoutOnFailure: true);
            if (result.Succeeded)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        // TODO: Forget Password(usama)

        // TODO: Reset Password(usama)

        // TODO: External Login Google(zubair)

        // TODO: External Login Facebook(zubair)


        // Helper Methods
        public async Task<bool> IsInRole(string email, Roles role)
        {
            var isInRole = false;

            var user = await _userManager.FindByEmailAsync(email);
            if (user != null)
            {
                isInRole = await _userManager.IsInRoleAsync(user, role.ToString());
            }

            return isInRole;
        }
        public async Task<bool> IsEmailExist(string email)
        {
            var isEmailExist = false;

            var user = await _userManager.FindByEmailAsync(email);
            if (user != null)
            {
                isEmailExist = true;
            }

            return isEmailExist;
        }
    }
}
