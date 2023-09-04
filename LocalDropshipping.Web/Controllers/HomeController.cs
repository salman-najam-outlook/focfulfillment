using LocalDropshipping.Web.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace LocalDropshipping.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }


        // UseFull links 
        public IActionResult AboutUs()
        {
            return View();
        }

        public IActionResult ContactUs()
        {
            return View();
        }

        public IActionResult TermAndCondition()
        {
            return View();
        }

        public IActionResult PrivacyPolicy()
        {
            return View();
        }

        public IActionResult Faq()
        {
            return View();
        }

        // UseFull links 

        public IActionResult Dashboard()
        {
            return View();
        }
        public IActionResult SearchProducts()
        {
            return View();
        } 
        public IActionResult Support()
        {
            return View();
        }

        public IActionResult AnnouncementsforMembers()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

    }
}