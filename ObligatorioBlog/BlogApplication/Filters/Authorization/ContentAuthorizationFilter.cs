using BlogDomain;
using BlogServices;
using BlogServicesInterfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace BlogApplication.Filters.Authorization
{
    public class ContentAuthorizationFilter : Attribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            User? loggedUser = context.HttpContext.Items["loggedUser"] as User;
            IArticleService? articleService = GetArticleService(context);
            IUserService? userService = GetUserService(context);
            string contentId = context.HttpContext.GetRouteValue("id") as string;

            bool isOwner = articleService.IsOwner(contentId, loggedUser.Username);
            bool isAdmin = userService.IsAdmin(loggedUser.Username);

            if (!isOwner && !isAdmin)
                context.Result = new ContentResult()
                {
                    Content = "This operation requires owner permissions.",
                    StatusCode = 401
                };
        }

        private IArticleService? GetArticleService(AuthorizationFilterContext context) =>
            context.HttpContext.RequestServices.GetService(typeof(IArticleService)) as IArticleService;

        private IUserService? GetUserService(AuthorizationFilterContext context) =>
            context.HttpContext.RequestServices.GetService(typeof(IUserService)) as IUserService;
    }
}
