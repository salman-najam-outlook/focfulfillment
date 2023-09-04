using Microsoft.AspNetCore.Mvc;

namespace LocalDropshipping.Web.Controllers
{
    public class SellerController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
