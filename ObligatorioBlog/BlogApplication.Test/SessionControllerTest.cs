using BlogApplication.Controllers;
using BlogApplication.Models.In;
using BlogApplication.Models.Out;
using BlogDomain;
using BlogDomain.DomainEnums;
using BlogServicesInterfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace BlogApplication.Test
{
    [TestClass]
    public class SessionControllerTest
    {
        private Mock<ISessionService> service;
        private SessionController controller;

        private string username;
        private string tokenStr;
        private Guid token;

        [TestInitialize]
        public void Initialize()
        {
            service = new Mock<ISessionService>(MockBehavior.Strict);
            controller = new SessionController(service.Object);

            tokenStr = "5fb7097c-335c-4d07-b4fd-000004e2d28c";
            token = new Guid(tokenStr);
            username = "OniTheDemon";
        }

        [TestMethod]
        public void LogIn()
        {
            LogInModel logIn = new LogInModel()
            {
                Username = username,
                Email = "onithedemon@gmail.com",
                Password = "123456"
            };
            Session session = new Session()
            {
                Username = username,
                Token = token,
                Role = UserRole.Admin
            };
            OutModelSession expectedSession = new OutModelSession(session);
            service.Setup(u => u.LogIn(It.IsAny<LogInInfo>())).Returns(session);

            IActionResult actionResult = controller.PostSession(logIn);

            service.VerifyAll();
            OkObjectResult? result = actionResult as OkObjectResult;
            Assert.AreEqual(expectedSession, result?.Value as OutModelSession);
        }

        [TestMethod]
        public void LogOut()
        {
            service.Setup(u => u.LogOut(It.IsAny<Guid>()));

            IActionResult result = controller.DeleteSession(tokenStr);

            service.VerifyAll();
            StatusCodeResult? resultObject = result as StatusCodeResult;
            Assert.AreEqual(200, resultObject?.StatusCode);
        }
    }
}