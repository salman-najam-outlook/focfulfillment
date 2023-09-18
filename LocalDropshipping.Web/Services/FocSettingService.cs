using LocalDropshipping.Web.Data;

namespace LocalDropshipping.Web.Services
{
    public class FocSettingService : IFocSettingService
    {
        private readonly LocalDropshippingContext _context;

        public FocSettingService(LocalDropshippingContext context)
        {
            _context = context;
        }
        public string GetShippingCost(string cost)
        {
            return _context.FocSettings.FirstOrDefault(x=>x.Name == cost).Value;
        }
    }
}
