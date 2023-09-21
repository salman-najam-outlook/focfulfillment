using LocalDropshipping.Web.Data.Entities;
using Microsoft.AspNetCore.Identity;

namespace LocalDropshipping.Web.Services
{
    public interface IAdminService
    {
		Task<SignInResult> AdminLoginUser(string email, string password);
		Task<bool> IsUserAdminAsync(string email);


		Task<bool> IsUserSuperAdminAsync(string email);
		Task<bool> IsUserActiveAsync(string email);
        Task<User> GetUserByEmail(string email);
    }
}
