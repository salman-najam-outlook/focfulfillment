using LocalDropshipping.Web.Enums;
using LocalDropshipping.Web.Models;
using LocalDropshipping.Web.Data.Entities;
using Microsoft.AspNetCore.Identity;

namespace LocalDropshipping.Web.Services
{
    public interface IAccountService
    {
        Task<bool> ConfirmEmailAsync(string userId, string base64Token);
        Task<bool> ForgotPasswordAsync(string email);
        Task<bool> UpdatePasswordAsync(NewPasswordViewModel model);
        Task<User> LoginAsync(string email, string password, bool rememberMe = false);
        Task<IdentityResult> RegisterAsync(User user, string password);
    }
}