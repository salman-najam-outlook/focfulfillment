using LocalDropshipping.Web.Data.Entities;
using LocalDropshipping.Web.Dtos;
using LocalDropshipping.Web.Models;
using LocalDropshipping.Web.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace LocalDropshipping.Web.Controllers
{
    public class SellerController : Controller
    {
        private readonly IProfilesService service;

        public SellerController(IProfilesService service)
        {
            this.service = service;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult ProfileVerification()
        {
            return View();
        }

        [HttpPost]
        public IActionResult ProfileVerification(ProfileVerificationViewModel profileVerificationViewModel)
        {
            if (ModelState.IsValid == false)
            {
                //ModelState.AddModelError("", "Please enter a valid data.");
                return View(profileVerificationViewModel);
            }
            Profiles profile = profileVerificationViewModel.ToEntity();
            service.Add(profile);
            return RedirectToAction("Shop", "Shop");
        }



    }
}
