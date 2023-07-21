using BlogDataAccess.Repositories;
using BlogDomain;
using BlogDomain.DomainEnums;
using Exceptions;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace BlogDataAccess.Test
{

    [TestClass]
    public class ArticleRepositoryTests
    {
        private Article article;
        private Article otherArticle;
        private List<Article> items;
        private ArticleRepository repository;
        private Mock<DbSet<Article>> setArticles;
        private Mock<DbContext> context;
        private string wrongId;

        [TestInitialize]
        public void Initialize()
        {
            items = new List<Article>();
            setArticles = CreateMockSetArticle(items);
            context = new Mock<DbContext>();
            SetupContext();
            repository = new ArticleRepository(context.Object);
            article = new Article()
            {
                Title = "Electro",
                Text = "Electro",
                Images = new List<Image>() { new Image() { Content = "image.png" } },
                Template = ArticleTemplate.ImageAtBottom,
                OwnerUsername = "YaeMiko",
                Id = "123"
            };
            otherArticle = new Article()
            {
                Title = "Pyro",
                Text = "Pyro",
                Images = new List<Image>() { new Image() { Content = "image.png" } },
                Template = ArticleTemplate.ImageAtTop,
                OwnerUsername = "Yoimiya",
                Id = "321"
            };
            wrongId = "555";
        }

        [TestMethod]
        public void GetArticleById()
        {
            items.Add(article);

            Article requestedArticle = repository.GetById(article.Id);

            Assert.AreEqual(article, requestedArticle);
        }

        [TestMethod]
        [ExpectedException(typeof(ResourceNotFoundException), "Article not found.")]
        public void GetArticleUsingWrongId()
        {
            items.Add(article);
            repository.GetById(wrongId);            
        }

        [TestMethod]
        public void DeleteArticle()
        {
            items.Add(article);

            repository.DeleteById(article.Id);
            Article deletedArticle = context.Object.Set<Article>().First(a => a.Id.Equals(article.Id));

            Assert.IsTrue(deletedArticle.Deleted);
        }

        [TestMethod]
        public void GetMultipleArticles()
        {
            items.Add(article);
            items.Add(otherArticle);

            Article[] articles = repository.GetMultiple(a => a.Id.Equals(article.Id) || a.Id.Equals(otherArticle.Id));

            CollectionAssert.AreEqual(items, articles);
        }

        [TestMethod]
        public void GetEmptyMultipleArticles()
        {
            Article[] articles = repository.GetMultiple(a => a.Id.Equals("pepe"));

            CollectionAssert.AreEqual(new List<Article>(), articles);
        }

        [TestMethod]
        [ExpectedException(typeof(ResourceNotFoundException), "Article not found.")]
        public void DeleteArticleUsingWrongId()
        {
            repository.DeleteById(wrongId);
        }

        [TestMethod]
        public void CheckIfExistentArticleExists()
        {
            items.Add(article);

            bool result = repository.Exists(article.Id);

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void CheckIfNonExistentArticleExists()
        {
            bool result = repository.Exists(article.Id);

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void UpdateArticle()
        {
            items.Add(article);
            otherArticle.Id = article.Id;
            Article expectedArticle = new Article()
            {
                Title = "Pyro",
                Text = "Pyro",
                Images = new List<Image>() { new Image() { Content = "image.png" } },
                Template = ArticleTemplate.ImageAtTop,
                OwnerUsername = "YaeMiko",
                Id = "123",
            };
            expectedArticle.Update(expectedArticle);

            repository.Update(otherArticle);
            Article articleFromSet = context.Object.Set<Article>().First(a => a.Id.Equals(article.Id));

            Assert.AreEqual(expectedArticle, articleFromSet);
        }

        [TestMethod]
        [ExpectedException(typeof(ResourceNotFoundException), "Article not found.")]
        public void UpdateArticleWithWrongId()
        {
            items.Add(article);
            repository.Update(otherArticle);
        }

        [TestMethod]
        [ExpectedException(typeof(ResourceNotFoundException), "Article not found.")]
        public void UpdateNonExistentArticle()
        {
            repository.Update(article);
        }

        [TestMethod]
        [ExpectedException(typeof(UnexpectedDataAccessException))]
        public void UpdateArticleWithSqlException()
        {
            items.Add(article);
            context.Setup(x => x.SaveChanges()).Throws(MakeSqlException());

            repository.Update(article);
        }

        [TestMethod]
        public void AddArticle()
        {
            repository.Add(article);

            Article articleFromSet = context.Object.Set<Article>().First(a => a.Id.Equals(article.Id));
            Assert.AreEqual(article, articleFromSet);
        }

        [TestMethod]
        [ExpectedException(typeof(UnexpectedDataAccessException))]
        public void AddArticleWithSqlException()
        {
            setArticles.Setup(x => x.Add(It.IsAny<Article>())).Throws(MakeSqlException());

            repository.Add(article);
        }

        [TestMethod]
        public void GetAllArticles()
        {
            items.Add(article);
            Article[] articles = repository.GetAll();
            CollectionAssert.AreEqual(items, articles);
        }

        private Mock<DbSet<Article>> CreateMockSetArticle(List<Article> items)
        {
            IQueryable<Article> data = items.AsQueryable();
            Mock<DbSet<Article>> mock = new Mock<DbSet<Article>>();
            mock.As<IQueryable<Article>>().Setup(m => m.Provider).Returns(data.Provider);
            mock.As<IQueryable<Article>>().Setup(m => m.Expression).Returns(data.Expression);
            mock.As<IQueryable<Article>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mock.As<IQueryable<Article>>().Setup(m => m.GetEnumerator()).Returns(() => data.GetEnumerator());
            mock.Setup(m => m.Add(It.IsAny<Article>())).Callback<Article>((s) => items.Add(s));
            return mock;
        }
        private void SetupContext()
        {
            context.Setup(x => x.Set<Article>()).Returns(setArticles.Object);
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