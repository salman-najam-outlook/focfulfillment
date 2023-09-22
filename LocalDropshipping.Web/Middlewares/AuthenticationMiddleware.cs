using LocalDropshipping.Web.Enums;
using LocalDropshipping.Web.Services;
using Newtonsoft.Json;

namespace LocalDropshipping.Web.Middlewares
{
    public class AuthenticationMiddleware
    {
        private readonly RequestDelegate _next;

        public AuthenticationMiddleware(RequestDelegate next)
        {
            _next = next;
        }
        public async Task InvokeAsync(HttpContext context, IUserService _userService)
        {
            context.Items.Add("IsSignedIn", false);
            if (_userService.IsUserSignedIn())
            {
                context.Items.Remove("IsSignedIn");
                context.Items.Add("IsSignedIn", true);

                var user = await _userService.GetCurrentUserAsync();
                if (user != null)
                {
                    context.Items.Add("CurrentUser", JsonConvert.SerializeObject(user));
                    List<Roles> currentUserRoles = new List<Roles>();
                    if (user.IsSuperAdmin)
                    {
                        currentUserRoles.Add(Roles.SuperAdmin);
                    }
                    if (user.IsAdmin)
                    {
                        currentUserRoles.Add(Roles.Admin);
                    }
                    if (user.IsSeller)
                    {
                        currentUserRoles.Add(Roles.Seller);
                    }
                    context.Items.Add("CurrentUserRoles", currentUserRoles);
                }
            }
            await _next(context);
        }
    }
}
