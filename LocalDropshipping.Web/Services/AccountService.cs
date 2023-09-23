using LocalDropshipping.Web.Data.Entities;
using LocalDropshipping.Web.Exceptions;
using LocalDropshipping.Web.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Security.Cryptography.Xml;

namespace LocalDropshipping.Web.Services
{
    public class AccountService : IAccountService
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IEmailService _emailService;
        private readonly IUserService _userService;

        public AccountService(
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            IEmailService emailService,
            IUserService userService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailService = emailService;
            _userService = userService;
        }

        public async Task<IdentityResult> RegisterAsync(User user, string password)
        {
            var result = await _userManager.CreateAsync(user, password);
            if (result.Succeeded)
            {
                string token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                string base64Token = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));

                // TODO: URL Need to be set dynamically
                var verificationLink = $"https://localhost:7153/Seller/EmailVerification?userId={user.Id}&token={base64Token}";
                var emailMessage = new EmailMessage
                {
                    ToEmail = user.Email!,
                    Subject = "Verify your account",
                    TemplatePath = "VerificationEmailTemplate",
                    Placeholders = new List<KeyValuePair<string, string>>()
                };
                emailMessage.Placeholders.Add(new KeyValuePair<string, string>("{{Link}}", verificationLink));

                await _emailService.SendEmail(emailMessage);
            }
            return result;
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
                    user.EmailConfirmed = true;
                    await _userService.UpdateUserAsync(user);
                    isVerified = true;
                }
            }

            return isVerified;
        }
        public async Task<User> LoginAsync(string email, string password, bool rememberMe = false)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                throw new UserNotFoundException("User with specified email doesn't exist");
            }

            var isPasswordCorrect = await _userManager.CheckPasswordAsync(user, password);
            if (!isPasswordCorrect)
            {
                throw new InvalidCredentialException();
            }

            var result = await _signInManager.PasswordSignInAsync(email, password, isPersistent: rememberMe, lockoutOnFailure: true);
            return user;
        }

        public async Task<bool> SendContactEmailAsync(ContactUsViewModel contactUsViewModel)
        {

            var emailMessage = new EmailMessage
            {
                ToEmail = contactUsViewModel.EmailAddress,
                Subject = "Contact Us Form Query",
                TemplatePath = "ContactUsTemplate",
                Placeholders = new List<KeyValuePair<string, string>>
                {

                    new KeyValuePair<string, string>("{{Name}}", contactUsViewModel.FullName),
                    new KeyValuePair<string, string>("{{EmailAddress}}", contactUsViewModel.EmailAddress),
                    new KeyValuePair<string, string>("{{PhoneNumber}}", contactUsViewModel.PhoneNumber),
                    new KeyValuePair<string, string>("{{Message}}", contactUsViewModel.Message)
                 }
            };
            await _emailService.SendEmail(emailMessage);
            return true;
        }

        public async Task<bool> ForgotPasswordAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user != null)
            {
                var token = await _userManager.GeneratePasswordResetTokenAsync(user);

                var base64Token = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));
                var verificationLink = $"https://localhost:7153/Seller/UpdatePassword?userId={user.Id}&token={base64Token}";

                var emailMessage = new EmailMessage
                {
                    ToEmail = email,
                    Subject = "ForgotPassword",
                    TemplatePath = "ForgotPasswordTemplate",
                    Placeholders = new List<KeyValuePair<string, string>>
                    {
                        new KeyValuePair<string, string>("{{Link}}", verificationLink),
                    }
                };
                await _emailService.SendEmail(emailMessage);

                return true;
            }

            return false; // Email not found in the database
        }

        //private string GenerateUpdatePasswordToken(string email)
        //{
        //    var user = _userManager.FindByEmailAsync(email);
        //    var token = _userManager.GeneratePasswordResetTokenAsync(user);

        //    var base64Token = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));
        //    return base64Token;
        //}


        public async Task<bool> UpdatePasswordAsync(NewPasswordViewModel model)
        {
            var isUpdated = false;

            if (string.IsNullOrEmpty(model.UserId) || string.IsNullOrEmpty(model.Token) || string.IsNullOrEmpty(model.Password))
            {
                return isUpdated;
            }


            var user = await _userManager.FindByIdAsync(model.UserId);
            if (user == null)
            {
                return isUpdated;
            }
            var decodedToken = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(model.Token));

            var result = await _userManager.ResetPasswordAsync(user, decodedToken, model.Password);
            if (result.Succeeded)
            {
                isUpdated = true;
            }
            return isUpdated;
        }


        public async Task<string> GeneratePasswordUpdateToken(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);

            if (user != null)
            {
                var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                return token;
            }

            return null;
        }

        public async Task<bool> UpdatePassword(string userId, string token, string newPassword)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user != null)
            {
                var result = await _userManager.ResetPasswordAsync(user, token, newPassword);

                if (result.Succeeded)
                {
                    return true;
                }
            }
            return false;
        }

        public ChallengeResult GoogleSignin(string redirectUrl)
        {
            var properties = _signInManager.ConfigureExternalAuthenticationProperties("Google", redirectUrl);
            return new ChallengeResult("Google", properties);
        }

        public async Task<bool> ExternalLoginAsync()
        {
            var info = await _signInManager.GetExternalLoginInfoAsync();

            if (info != null)
            {
                var result = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, false);
                if (result.Succeeded)
                {
                    await _signInManager.UpdateExternalAuthenticationTokensAsync(info);
                    return true;
                }
                else
                {
                    User user = new User
                    {
                        Email = info.Principal.FindFirstValue(ClaimTypes.Email),
                        UserName = info.Principal.FindFirstValue(ClaimTypes.Email),
                        Fullname = info.Principal.FindFirstValue(ClaimTypes.Name),
                        IsSeller = true,
                    };

                    var res = await _userManager.CreateAsync(user);
                    if (res.Succeeded)
                    {
                        res = await _userManager.AddLoginAsync(user, info);
                        var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                        await _userManager.ConfirmEmailAsync(user, token);
                        if (res.Succeeded)
                        {
                            await _signInManager.SignInAsync(user, isPersistent: false);
                            await _signInManager.UpdateExternalAuthenticationTokensAsync(info);
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        public async Task LogoutAsync()
        {
            await _signInManager.SignOutAsync();
            return;
        }
        // TODO: External Login Facebook(zubair)
    }
}