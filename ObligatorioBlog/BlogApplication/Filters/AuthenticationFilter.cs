

using BlogDomain;
using BlogServicesInterfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace BlogApplication.Filters
{
    public class AuthenticationFilter : Attribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            string token = context.HttpContext.Request.Headers["Authorization"];

            if (string.IsNullOrEmpty(token))
                context.Result = new ContentResult()
                {
                    Content = "A token is needed.",
                    StatusCode = 401
                };
            else
                TryToParseToken(token, context);
        }

        private void TryToParseToken(string token, AuthorizationFilterContext context)
        {
            try
            {
                Guid parsedToken = Guid.Parse(token);
                VerifyToken(parsedToken, context);
            }
            catch (FormatException)
            {
                context.Result = new ContentResult()
                {
                    Content = "Invalid token format.",
                    StatusCode = 401
                };
            }
        }

        private void VerifyToken(Guid token, AuthorizationFilterContext context)
        {
            ISessionService service = GetSessionService(context);
            if (service.IsValidToken(token))
            {
                User loggedUser = service.GetUserByToken(token);
                context.HttpContext.Items.Add("loggedUser", loggedUser);
            }
            else
            {
                context.Result = new ContentResult()
                {
                    Content = "Invalid token",
                    StatusCode = 401
                };
            }
        }

        private ISessionService? GetSessionService(AuthorizationFilterContext context) =>
            context.HttpContext.RequestServices.GetService(typeof(ISessionService)) as ISessionService;
    }
}
