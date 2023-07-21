using BlogApplication.Controllers;
using BlogApplication.Models.In;
using BlogApplication.Models.Out;
using BlogDomain;
using BlogDomain.DomainEnums;
using BlogServicesInterfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace BlogApplication.Test
{
    [TestClass]
    public class CommentControllerTest
    {
        private Mock<ICommentService> service;
        private CommentController controller;
        private HttpContext context;

        private InModelComment commentIn;
        private Comment comment;
        private User user;

        [TestInitialize]
        public void Initialize()
        {
            service = new Mock<ICommentService>(MockBehavior.Strict);
            controller = new CommentController(service.Object);
            context = new DefaultHttpContext();
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = context
            };
            user = new User()
            {
                Username = "Emi",
                Role = UserRole.Blogger
            };
            context.Items["loggedUser"] = user;

            commentIn = new InModelComment()
            {
                Text = "I think your article is really bad",
                OwnerUsername = "Franco",
                contentId = "2023"
            };

            comment = new Comment()
            {
                Text = "I think your article is really bad",
                OwnerUsername = "Franco",
                IdAttachedTo = "2023"
            };
        }

        [TestMethod]
        public void CreateComment()
        {
            OutModelComment expectedResult = new OutModelComment(comment);
            service.Setup(c => c.CreateComment(It.IsAny<Comment>(), It.IsAny<string>())).Returns(comment);
            service.Setup(a => a.VerifyWords(It.IsAny<Comment>()));

            IActionResult actionResult = controller.PostComment(commentIn);

            service.VerifyAll();
            OkObjectResult? result = actionResult as OkObjectResult;
            Assert.AreEqual(expectedResult, result?.Value as OutModelComment);
        }

        [TestMethod]
        public void GetComment()
        {
            string commentId = "1234";
            OutModelComment expectedComment = new OutModelComment(comment);
            service.Setup(c => c.GetCommentById(It.IsAny<string>())).Returns(comment);

            IActionResult actionResult = controller.GetCommentById(commentId);

            service.VerifyAll();
            OkObjectResult? result = actionResult as OkObjectResult;
            Assert.AreEqual(expectedComment, result?.Value as OutModelComment);
        }

        [TestMethod]
        public void UpdateComment()
        {
            service.Setup(a => a.VerifyWords(It.IsAny<Comment>()));
            InModelComment newInModelComment = new InModelComment()
            {
                contentId = "2023",
                OwnerUsername = "Franco",
                Text = "I think your article is really good"
            };

            Comment newComment = new Comment()
            {
                IdAttachedTo = "2023",
                OwnerUsername = "Franco",
                Text = "I think your article is really good"
            };
            OutModelComment expectedComment = new OutModelComment(newComment);
            service.Setup(c => c.UpdateComment(It.IsAny<Comment>(), It.IsAny<string>())).Returns(newComment);

            IActionResult actionResult = controller.PutComment(newInModelComment);
            service.VerifyAll();
            OkObjectResult? result = actionResult as OkObjectResult;
            Assert.AreEqual(expectedComment, result?.Value as OutModelComment);
        }

        [TestMethod]
        public void DeleteComment()
        {
            string commentId = "1234";
            service.Setup(c => c.DeleteCommentById(It.IsAny<string>()));

            IActionResult actionResult = controller.DeleteComment(commentId);
            service.VerifyAll();
            StatusCodeResult? result = actionResult as StatusCodeResult;
            Assert.AreEqual(200, result?.StatusCode);
        }

        [TestMethod]
        public void VerifyWordsIsNotBeingCalledWhenAddingCommentAsAdmin()
        {
            user.Role = UserRole.Admin;

            OutModelComment expectedResult = new OutModelComment(comment);
            service.Setup(u => u.CreateComment(It.IsAny<Comment>(), It.IsAny<string>())).Returns(comment);

            IActionResult actionResult = controller.PostComment(commentIn);

            service.Verify(s => s.VerifyWords(It.IsAny<Comment>()), Times.Never);
        }
    }
}
