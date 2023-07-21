using BlogDataAccess.Interfaces;
using BlogDomain;
using BlogDomain.DomainEnums;
using Moq;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace BlogServices.Test
{
    [TestClass]
    public class SessionServiceTest
    {
        private Guid token;
        private User user;
        private Session session;
        private LogInInfo logInInfo;
        private Mock<ISessionRepository> sessionRepo;
        private Mock<IUserRepository> userRepository;
        private SessionService service;

        [TestInitialize] 
        public void Initialize()
        {
            token = new Guid("5fb7097c-335c-4d07-b4fd-000004e2d28c");
            user = new User()
            {
                Username = "mar10",
                Name = "Mario",
                Lastname = "Bros",
                Email = "mario@nintendo.com",
                Password = "123123",
                Role = UserRole.Blogger
            };
            session = new Session()
            {
                Username = "mar10",
                Token = token
            };
            logInInfo = new LogInInfo()
            {
                Username = "mar10",
                Email = "mario@nintendo.com",
                Password = "123123"
            };
            sessionRepo = new Mock<ISessionRepository>(MockBehavior.Strict);
            userRepository = new Mock<IUserRepository>(MockBehavior.Strict);
            service = new SessionService(sessionRepo.Object, userRepository.Object);
            sessionRepo.Setup(s => s.Add(It.IsAny<LogInInfo>())).Returns(session);
            sessionRepo.Setup(s => s.Get(It.IsAny<Func<Session, bool>>())).Returns(session);
            sessionRepo.Setup(s => s.Exists(It.IsAny<Guid>())).Returns(false);
            sessionRepo.Setup(s => s.Exists(token)).Returns(true);
            sessionRepo.Setup(s => s.DeleteByToken(It.IsAny<Guid>()));
            userRepository.Setup(u => u.GetByUsername(user.Username)).Returns(user);
            userRepository.Setup(u => u.GetByEmail(It.IsAny<string>())).Returns(user);
        }

        [TestMethod]
        public void LogInOnlyWithUsername()
        {
            logInInfo.Email = "";

            Session result = service.LogIn(logInInfo);

            Assert.AreEqual(session, result);
        }

        [TestMethod]
        public void LogInOnlyWithEmail()
        {
            logInInfo.Username = "";

            Session result = service.LogIn(logInInfo);

            Assert.AreEqual(session, result);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException), "Username or Email is required")]
        public void LoginWithoutUsernameOrMail()
        {
            logInInfo.Username = "";
            logInInfo.Email = "";
            service.LogIn(logInInfo);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException), "Password is incorrect")]
        public void LoginWithWrongPassword()
        {
            logInInfo.Password = "NotMyPassword";
            service.LogIn(logInInfo);            
        }
        
        [TestMethod]
        public void GetUserByToken()
        {
            User result = service.GetUserByToken(token);
            Assert.AreEqual(result, user);
        }

        [TestMethod]
        public void IsValidToken()
        {
            bool result = service.IsValidToken(token);
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void IsNotValidToken()
        {
            Guid invalidToken = new Guid("2ab2308b-335c-4d07-b4fd-000004e2d28c");
            bool result = service.IsValidToken(invalidToken);
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void LogOut() {
            service.LogOut(token);
        }

    }
}
