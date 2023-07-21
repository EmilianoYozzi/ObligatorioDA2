using BlogApplication.Filters;
using BlogApplication.Models.In;
using BlogApplication.Models.Out;
using BlogDomain;
using BlogServicesInterfaces;
using Microsoft.AspNetCore.Mvc;

namespace BlogApplication.Controllers
{
    [ExceptionFilter]
    [ApiController]
    [Route("api/session")]
    public class SessionController : ControllerBase
    {
        private readonly ISessionService service;

        public SessionController(ISessionService service)
        {
            this.service = service;
        }

        [HttpPost]
        public IActionResult PostSession([FromBody] LogInModel logIn) {
            LogInInfo info = logIn.ToEntity();
            Session session = service.LogIn(info);
            OutModelSession sessionOut = new OutModelSession(session);
            return Ok(sessionOut);
        }

        [AuthenticationFilter]
        [HttpDelete("{username}")]
        public IActionResult DeleteSession([FromHeader(Name = "Authorization")] string token)
        {
            Guid tokenGuid = new Guid(token);
            service.LogOut(tokenGuid);
            return Ok();
        }
    }
}
