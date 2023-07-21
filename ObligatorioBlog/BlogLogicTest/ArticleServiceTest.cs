using BlogDataAccess.Interfaces;
using BlogDomain;
using Moq;
using BlogDomain.DomainEnums;
using System;
using BlogServicesInterfaces;

namespace BlogServices.Test
{
    [TestClass]

    public class ArticleServiceTest
    {
        private Article article;
        private List<Article> articles;
        private Mock<IArticleRepository> articleRepo;
        private Mock<IUserService> userService;
        private Mock<IWordControl> wordControl;
        private ArticleService service;

        [TestInitialize]
        public void Initialize()
        {
            article = new Article()
            {
                OwnerUsername = "NotMartinFowler",
                Id = "107366ab-1a7d-4f25-a0c9-6c49e629f567",
                Template = ArticleTemplate.ImageAtTopLeft,
                Text = "Test driven development is pain.",
                Title = "What is TDD",
                Visibility = Visibility.Public,
                Images = new List<Image>() { new Image() { Content = "asd.png" } }
            };
            articles = new List<Article>();

            articleRepo = new Mock<IArticleRepository>(MockBehavior.Strict);
            userService = new Mock<IUserService>();
            wordControl = new Mock<IWordControl>(MockBehavior.Strict);
            service = new ArticleService(articleRepo.Object, userService.Object, wordControl.Object);
            articleRepo.Setup(a => a.Add(article)).Returns(article);
            articleRepo.Setup(a => a.GetById(article.Id)).Returns(article);
            articleRepo.Setup(a => a.DeleteById(It.IsAny<string>()));
            articleRepo.Setup(a => a.Update(article)).Returns(article);
            articleRepo.Setup(a => a.GetMultiple(It.IsAny<Func<Article, bool>>())).Returns(new Article[] { article });

            wordControl.Setup(w => w.CheckOffensiveWords(It.IsAny<string>())).Returns(new List<string>());
            wordControl.Setup(w => w.CheckOffensiveWords("OffensiveText")).Returns(new List<string>() { "OffensiveWord" });
        }

        [TestMethod]
        public void AddArticle()
        {
            Article result = service.AddArticle(article);
            Assert.AreEqual(article, result);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException), "Title is needed.")]
        public void AddArticleWithoutTitle()
        {
            article.Title = "";
            service.AddArticle(article);
        }

        [TestMethod]
        [ExpectedException(typeof(FormatException), "Title is needed.")]
        public void AddArticleWithoutVisibility()
        {
            article.Visibility = (Visibility)(-1);
            service.AddArticle(article);
        }

        [TestMethod]
        public void GetArticle()
        {
            Article result = service.GetArticleById(article.Id);
            Assert.AreEqual(article, result);
        }

        [TestMethod]
        public void DeleteArticleById()
        {
            service.DeleteArticleById(article.Id);
        }

        [TestMethod]
        public void UpdateArticle()
        {
            Article result = service.UpdateArticle(article);
            Assert.AreEqual(article, result);
        }

        [TestMethod]
        [ExpectedException(typeof(FormatException), "Article visibilty is not valid.")]
        public void UpdateArticleWithoutVisibility()
        {
            article.Visibility = (Visibility)(-1);
            service.UpdateArticle(article);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException), "Title is needed.")]
        public void UpdateArticleWithoutTitle()
        {
            article.Title = "";
            service.UpdateArticle(article);
        }

        [TestMethod]
        public void CheckIfNonOwnerUserIsOwnerOfArticle()
        {
            string username = "MartinFowler";
            bool result = service.IsOwner(article.Id, username);
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void CheckIfOwnerUserIsOwnerOfArticle()
        {
            string username = "NotMartinFowler";
            bool result = service.IsOwner(article.Id, username);
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void CheckIfAPrivateArticleIsPrivate()
        {
            article.Visibility = Visibility.Private;
            bool result = service.IsHidden(article.Id);
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void CheckIfAPublicArticleIsPrivate()
        {
            article.Visibility = Visibility.Public;
            bool result = service.IsHidden(article.Id);
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void GetArticlesByKeyword()
        {
            Article[] articles = service.GetArticlesByKeyword("TDD");
            Assert.AreEqual(articles[0], article);

        }

        [TestMethod]
        public void GetArticlesByKeywordWithLowerCase()
        {
            Article[] articles = service.GetArticlesByKeyword("tdd");
            Assert.AreEqual(articles[0], article);

        }

        [TestMethod]
        public void GetArticlesByKeywordWithSomeLowerCase()
        {
            Article[] articles = service.GetArticlesByKeyword("tDd");
            Assert.AreEqual(articles[0], article);

        }

        [TestMethod]
        public void GetLastArticles()
        {
            GenerateArticles(7);

            Article[] feed = service.GetFeed();

            Assert.AreEqual(articles.Count, feed.Length);
        }

        [TestMethod]
        public void FeedIsSorted()
        {
            GenerateArticles(5);

            Article[] feed = service.GetFeed();

            articles = articles.OrderByDescending(a => a.DateLog).ToList();
            CollectionAssert.AreEqual(articles, feed);
        }

        [TestMethod]
        public void FeedCantHaveMoreThan10Articles()
        {
            GenerateArticles(16);

            Article[] feed = service.GetFeed();

            Assert.AreEqual(10, feed.Length);
        }

        [TestMethod]
        public void FeedOnlyHasPublicArticles()
        {
            GenerateArticles(2);
            articles[0].Visibility = Visibility.Private;

            Article[] feed = service.GetFeed();

            Assert.AreEqual(1, feed.Length);
        }

        [TestMethod]
        public void WhenVerifyingWordsOfNonOffensiveArticleIsNotSetToRevision()
        {
            service.VerifyWords(article);
            Assert.IsFalse(article.WaitingForRevision);
        }

        [TestMethod]
        public void WhenVerifyingWordsOfOffensiveArticleIsSetToRevision()
        {
            article.Text = "OffensiveText";
            service.VerifyWords(article);
            Assert.IsTrue(article.WaitingForRevision);
        }

        [TestMethod]
        public void WhenVerifyingWordsOfOffensiveArticleAdminAndOwnerAreNotified()
        {
            article.Text = "OffensiveText";
            service.VerifyWords(article);
            userService.Verify(u => u.NotifyAdmins(It.IsAny<Notification>()), Times.Once);
            userService.Verify(u => u.NotifyUser(It.IsAny<Notification>(), It.IsAny<string>()), Times.Once);
        }

        [TestMethod]
        public void AddedImagesHaveArticleId()
        {
            service.AddArticle(article);
            Assert.IsTrue(article.Images.All(i => i.IdAttachedTo.Equals(article.Id)));
        }

        private void GenerateArticles(int amount)
        {
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
            articleRepo.Setup(a => a.GetAll()).Returns(articles.ToArray());
        } 

    }
}


