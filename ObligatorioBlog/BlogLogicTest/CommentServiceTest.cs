using BlogDataAccess.Interfaces;
using BlogDomain;
using BlogServicesInterfaces;
using Moq;

namespace BlogServices.Test
{
    [TestClass]
    public class CommentServiceTest
    {
        private Mock<ICommentRepository> commentRepo;
        private Mock<IUserService> userService;
        private Mock<IArticleService> articleService;
        private Mock<IWordControl> wordControl;
        private ICommentService service;
        private string commentId;
        private string articleId;
        private string username;
        private Comment comment;
        private Article article;
        private User owner;

        [TestInitialize]
        public void Initialize()
        {
            commentId = "123123";
            articleId = "757575";
            username = "Taringero013";
            owner = new User()
            {
                Username = username,
                Name = "Esteban",
                Lastname = "Quito",
            };
            comment = new Comment()
            {
                Text = "+10 lince.",
                OwnerUsername = username,
                IdAttachedTo = articleId,
                Id = commentId
            };
            article = new Article()
            {
                Title = "Doom Eternal is a masterpiece.",
                Text = "...",
                Id = articleId,
                OwnerUsername = username,
            };
            commentRepo = new Mock<ICommentRepository>(MockBehavior.Strict); 
            userService = new Mock<IUserService>();
            wordControl = new Mock<IWordControl>(MockBehavior.Strict);
            articleService = new Mock<IArticleService>(MockBehavior.Strict);
            service = new CommentService(commentRepo.Object, articleService.Object, userService.Object, wordControl.Object);
            commentRepo.Setup(c => c.Add(It.IsAny<Comment>())).Returns(comment);
            commentRepo.Setup(c => c.GetById(commentId)).Returns(comment);
            commentRepo.Setup(c => c.Exists(It.IsAny<string>())).Returns(false);
            commentRepo.Setup(c => c.Exists(commentId)).Returns(true);
            commentRepo.Setup(c => c.Delete(It.IsAny<string>()));
            commentRepo.Setup(c => c.Update(comment)).Returns(comment);
            userService.Setup(u => u.Exists(It.IsAny<string>())).Returns(false);
            userService.Setup(u => u.Exists(username)).Returns(true);
            userService.Setup(u => u.GetUserByUsername(username)).Returns(owner);
            userService.Setup(u => u.UpdateUser(It.IsAny<User>())).Callback<User>(u => owner = u).Returns(owner);
            userService.Setup(u => u.NotifyUser(It.IsAny<Notification>(), username))
                .Callback<Notification,string>((n, u) => owner.AddNotification(n));
            articleService.Setup(a => a.Exists(It.IsAny<string>())).Returns(false);
            articleService.Setup(a => a.Exists(articleId)).Returns(true);
            articleService.Setup(a => a.GetArticleById(It.IsAny<string>())).Throws(new Exception());
            articleService.Setup(a => a.GetArticleById(articleId)).Returns(article);
            articleService.Setup(a => a.UpdateArticle(article)).Returns(article);

            wordControl.Setup(w => w.CheckOffensiveWords(It.IsAny<string>())).Returns(new List<string>());
            wordControl.Setup(w => w.CheckOffensiveWords("OffensiveText")).Returns(new List<string>() { "OffensiveWord" });
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException), "Text is needed.")]
        public void CheckTextIsNotEmpty()
        {
            comment.Text = "";
            service.CreateComment(comment, article.Id);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException), "Owner is needed.")]
        public void CheckOwnerIsNotEmpty()
        {
            comment.OwnerUsername = "";
            service.CreateComment(comment, article.Id);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException), "Comment owner does not exist.")]
        public void CheckOwnerExists()
        {
            comment.OwnerUsername = "UnexistentUser";
            service.CreateComment(comment, article.Id);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException), "Content to comment unespecified.")]
        public void CheckArticleIsNotEmpty()
        {
            service.CreateComment(comment, "");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException), "Content to comment does not exist.")]
        public void CheckArticleExists()
        {
            service.CreateComment(comment, "00");
        }

        [TestMethod]
        public void CommentIdAttachedToIsTheContentId()
        {
            service.CreateComment(comment, article.Id);

            Assert.AreEqual(comment.IdAttachedTo, article.Id);
        }

        [TestMethod]
        public void ArticleAddsTheCommentToComments()
        {
            service.CreateComment(comment, article.Id);
            CollectionAssert.Contains(article.Comments, comment);
        }

        [TestMethod]
        public void CommentAddsTheCommentToComments()
        {
            service.CreateComment(comment, comment.Id);
            Assert.AreEqual(comment.Answer, comment);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException), "This comment can't be replied because already has a reply.")]
        public void CantReplyACommentAlreadyReplied()
        {
            comment.Answer = comment;
            service.CreateComment(comment, comment.Id);
        }

        [TestMethod]
        public void NotifyOwnerAboutNewComment()
        {
            service.CreateComment(comment, article.Id);
            Assert.AreEqual(1, owner.Notifications.Count);
        }

        [TestMethod]
        public void GetCommentById()
        {
            Comment result = service.GetCommentById(comment.Id);
            Assert.AreEqual(comment, result);
        }
        [TestMethod]
        public void UpdateComment()
        {
            Comment updatedComment = new Comment()
            {
                Id = commentId,
                Text = "Updated text.",
                OwnerUsername = username,
                IdAttachedTo = articleId
            };
            commentRepo.Setup(c => c.Update(It.IsAny<Comment>())).Callback<Comment>(c => comment = c).Returns(updatedComment);
            Comment result = service.UpdateComment(updatedComment, commentId);

            Assert.AreEqual(updatedComment, result);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException), "Text is needed.")]
        public void CheckTextIsNotEmptyOnUpdate()
        {
            Comment updatedComment = new Comment()
            {
                Id = commentId,
                Text = "",
                OwnerUsername = username,
                IdAttachedTo = articleId
            };
            service.UpdateComment(updatedComment, commentId);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException), "Owner is needed.")]
        public void CheckOwnerIsNotEmptyOnUpdate()
        {
            Comment updatedComment = new Comment()
            {
                Id = commentId,
                Text = "Updated text.",
                OwnerUsername = "",
                IdAttachedTo = articleId
            };
            service.UpdateComment(updatedComment, commentId);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException), "Comment owner does not exist.")]
        public void CheckOwnerExistsOnUpdate()
        {
            Comment updatedComment = new Comment()
            {
                Id = commentId,
                Text = "Updated text.",
                OwnerUsername = "UnexistentUser",
                IdAttachedTo = articleId
            };
            service.UpdateComment(updatedComment, commentId);
        }

        [TestMethod]
        public void DeleteComment()
        {
            service.DeleteCommentById(commentId);
        }

        [TestMethod]
        public void WhenVerifyingWordsOfNonOffensiveCommentIsNotSetToRevision()
        {
            service.VerifyWords(comment);
            Assert.IsFalse(comment.WaitingForRevision);
        }

        [TestMethod]
        public void WhenVerifyingWordsOfOffensiveCommentIsSetToRevision()
        {
            comment.Text = "OffensiveText";
            service.VerifyWords(comment);
            Assert.IsTrue(comment.WaitingForRevision);
        }

        [TestMethod]
        public void WhenVerifyingWordsOfOffensiveCommentAdminAndOwnerAreNotified()
        {
            comment.Text = "OffensiveText";
            service.VerifyWords(comment);
            userService.Verify(u => u.NotifyAdmins(It.IsAny<Notification>()), Times.Once);
            userService.Verify(u => u.NotifyUser(It.IsAny<Notification>(), It.IsAny<string>()), Times.Once);
        }
    }
}
