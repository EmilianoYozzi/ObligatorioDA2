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
    public class SessionRepositoryTests
    {
        private string username;
        private Session session;
        private Guid token;
        private List<Session> sessions;
        private Mock<DbSet<Session>> setSessions;
        private Mock<DbContext> context;
        private ISessionRepository repository;
        private LogInInfo info;

        [TestInitialize]
        public void Initialize()
        {
            token = new Guid("b30a2343-b1a9-478c-814a-ee457749306e");
            username = "Megumin";
            session = new Session()
            {
                Username = username,
                Token = token
            };
            info = new LogInInfo()
            {
                Username = username,
                Password = "Explosion"
            };
            context = new Mock<DbContext>();
            repository = new SessionRepository(context.Object);
            sessions = new List<Session>();
            setSessions = CreateMockSetSession(sessions);
            SetupContext();
        }

        [TestMethod]
        public void GetSessionByToken()
        {
            sessions.Add(session);
                      
            Session requestedSession = repository.GetByToken(token);

            Assert.AreEqual(session, requestedSession);
        }

        [TestMethod]
        [ExpectedException(typeof(ResourceNotFoundException))]
        public void GetUserUsingANonExistentToken()
        {
            repository.GetByToken(token);
        }

        [TestMethod]
        public void CheckIfExistentSessionExists()
        {
            sessions.Add(session);
            bool exists = repository.Exists(token);
            
            Assert.IsTrue(exists);
        }

        [TestMethod]
        public void CheckIfNonExistentSessionExists()
        {
            bool exists = repository.Exists(token);

            Assert.IsFalse(exists);
        }

        [TestMethod]
        public void DeleteSessionWithCorrectToken()
        {
            sessions.Add(session);

            repository.DeleteByToken(session.Token);

            Session sessionFromSet = setSessions.Object.First(s => s.Token.Equals(token));
            Assert.IsTrue(sessionFromSet.Deleted);
        }

        [TestMethod]
        [ExpectedException(typeof(ResourceNotFoundException), "Token not found.")]
        public void DeleteSessionWithWrongToken()
        {
            Guid wrongToken = new Guid("b30a2343-b1a9-478c-814a-ee451149306e");
            sessions.Add(session);
            repository.DeleteByToken(wrongToken);
        }

        [TestMethod]
        public void AddSession()
        {
            repository.Add(info);
            Session sessionFromSet = setSessions.Object.First(s => s.Username.Equals(username));

            Assert.AreEqual(username, sessionFromSet.Username);
        }

        [TestMethod]
        [ExpectedException(typeof(UnexpectedDataAccessException))]
        public void AddSessionWithSqlException()
        {
            setSessions.Setup(x => x.Add(It.IsAny<Session>())).Throws(MakeSqlException());            

            repository.Add(info);
        }

        private Mock<DbSet<Session>> CreateMockSetSession(List<Session> items)
        {
            IQueryable<Session> data = items.AsQueryable();
            Mock<DbSet<Session>> mock = new Mock<DbSet<Session>>();
            mock.As<IQueryable<Session>>().Setup(m => m.Provider).Returns(data.Provider);
            mock.As<IQueryable<Session>>().Setup(m => m.Expression).Returns(data.Expression);
            mock.As<IQueryable<Session>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mock.As<IQueryable<Session>>().Setup(m => m.GetEnumerator()).Returns(() => data.GetEnumerator());
            mock.Setup(m => m.Add(It.IsAny<Session>())).Callback<Session>((s) => items.Add(s));
            return mock;
        }

        private void SetupContext()
        {
            context.Setup(x => x.Set<Session>()).Returns(setSessions.Object);
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