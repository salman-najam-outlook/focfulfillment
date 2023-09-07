using LocalDropshipping.Web.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace LocalDropshipping.Web.Controllers
{
    public class PublicController : Controller
    {
        #region About Us
        public IActionResult AboutUs()
        {
            return View();
        }
        #endregion

        #region Contact Us
        public IActionResult ContactUs()
        {
            return View();
        }
        #endregion

        #region Terms and Conditions
        public IActionResult TermAndCondition()
        {
            return View();
        }
        #endregion

        #region PrivacyPolicy
        public IActionResult PrivacyPolicy()
        {
            return View();
        }
        #endregion

        #region MyRegion
        public IActionResult Faq()
        {
            return View();
        }
        #endregion

        #region Error
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        #endregion


    }
}
