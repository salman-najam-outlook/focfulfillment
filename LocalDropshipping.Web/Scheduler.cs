using LocalDropshipping.Web.Data;
using LocalDropshipping.Web.Data.Entities;
using LocalDropshipping.Web.Enums;
using LocalDropshipping.Web.Services;
using System.Linq;

namespace LocalDropshipping.Web
{
    public static class Scheduler
    {
        private readonly static LocalDropshippingContext _context;
        private readonly static List<Subscription> subscriptions = new List<Subscription>();
        public static List<Subscription> SchedularTask()
        {
            return _context.Subscriptions.ToList();
        }
    }
}
