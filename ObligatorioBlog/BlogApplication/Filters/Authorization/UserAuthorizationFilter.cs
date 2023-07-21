using BlogDomain;
using BlogServicesInterfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace BlogApplication.Filters.Authorization
{
    public class UserAuthorizationFilter : Attribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            User? loggedUser = context.HttpContext.Items["loggedUser"] as User;
            IUserService? userService = GetUserService(context);
            string username = context.HttpContext.GetRouteValue("username") as string;

            bool isAdmin = userService.IsAdmin(loggedUser.Username);
            bool isTheUser = loggedUser.Equals(userService.GetUserByUsername(username));

            if (!isAdmin && !isTheUser)
                context.Result = new ContentResult()
                {
                    Content = "Permissions required.",
                    StatusCode = 401
                };
        }

        private IUserService? GetUserService(AuthorizationFilterContext context) =>
            context.HttpContext.RequestServices.GetService(typeof(IUserService)) as IUserService;
    }
}
