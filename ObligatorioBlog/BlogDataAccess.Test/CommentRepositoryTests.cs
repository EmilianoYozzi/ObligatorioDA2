
using BlogDataAccess.Interfaces;
using BlogDataAccess.Repositories;
using BlogDomain;
using Exceptions;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Moq;
using System.Collections.Generic;
using System.Net.Sockets;

namespace BlogDataAccess.Test
{
    [TestClass]
    public class CommentRepositoryTests
    {
        private Comment comment;
        private List<Comment> items;
        private Mock<DbSet<Comment>> setComments;
        private Mock<DbContext> context;
        private ICommentRepository repository;

        [TestInitialize]
        public void Initialize()
        {
            comment = new Comment()
            {
                OwnerUsername = "Otzdarva",
                Text = "Nice blog.",
                IdAttachedTo = "122",
                Id = "538"
            };
            context = new Mock<DbContext>();
            repository = new CommentRepository(context.Object);
            items = new List<Comment>();
            setComments = CreateMockSetComment(items);
            SetupContext();
        }

        [TestMethod]
        public void AddComment()
        {
            repository.Add(comment);

            CollectionAssert.Contains(setComments.Object.ToArray(), comment);
        }

        [TestMethod]
        [ExpectedException(typeof(UnexpectedDataAccessException))]
        public void AddCommentWithException()
        {
            setComments.Setup(x => x.Add(It.IsAny<Comment>())).Throws(MakeSqlException());

            repository.Add(comment);
        }

        [TestMethod]
        public void GetCommentById()
        {
            items.Add(comment);

            Comment result = repository.GetById(comment.Id);

            Assert.AreEqual(comment, result);
        }

        [TestMethod]
        [ExpectedException(typeof(ResourceNotFoundException), "Comment not found.")]
        public void GetCommentThatDoesntExistById()
        {
            repository.GetById(comment.Id);
        }
        [TestMethod]
        public void UpdateComment()
        {
            items.Add(comment);

            Comment updatedComment = new Comment()
            {
                OwnerUsername = "Otzdarva",
                Text = "Nice one.",
                IdAttachedTo = "122",
                Id = "538"
            };

            repository.Update(updatedComment);

            Assert.AreEqual(updatedComment, comment);
        }
        [TestMethod]
        public void UpdatedCommentIsEdited()
        {
            items.Add(comment);

            Comment updatedComment = new Comment()
            {
                OwnerUsername = "Otzdarva",
                Text = "Nice one.",
                IdAttachedTo = "122",
                Id = "538"
            };
            repository.Update(updatedComment);
            Assert.IsTrue(comment.Edited);
        }

        [TestMethod]
        public void DeleteComment()
        {
            items.Add(comment);

            repository.Delete(comment.Id);
            Comment commentToDelete = context.Object.Set<Comment>().First(c => c.Id.Equals(comment.Id));

            Assert.IsTrue(comment.Deleted);
        }

        [TestMethod]
        [ExpectedException(typeof(ResourceNotFoundException), "Comment not found.")]
        public void DeleteCommentThatDoesntExist()
        {
            repository.Delete(comment.Id);
        }

        [TestMethod]
        public void CheckIfAnExistentCommentExists()
        {
            items.Add(comment);

            bool result = repository.Exists(comment.Id);

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void CheckIfANonExistentCommentExists()
        {
            bool result = repository.Exists(comment.Id);

            Assert.IsFalse(result);
        }

        private Mock<DbSet<Comment>> CreateMockSetComment(List<Comment> items)
        {
            IQueryable<Comment> data = items.AsQueryable();
            Mock<DbSet<Comment>> mock = new Mock<DbSet<Comment>>();
            mock.As<IQueryable<Comment>>().Setup(m => m.Provider).Returns(data.Provider);
            mock.As<IQueryable<Comment>>().Setup(m => m.Expression).Returns(data.Expression);
            mock.As<IQueryable<Comment>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mock.As<IQueryable<Comment>>().Setup(m => m.GetEnumerator()).Returns(() => data.GetEnumerator());
            mock.Setup(m => m.Add(It.IsAny<Comment>())).Callback<Comment>((s) => items.Add(s));
            return mock;
        }

        private void SetupContext()
        {
            context.Setup(x => x.Set<Comment>()).Returns(setComments.Object);
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