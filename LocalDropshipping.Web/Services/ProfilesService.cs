using LocalDropshipping.Web.Data;
using LocalDropshipping.Web.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace LocalDropshipping.Web.Services
{
    public class ProfilesService : IProfilesService
    {
        private readonly LocalDropshippingContext context;

        public ProfilesService(LocalDropshippingContext context)
        {
            this.context = context;
        }

        public Profiles Add(Profiles profile)
        {
            if (context.Profiles.Any(x => x.UserId == profile.UserId))
            {
                throw new Exception("User profile already exist");
            }
            else
            {
                context.Profiles.Add(profile);
                context.SaveChanges();
                return profile;
            }
        }

        public List<Profiles> GetAllProfiles()
        {
            return context.Profiles.Include(x => x.User).ToList();
        }

        //public Profiles? GetProfileById(string id)
        //{
        //    return context.Profiles.FirstOrDefault(x => x.ProfileId == id);
        //}
        public Profiles? GetProfileById(string id)
        {
            return context.Profiles.FirstOrDefault(x => x.UserId == id);
        }
    }
}
