using LocalDropshipping.Web.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace LocalDropshipping.Web.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class AuthorizeOnlyAttribute : Attribute, IAuthorizationFilter
    {
        private readonly string _redirectToAction;
        private readonly string _redirectToController;
        private readonly Roles _allowedUserRoles;

        public AuthorizeOnlyAttribute(Roles roles, string redirectToAction = "AuthorizationFailed", string redirectToController = "Public")
        {
            _redirectToAction = redirectToAction;
            _redirectToController = redirectToController;
            _allowedUserRoles = roles;
        }


        public void OnAuthorization(AuthorizationFilterContext context)
        {
            if (context.HttpContext.Items.ContainsKey("IsSignedIn") )
            {
                if (Convert.ToBoolean(context.HttpContext.Items["IsSignedIn"].ToString()) && context.HttpContext.Items.ContainsKey("CurrentUserRoles"))
                {
                    var currentUserRoles = (List<Roles>)context.HttpContext.Items["CurrentUserRoles"]!;

                    if ((_allowedUserRoles & Roles.Admin) != 0 && currentUserRoles.Contains(Roles.Admin))
                    {
                        return;
                    }
                    if ((_allowedUserRoles & Roles.SuperAdmin) != 0 && currentUserRoles.Contains(Roles.SuperAdmin))
                    {
                        return;
                    }
                    if ((_allowedUserRoles & Roles.Seller) != 0 && currentUserRoles.Contains(Roles.Seller))
                    {
                        return;
                    }
                }
            }
            context.Result = new RedirectToActionResult(_redirectToAction, _redirectToController, new { returnUrl = context.HttpContext.Request.Path });
        }
    }
}
