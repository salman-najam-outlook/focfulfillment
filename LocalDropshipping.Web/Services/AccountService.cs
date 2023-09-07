using LocalDropshipping.Web.Data.Entities;
using LocalDropshipping.Web.Exceptions;
using LocalDropshipping.Web.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using System.Text;

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

        // TODO: Forget Password(usama)

        // TODO: Reset Password(usama)

        // TODO: External Login Google(zubair)

        // TODO: External Login Facebook(zubair)
    }
}
