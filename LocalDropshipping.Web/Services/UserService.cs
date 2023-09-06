using LocalDropshipping.Web.Data;
using LocalDropshipping.Web.Data.Entities;
using LocalDropshipping.Web.Dtos;

namespace LocalDropshipping.Web.Services
{
    public class UserService : IUserService
    {
        private readonly LocalDropshippingContext context;

        public UserService(LocalDropshippingContext context)
        {
            this.context = context;
        }


        public User Add(User user)
        {
            user.IsActive = true;

            context.Users.Add(user);

            context.SaveChanges();
            return user;
        }

        public User Delete(string userId)
        {
            try
            {
                var user = context.Users.Find(userId);
                if (user != null)
                {
                    user.IsDeleted = true;

                    context.SaveChanges();
                    return user;
                }
            }
            catch (Exception ex)
            {
            }
            return null;
        }
        public List<User> GetAllStaffMember()
        {
            return context.Users.Where(x => x.IsActive == true && x.IsDeleted == false && x.IsAdmin == true).ToList();
        }
        public List<User> GetAll()
        {
            return context.Users.Where(x => x.IsActive == true && x.IsAdmin == false && x.IsSuperAdmin == false && x.IsDeleted == false).ToList();
        }

        public User? GetById(string userId)
        {
            return context.Users.FirstOrDefault(c => c.Id == userId);
        }

        public User? Update(string userId, UserDto userDto)
        {
            var user = context.Users.FirstOrDefault(x => x.Id == userId);
            if (user != null)
            {
                user.UserName = userDto.Name;
                user.PhoneNumber = userDto.PhoneNumber;
                context.SaveChanges();
            }
            return user;
        }
    }
}
