using LocalDropshipping.Web.Data.Entities;
using LocalDropshipping.Web.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;


namespace LocalDropshipping.Web.Attributes
{
    public class CustomAuthorizationFilter : IAuthorizationFilter
    {
        private readonly IUserService _userService;

        public CustomAuthorizationFilter(IUserService userService)
        {
            _userService = userService;
        }
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var user = _userService.GetCurrentUserAsync().GetAwaiter().GetResult();
            if (user != null)
            {
                if (user.IsActive && !user.IsSubscribed && user.IsProfileCompleted)
                {
                    context.Result = new RedirectToActionResult("ReSubscribe", "Seller", null);
                }

                if (!user.IsActive && user.IsSubscribed && user.IsProfileCompleted)
                {
                    context.Result = new RedirectToActionResult("AccountSuspended", "Seller", null);
                }

                if (!user.IsActive && !user.IsSubscribed && user.IsProfileCompleted)
                {
                    context.Result = new RedirectToActionResult("Subscribe", "Seller", null);
                }

                if (!user.IsActive && !user.IsSubscribed && !user.IsProfileCompleted)
                {
                    context.Result = new RedirectToActionResult("ProfileVerification", "Seller", null);
                }
            }
        }

    }

}
