using BlogDataAccess.Interfaces;
using BlogDomain;
using BlogDomain.DomainEnums;
using BlogServices;
using Moq;
using System.Data;
using System.Diagnostics;

namespace BlogServicesTest
{
    [TestClass]
    public class UserServiceTest
    {
        private User user;
        private Notification notification;
        private Mock<IUserRepository> userRepo;
        private UserService service;
        private User[] users;

        [TestInitialize]
        public void Initialize()
        {
            user = new User()
            {
                Username = "Nobuwu",
                Name = "Nobunaga",
                Lastname = "Oda",
                Email = "noda@gmail.com",
                Password = "Shogun22",
                Role = UserRole.Blogger
            };
            notification = new Notification()
            {
                Message = "Your post got commented!",
            };
            userRepo = new Mock<IUserRepository>(MockBehavior.Strict);
            userRepo.Setup(u => u.Add(user)).Returns(user);
            userRepo.Setup(u => u.GetByUsername("Nobuwu")).Returns(user);
            userRepo.Setup(u => u.Update(user)).Returns(user);
            userRepo.Setup(u => u.GetMultiple(It.IsAny<Func<User, bool>>())).Returns(new User[] { user });
            service = new UserService(userRepo.Object);
            users = new User[] {
                new User()
                {
                    Username = "OniTheDemon",
                    Name = "Oni",
                    Lastname = "TheDemon",
                    Email = "onithedemon@gmail.com",
                    Password = "HuTao77"
                },
                new User()
                {
                    Username = "Titolandia22",
                    Name = "Tito",
                    Lastname = "Landia",
                    Email = "tito22@hotmail.com",
                    Password = "NoelleNoelle"
                }
            };
        }

