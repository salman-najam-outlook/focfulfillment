using Microsoft.AspNetCore.Mvc;

namespace LocalDropshipping.Web.Controllers
{
    public class ShopPageController : Controller
    {
        public IActionResult ShopPage()
        {
            return View();
        }
    }
}
