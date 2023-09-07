using LocalDropshipping.Web.Data.Entities;

namespace LocalDropshipping.Web.Services
{
    public interface IProfilesService
    {
        Profiles? Add(Profiles profile);
        List<Profiles?> GetAllProfiles();
        Profiles? GetProfileById(int id);
    }
}