        [TestMethod]
        public void AddUser()
        {
            User result = service.AddUser(user);
            Assert.AreEqual(user, result);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException), "Username is needed.")]
        public void AddUserWithEmptyUsername()
        {
            user.Username = "";
            service.AddUser(user);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException), "Name is needed.")]
        public void AddUserWithEmptyName()
        {
            user.Name = "";
            service.AddUser(user);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException), "Lastname is needed.")]
        public void AddUserWithEmptyLastname()
        {
            user.Lastname = "";
            service.AddUser(user);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException), "Email is needed.")]
        public void AddUserWithEmptyEmail()
        {
            user.Email = "";
            service.AddUser(user);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException), "Password is needed.")]
        public void AddUserWithEmptyPassword()
        {
            user.Password = "";
            service.AddUser(user);
        }

        [TestMethod]
        [ExpectedException(typeof(FormatException), "Invalid username.")]
        public void AddUserWithInvalidUsername()
        {
            user.Username = "Oni_The_Demon";
            service.AddUser(user);
        }

        [TestMethod]
        [ExpectedException(typeof(FormatException), "Invalid email.")]
        public void AddUserWithInvalidEmail()
        {
            user.Email = "onith@edemon@gmail.";
            service.AddUser(user);
        }

        [TestMethod]
        public void GetUserByUsername()
        {
            User result = service.GetUserByUsername("Nobuwu");
            Assert.AreEqual(user, result);
        }

        [TestMethod]
        public void GetAllUsers()
        {
            userRepo.Setup(u => u.GetAllUsers()).Returns(users);

            User[] result = service.GetAllUsers();

            CollectionAssert.AreEqual(users, result);
        }

        [TestMethod]
        public void GetAllUsersDoesntReturnsDeletedUsers()
        {
            users[1].Deleted = true;
            User[] expected = new User[]
            {
                new User()
                {
                    Username = "OniTheDemon",
                    Name = "Oni",
                    Lastname = "TheDemon",
                    Email = "onithedemon@gmail.com",
                    Password = "HuTao77"
                }
            };

            userRepo.Setup(u => u.GetAllUsers()).Returns(users);

            User[] result = service.GetAllUsers();

            CollectionAssert.AreEqual(expected, result);
        }

        [TestMethod]
        public void UpdateUser()
        {
            User result = service.UpdateUser(user);

            Assert.AreEqual(user, result);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException), "Username is needed.")]
        public void UpdateUserWithEmptyUsername()
        {
            user.Username = "";
            service.UpdateUser(user);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException), "Name is needed.")]
        public void UpdateUserWithEmptyName()
        {
            user.Name = "";
            service.UpdateUser(user);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException), "Lastname is needed.")]
        public void UpdateUserWithEmptyLastname()
        {
            user.Lastname = "";
            service.UpdateUser(user);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException), "Email is needed.")]
        public void UpdateUserWithEmptyEmail()
        {
            user.Email = "";
            service.UpdateUser(user);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException), "Password is needed.")]
        public void UpdateUserWithEmptyPassword()
        {
            user.Password = "";
            service.UpdateUser(user);
        }

        [TestMethod]
        [ExpectedException(typeof(FormatException), "Invalid username.")]
        public void UpdateUserWithInvalidUsername()
        {
            user.Username = "Oni_The_Demon";
            service.UpdateUser(user);
        }

        [TestMethod]
        [ExpectedException(typeof(FormatException), "Invalid email.")]
        public void UpdateUserWithInvalidEmail()
        {
            user.Email = "onith@edemon@gmail.";
            service.UpdateUser(user);
        }

        [TestMethod]
        public void CheckIfNonAdminUserIsAdmin()
        {
            bool result = service.IsAdmin(user.Username);

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void CheckIfAdminUserIsAdmin()
        {
            user.Role = UserRole.Admin;
            bool result = service.IsAdmin(user.Username);

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void DeleteUser()
        {
            userRepo.Setup(u => u.DeleteByUsername(It.IsAny<string>()));
            service.DeleteUser(user.Username);
        }

        [TestMethod]
        public void NotifyUser()
        {
            service.NotifyUser(notification, user.Username);
            Assert.AreEqual(notification, user.Notifications[0]);
        }

        [TestMethod]
        public void NotifyAdmins()
        {
            service.NotifyAdmins(notification);
            Assert.AreEqual(notification, user.Notifications[0]);
        }

        [TestMethod]
        public void GetUserNotifications()
        {
            user.Notifications = new List<Notification>()
            {
                new Notification() { Id = "3" },
                new Notification() { Id = "5" },
                new Notification() { Id = "8" }
            };

            Notification[] result = service.GetUserNotifications(user.Username);

            CollectionAssert.AreEqual(user.Notifications, result);
        }

        [TestMethod]
        public void GettingAllArticlesFromUserTakesHiddenArticles()
        {
            user.Articles = GenerateArticles(3);
            user.Articles[1].Visibility = Visibility.Private;
            user.Articles[2].WaitingForRevision = true;

            Article[] articles = service.GetAllArticlesFromUser(user.Username);

            Assert.AreEqual(3, articles.Length);
        }

        [TestMethod]
        public void GettingVisibleArticlesFromUserDoesNotTakeHiddenArticles()
        {
            user.Articles = GenerateArticles(3);
            user.Articles[1].Visibility = Visibility.Private;
            user.Articles[2].WaitingForRevision = true;

            Article[] articles = service.GetVisibleArticlesFromUser(user.Username);

            Assert.AreEqual(1, articles.Length);
        }

        private List<Article> GenerateArticles(int amount)
        {
            List<Article> articles = new List<Article>();
            for (int i = 0; i < amount; i++)
            {
                Article article = new Article()
                {
                    OwnerUsername = "MartinSpammer",
                    Id = i.ToString(),
                    Template = ArticleTemplate.ImageAtTopLeft,
                    Text = "Spam",
                    Title = "Spam",
                    Visibility = Visibility.Public,
                    Images = new List<Image>() { new Image() { Content = "spam.png" } }
                };
                DateTime date = new DateTime(2023, 5, i * (1 + (int)Math.Pow(-1, i)) + 1);
                article.DateLog.RegisterModification(date);
                articles.Add(article);
            }
            return articles;
        }
    }
}