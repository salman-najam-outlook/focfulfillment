using Microsoft.AspNetCore.Mvc;

namespace LocalDropshipping.Web.Controllers
{
    public class ShopController : Controller
    {
        public IActionResult Shop()
        {
            return View();
        }
    }
}
