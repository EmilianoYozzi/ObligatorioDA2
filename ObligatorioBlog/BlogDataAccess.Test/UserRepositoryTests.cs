using BlogDataAccess.Interfaces;
using BlogDataAccess.Repositories;
using BlogDomain;
using Exceptions;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace BlogDataAccess.Test
{
    [TestClass]
    public class UserRepositoryTests
    {
        private User user;
        private List<User> items;
        private Mock<DbSet<User>> setUsers;
        private Mock<DbContext> context;
        private IUserRepository repository;

        [TestInitialize]
        public void Initialize()
        {
            user = new User()
            {
                Username = "Titolandia",
                Name = "Franco",
                Password = "1234",
                Email = "titolandia@gmail.com",
                Lastname = "Thomasset"
            };
            items = new List<User>();
            context = new Mock<DbContext>();
            repository = new UserRepository(context.Object);
            setUsers = CreateMockSetUser(items);
            SetupContext();
        }

        [TestMethod]
        public void AddUser()
        {
            repository.Add(user);
            CollectionAssert.Contains(setUsers.Object.ToArray(), user);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException), "Username already exists")]
        public void AddUserWithUsernameAlreadyUsed()
        {
            items.Add(user);
            repository.Add(user);
        }

        [TestMethod]
        public void GetUserByUsername()
        {
            items.Add(user);
            User result = repository.GetByUsername(user.Username);
            Assert.AreEqual(user, result);

        }

        [TestMethod]
        public void GetUserByEmail()
        {
            items.Add(user);
            User result = repository.GetByEmail(user.Email);
            Assert.AreEqual(user, result);
        }

        [TestMethod]
        public void UpdateUser()
        {
            User modifiedUser = new User()
            {
                Username = "Titolandia",
                Name = "Mariano",
                Password = "1234",
                Email = "titolandia@gmail.com",
                Lastname = "Pereira"
            };
            items.Add(user);
            repository.Update(modifiedUser);
            User result = setUsers.Object.First((u) => u.Username == modifiedUser.Username);
            Assert.AreEqual(user, result);
        }

        [TestMethod]
        public void DeleteUser()
        {

            User deletedUser = new User()
            {
                Username = "Titolandia",
                Name = "Franco",
                Password = "1234",
                Email = "titolandia@gmail.com",
                Lastname = "Pereira",
                Deleted = true,
            };

            items.Add(user);

            repository.Delete(u => u.Username.Equals(user.Username));
            User result = setUsers.Object.First((u) => u.Username == deletedUser.Username);
            Assert.AreEqual(deletedUser, user);
        }

        [TestMethod]
        public void GetAllUsersTest()
        {
            User anotherUser = new User()
            {
                Username = "Titolandia2",
                Name = "Franco",
                Password = "1234",
                Email = "titolandia@gmail.com",
                Lastname = "Thomasset"
            };

            items.Add(user);
            items.Add(anotherUser);
            User[] expectedUsers = { user, anotherUser };
            var result = repository.GetAllUsers();

            CollectionAssert.AreEqual(result, expectedUsers);
        }

        [TestMethod]
        public void GetMultipleArticles()
        {
            items.Add(user);
            items.Add(user);

            User[] users = repository.GetMultiple(u => u.Username.Equals(user.Username));

            CollectionAssert.AreEqual(items, users);
        }

        [TestMethod]
        public void GetEmptyMultipleArticles()
        {
            User[] users = repository.GetMultiple(u => u.Username.Equals("not" + user.Username));

            CollectionAssert.AreEqual(new List<Article>(), users);
        }

        [TestMethod]
        public void CheckIfAnExistentUserExists()
        {
            items.Add(user);

            bool result = repository.Exists(user.Username);

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void CheckIfANonExistentUserExists()
        {
            bool result = repository.Exists(user.Username);

            Assert.IsFalse(result);
        }

        private Mock<DbSet<User>> CreateMockSetUser(List<User> items)
        {
            IQueryable<User> data = items.AsQueryable();
            Mock<DbSet<User>> mock = new Mock<DbSet<User>>();
            mock.As<IQueryable<User>>().Setup(m => m.Provider).Returns(data.Provider);
            mock.As<IQueryable<User>>().Setup(m => m.Expression).Returns(data.Expression);
            mock.As<IQueryable<User>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mock.As<IQueryable<User>>().Setup(m => m.GetEnumerator()).Returns(() => data.GetEnumerator());
            mock.Setup(m => m.Add(It.IsAny<User>())).Callback<User>((s) => items.Add(s));
            return mock;
        }

        private void SetupContext()
        {
            context.Setup(x => x.Set<User>()).Returns(setUsers.Object);
            context.Setup(x => x.SaveChanges()).Returns(0);
        }

        private SqlException MakeSqlException()
        {
            SqlException exception = null;
            try
            {
                SqlConnection conn = new SqlConnection(@"Data Source=.;Database=GUARANTEED_TO_FAIL;Connection Timeout=1");
                conn.Open();
            }
            catch (SqlException ex)
            {
                exception = ex;
            }
            return (exception);
        }
    }
}