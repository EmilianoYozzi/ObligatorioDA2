using BlogApplication.Controllers;
using BlogApplication.Models.In;
using BlogApplication.Models.Out;
using BlogDomain;
using BlogDomain.DomainEnums;
using BlogServicesInterfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Newtonsoft.Json.Linq;

namespace BlogApplication.Test
{
    [TestClass]
    public class UserControllerTest
    {
        private UserController controller;
        private Mock<IUserService> service;
        private HttpContext context;

        private string username;
        private InModelUser userIn;
        private User expectedUser;
        private Notification[] notifications;
        private User loggedUser;

        [TestInitialize]
        public void Initialize()
        {
            service = new Mock<IUserService>(MockBehavior.Strict);
            controller = new UserController(service.Object);

            username = "OniTheDemon";
            userIn = new InModelUser()
            {
                Username = username,
                Name = "Oni",
                Lastname = "TheDemon",
                Email = "onithedemon@gmail.com",
                Password = "123456"
            };
            expectedUser = new User()
            {
                Username = "OniTheDemon",
                Name = "Oni",
                Lastname = "TheDemon",
                Email = "onithedemon@gmail.com",
                Password = "123456"
            };
            notifications = new Notification[] {
                new Notification() {
                    Message = "Your post got commented!",
                    Uri = "articles/2394",
                    Id = "1"
                }
            };

            context = new DefaultHttpContext();
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = context
            };
            loggedUser = new User()
            {
                Username = "Emi",
                Role = UserRole.Blogger
            };
            context.Items["loggedUser"] = loggedUser;
        }

        [TestMethod]
        public void AddUser()
        {
            service.Setup(u => u.AddUser(It.IsAny<User>())).Returns(expectedUser);
            OutModelUser expectedResult = new OutModelUser(expectedUser);

            IActionResult actionResult = controller.PostUser(userIn);

            service.VerifyAll();
            OkObjectResult? result = actionResult as OkObjectResult;
            Assert.AreEqual(expectedResult, result?.Value as OutModelUser);
        }

        [TestMethod]
        public void GetUser()
        {
            service.Setup(u => u.GetUserByUsername(It.IsAny<string>())).Returns(expectedUser);
            OutModelUser expectedResult = new OutModelUser(expectedUser);

            IActionResult actionResult = controller.GetUserByUsername(username);

            service.VerifyAll();
            OkObjectResult? result = actionResult as OkObjectResult;
            Assert.AreEqual(expectedResult, result?.Value as OutModelUser);
        }

        [TestMethod]
        public void UpdateUser()
        {
            OutModelUser expectedResult = new OutModelUser(expectedUser);
            service.Setup(u => u.UpdateUser(It.IsAny<User>())).Returns(expectedUser);

            IActionResult actionResult = controller.UpdateUser(userIn, username);

            service.VerifyAll();
            OkObjectResult? result = actionResult as OkObjectResult;
            Assert.AreEqual(expectedResult, result?.Value as OutModelUser);
        }

        [TestMethod]
        public void GetAllUsers()
        {
            User[] users = new User[] { expectedUser };
            OutModelUser[] expectedResult = users.Select(u => new OutModelUser(u)).ToArray();
            service.Setup(u => u.GetAllUsers()).Returns(users);

            IActionResult actionResult = controller.GetAllUsers();

            service.VerifyAll();
            OkObjectResult? result = actionResult as OkObjectResult;
            CollectionAssert.AreEqual(expectedResult, result?.Value as OutModelUser[]);
        }

        [TestMethod]
        public void DeleteUser()
        {
            service.Setup(u => u.DeleteUser(It.IsAny<string>()));

            IActionResult actionResult = controller.DeleteUser(username);

            service.VerifyAll();
            StatusCodeResult result = actionResult as StatusCodeResult;
            Assert.AreEqual(200, result.StatusCode);
        }

        [TestMethod]
        public void GetArticlesFromAUserNotBeingAdminOrOwner()
        {
            Article[] articles = new Article[] { };
            service.Setup(u => u.GetVisibleArticlesFromUser(username)).Returns(articles);

            IActionResult actionResult = controller.GetArticles(username);

            service.VerifyAll();
            OkObjectResult result = actionResult as OkObjectResult;
            Assert.IsInstanceOfType(result.Value as OutModelArticle[], typeof(OutModelArticle[]));
            Assert.AreEqual(200, result.StatusCode);
        }

        [TestMethod]
        public void GetArticlesFromAUserBeingAdmin()
        {
            loggedUser.Role = UserRole.Admin;
            Article[] articles = new Article[] { };
            service.Setup(u => u.GetAllArticlesFromUser(username)).Returns(articles);

            IActionResult actionResult = controller.GetArticles(username);

            service.VerifyAll();
            OkObjectResult result = actionResult as OkObjectResult;
            Assert.IsInstanceOfType(result.Value as OutModelArticle[], typeof(OutModelArticle[]));
            Assert.AreEqual(200, result.StatusCode);
        }

        [TestMethod]
        public void GetArticlesFromAUserBeingTheSameUser()
        {
            loggedUser.Username = username;
            Article[] articles = new Article[] { };
            service.Setup(u => u.GetAllArticlesFromUser(username)).Returns(articles);

            IActionResult actionResult = controller.GetArticles(username);

            service.VerifyAll();
            OkObjectResult result = actionResult as OkObjectResult;
            Assert.IsInstanceOfType(result.Value as OutModelArticle[], typeof(OutModelArticle[]));
            Assert.AreEqual(200, result.StatusCode);
        }

        [TestMethod]
        public void GetNotifications()
        {            
            OutModelNotification[] expectedResult = notifications.Select(n => new OutModelNotification(n)).ToArray();

            service.Setup(u => u.GetUserNotifications(It.IsAny<string>())).Returns(notifications);

            IActionResult actionResult = controller.GetNotifications(username);

            service.VerifyAll();
            OkObjectResult result = actionResult as OkObjectResult;
            CollectionAssert.AreEqual(expectedResult, result.Value as OutModelNotification[]);
            Assert.AreEqual(200, result.StatusCode);
        }
    }
}