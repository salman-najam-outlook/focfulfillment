using LocalDropshipping.Web.Data.Entities;
using System.Security.Claims;

namespace LocalDropshipping.Web.Services
{
    public interface IUserService
    {
        Task<User?> GetCurrentUserAsync();
        bool IsUserSignedIn();
        Task UpdateUserAsync(User user);
    }
}