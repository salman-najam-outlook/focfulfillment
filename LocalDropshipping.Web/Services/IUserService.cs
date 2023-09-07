using LocalDropshipping.Web.Data.Entities;
using System.Security.Claims;

namespace LocalDropshipping.Web.Services
{
    public interface IUserService
    {
        Task<User?> GetUserAsync(ClaimsPrincipal userClaimsPrincipal);
    }
}