using LocalDropshipping.Web.Data.Entities;
using Microsoft.AspNetCore.Identity;

namespace LocalDropshipping.Web.Services
{
    public interface IAccountService
    {
        Task<IdentityResult> RegisterUser(User user, string password);
        Task<SignInResult> LoginUser(string email, string password);




	}
}