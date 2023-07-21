using BlogApplication.Filters;
using BlogApplication.Filters.Authorization;
using BlogApplication.Models.In;
using BlogApplication.Models.Out;
using BlogDomain;
using BlogDomain.DomainEnums;
using BlogServicesInterfaces;
using Microsoft.AspNetCore.Mvc;

namespace BlogApplication.Controllers
{
    [ApiController]
    [ExceptionFilter]
    [Route("api/users")]
    public class UserController : ControllerBase
    {
        private readonly IUserService services;

        public UserController(IUserService services)
        {
            this.services = services;
        }

        [HttpPost]
        public IActionResult PostUser([FromBody] InModelUser userIn) {
            
                User user = userIn.ToEntity();
                User addedUser = services.AddUser(user);
                OutModelUser userOut = new OutModelUser(addedUser);
                return Ok(userOut);
        }

        [HttpGet("{username}")]
        public IActionResult GetUserByUsername([FromRoute] string username) 
        {
            User requestedUser = services.GetUserByUsername(username);
            OutModelUser userOut = new OutModelUser(requestedUser);
            return Ok(userOut);
        }

        [AuthenticationFilter]
        [AdminAuthorizationFilter]
        [HttpGet]
        public IActionResult GetAllUsers()
        {
            User[] requestedUsers = services.GetAllUsers();
            OutModelUser[] usersOut = requestedUsers.Select(user => new OutModelUser(user)).ToArray();
            return Ok(usersOut);
        }

        [AuthenticationFilter]
        [UserAuthorizationFilter]
        [HttpPut("{username}")]
        public IActionResult UpdateUser([FromBody] InModelUser userIn, [FromRoute] string username)
        {
            User user = userIn.ToEntity();
            user.Username = username;
            User userToEdit = services.UpdateUser(user);
            OutModelUser userOut = new OutModelUser(userToEdit);
            return Ok(userOut);
        }

        [AuthenticationFilter]
        [AdminAuthorizationFilter]
        [HttpDelete("{username}")]
        public IActionResult DeleteUser([FromRoute] string username)
        {
            services.DeleteUser(username);
            return Ok();
        }

        [AuthenticationFilter]
        [UserAuthorizationFilter]
        [HttpGet("{username}/notifications")]
        public IActionResult GetNotifications([FromRoute] string username)
        {
            Notification[] notifications = services.GetUserNotifications(username);
            OutModelNotification[] notiOut = notifications.Select(n => new OutModelNotification(n)).ToArray();
            return Ok(notiOut);
        }

        [AuthenticationFilter]
        [HttpGet("{username}/articles")]
        public IActionResult GetArticles([FromRoute] string username)
        {
            User loggedUser = HttpContext.Items["loggedUser"] as User;
            bool showHiddenArticles = loggedUser.Username.Equals(username) || 
                loggedUser.HasRole(UserRole.Admin); 

            Article[] articles = showHiddenArticles ? 
                services.GetAllArticlesFromUser(username) :
                services.GetVisibleArticlesFromUser(username);

            OutModelArticle[] articlesOut = articles.Select(a => new OutModelArticle(a)).ToArray();
            return Ok(articlesOut);
        }
    }
}
