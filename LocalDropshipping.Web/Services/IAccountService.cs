﻿using LocalDropshipping.Web.Enums;
using LocalDropshipping.Web.Models;

namespace LocalDropshipping.Web.Services
{
    public interface IAccountService
    {
        Task<bool> ConfirmEmailAsync(string userId, string base64Token);
        Task<bool> IsEmailExist(string email);
        Task<bool> IsInRole(string email, Roles role);
        Task<bool> LoginAsync(string email, string password);
        Task<bool> RegisterAsync(string email, string password, string? fullname = "", string? username = "", string scheme = "http", string host = "example.com");
        Task<bool> ForgotPasswordAsync(string email);
        Task<bool> UpdatePasswordAsync(NewPasswordViewModel model);


    }
}