using BlogApplication.Filters;
using BlogApplication.Models.In;
using BlogApplication.Models.Out;
using BlogDomain;
using BlogDomain.DomainEnums;
using BlogServicesInterfaces;
using Microsoft.AspNetCore.Mvc;

namespace BlogApplication.Controllers
{
    [ExceptionFilter]
    [AuthenticationFilter]
    [ApiController]
    [Route("api/comments")]
    public class CommentController : ControllerBase
    {

        private readonly ICommentService services;

        public CommentController(ICommentService services) {
            this.services = services;
        }

        [HttpGet("{id}")]
        public IActionResult GetCommentById([FromRoute] string id)
        {
            Comment comment = services.GetCommentById(id);
            OutModelComment outModelComment = new OutModelComment(comment);
            return Ok(outModelComment);
        }

        [HttpPost]
        public IActionResult PostComment([FromBody] InModelComment commentIn)
        {
            Comment comment = commentIn.ToEntity();
            RunWordControl(comment);
            Comment addedComment = services.CreateComment(comment, commentIn.contentId);
            OutModelComment commentOut = new OutModelComment(addedComment);
            return Ok(commentOut);
        }

        [HttpPut]
        public IActionResult PutComment([FromBody] InModelComment commentIn)
        {
            Comment comment = commentIn.ToEntity();
            RunWordControl(comment);
            Comment updatedComment = services.UpdateComment(comment, commentIn.contentId);
            OutModelComment commentOut = new OutModelComment(updatedComment);
            return Ok(commentOut);
        }

        [HttpDelete("{commentId}")]
        public IActionResult DeleteComment([FromRoute] string commentId)
        {
            services.DeleteCommentById(commentId);
            return Ok();
        }

        private void RunWordControl(Comment comment)
        {
            comment.WaitingForRevision = false;
            User? user = HttpContext.Items["loggedUser"] as User;
            if (!user.HasRole(UserRole.Admin))
                services.VerifyWords(comment);
        }
    }
}
