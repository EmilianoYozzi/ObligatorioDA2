using BlogDomain;
using BlogServicesInterfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace BlogApplication.Filters.Authorization
{
    public class AdminAuthorizationFilter : Attribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            User? loggedUser = context.HttpContext.Items["loggedUser"] as User;
            IUserService? service = GetUserService(context);

            if (!service.IsAdmin(loggedUser.Username))
                context.Result = new ContentResult()
                {
                    Content = "This operation requires admin permissions.",
                    StatusCode = 401
                };
        }

        private IUserService? GetUserService(AuthorizationFilterContext context) =>
            context.HttpContext.RequestServices.GetService(typeof(IUserService)) as IUserService;
    }
}
