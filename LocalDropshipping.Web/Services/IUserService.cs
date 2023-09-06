using LocalDropshipping.Web.Data.Entities;
using LocalDropshipping.Web.Dtos;

namespace LocalDropshipping.Web.Services
{
    public interface IUserService
    {
        User? Add(User user);

        List<User?> GetAll();
        List<User?> GetAllStaffMember();

        User? GetById(string userId);

        User? Delete(string userId);
        User? Update(string userId, UserDto userDto);
    }
}
