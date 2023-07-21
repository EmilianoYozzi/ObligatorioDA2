using BlogApplication.Controllers;
using BlogApplication.Models.In;
using BlogApplication.Models.Out;
using BlogDomain;
using BlogDomain.DomainEnums;
using BlogImporterDomain;
using BlogServices.Interfaces;
using BlogServicesInterfaces;
using Importers.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace BlogApplication.Test
{
    [TestClass]
    public class ArticleControllerTest
    {
        private Mock<IArticleService> service;
        private Mock<IImporterService> importerService;
        private ArticleController controller;
        private HttpContext context;

        private string articleId;
        private InModelArticle articleIn;
        private Article article;
        private User user;

        [TestInitialize]
        public void Initialize()
        {
            service = new Mock<IArticleService>(MockBehavior.Strict);
            importerService = new Mock<IImporterService>(MockBehavior.Strict);
            controller = new ArticleController(service.Object, importerService.Object);
            context = new DefaultHttpContext();
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = context
            };
            user = new User()
            {
                Username = "Emi",
                Role = UserRole.Blogger
            };
            context.Items["loggedUser"] = user;

            articleId = "12345";
            articleIn = new InModelArticle()
            {
                Title = "The Complexity of Redstone in Minecraft",
                Text = "...",
                Visibility = "public",
                Images = new List<string>() { "image.png" },
                Template = "Bottom",
                OwnerUsername = "oni"
            };
            article = new Article()
            {
                Title = "The Complexity of Redstone in Minecraft",
                Text = "...",
                Visibility = Visibility.Public,
                Images = new List<Image>() { new Image() { Content = "image.png" } },
                Template = ArticleTemplate.ImageAtBottom,
                OwnerUsername = "oni",
                Id = articleId
            };            
        }

        [TestMethod]
        public void AddArticle()
        {
            OutModelArticle expectedResult = new OutModelArticle(article);
            service.Setup(u => u.AddArticle(It.IsAny<Article>())).Returns(article);
            service.Setup(a => a.VerifyWords(It.IsAny<Article>()));

            IActionResult actionResult = controller.PostArticle(articleIn);

            service.VerifyAll();
            OkObjectResult? result = actionResult as OkObjectResult;
            Assert.AreEqual(expectedResult, result?.Value as OutModelArticle);
        }

        [TestMethod]
        public void GetArticle()
        {
            OutModelArticle expectedResult = new OutModelArticle(article);
            service.Setup(a => a.GetArticleById(It.IsAny<string>())).Returns(article);

            IActionResult actionResult = controller.GetArticleById(articleId);

            service.VerifyAll();
            OkObjectResult? result = actionResult as OkObjectResult;
            Assert.AreEqual(expectedResult, result?.Value as OutModelArticle);
        }

        [TestMethod]
        public void DeleteArticle()
        {
            service.Setup(a => a.DeleteArticleById(It.IsAny<String>()));

            IActionResult actionResult = controller.DeleteArticleById(articleId);

            service.VerifyAll();
            StatusCodeResult? result = actionResult as StatusCodeResult;
            Assert.AreEqual(200, result?.StatusCode);
        }

        [TestMethod]
        public void UpdateArticle()
        {
            OutModelArticle expectedResult = new OutModelArticle(article);
            service.Setup(u => u.UpdateArticle(It.IsAny<Article>())).Returns(article);
            service.Setup(a => a.VerifyWords(It.IsAny<Article>()));

            IActionResult actionResult = controller.UpdateArticle(articleIn, articleId);

            service.VerifyAll();
            OkObjectResult? result = actionResult as OkObjectResult;
            Assert.AreEqual(expectedResult, result?.Value as OutModelArticle);
        }

        [TestMethod]
        public void GetFeed()
        {
            Article[] articles = new Article[] { article };

            OutModelArticle[] expectedResult = new OutModelArticle[] { new OutModelArticle(articles[0]) };
            service.Setup(a => a.GetFeed()).Returns(articles);

            IActionResult actionResult = controller.GetFeed();

            service.VerifyAll();
            OkObjectResult? result = actionResult as OkObjectResult;
            CollectionAssert.AreEqual(expectedResult, result?.Value as OutModelArticle[]);
        }

        [TestMethod]
        public void VerifyWordsIsNotBeingCalledWhenAddingArticleAsAdmin()
        {
            user.Role = UserRole.Admin;

            OutModelArticle expectedResult = new OutModelArticle(article);
            service.Setup(u => u.AddArticle(It.IsAny<Article>())).Returns(article);

            IActionResult actionResult = controller.PostArticle(articleIn);

            service.Verify(s => s.VerifyWords(It.IsAny<Article>()), Times.Never);
        }

        [TestMethod]
        public void GetImporters()
        {
            OutModelImporter[] expected = new OutModelImporter[] 
            { 
                new OutModelImporter()
                {
                    Name = "JSON Importer",
                    Parameters = new List<OutModelParameter> { 
                        new OutModelParameter() { 
                            Name = "File name", 
                            Necessary = false, 
                            ParameterType = "Text" 
                        } 
                    }
                }
            };
            List<IArticleImporter> importers = new List<IArticleImporter>() 
            {
                MockImporter("JSON Importer").Object,
            };

            importerService.Setup(a => a.GetArticleImporters()).Returns(importers);

            IActionResult actionResult = controller.GetImporters();

            OkObjectResult? result = actionResult as OkObjectResult;
            CollectionAssert.AreEqual(expected, result?.Value as List<OutModelImporter>);
        }

        [TestMethod]
        public void ImportArticles()
        {
            InModelImport import = new InModelImport()
            {
                Name = "JSON Importer",
                Parameters = new List<InModelParameter> 
                { 
                    new InModelParameter() 
                    { 
                        Name = "File name", 
                        Value = "ArticlesLastWeek", 
                        ParameterType = "Text" 
                    }
                }
            };
            List<Article> articles = new List<Article>() { article };
            importerService.Setup(a => a.ImportArticles(It.IsAny<string>(), It.IsAny<List<Parameter>>())).Returns(articles);
            service.Setup(a => a.VerifyWords(It.IsAny<Article>()));
            service.Setup(u => u.AddArticle(It.IsAny<Article>())).Returns(article);
            List<OutModelArticle> expected = articles.Select(a => new OutModelArticle(a)).ToList();

            IActionResult actionResult = controller.Import(import);

            OkObjectResult? result = actionResult as OkObjectResult;
            CollectionAssert.AreEqual(expected, result?.Value as List<OutModelArticle>);
        }

        private Mock<IArticleImporter> MockImporter(string name)
        {
            List<Parameter> parameters = new List<Parameter>() { new Parameter() { Name = "File name" } };
            Mock<IArticleImporter> importer = new Mock<IArticleImporter>();
            importer.Setup(i => i.GetName()).Returns(name);
            importer.Setup(i => i.GetRequiredParameters()).Returns(parameters);
            return importer;
        }
    }
}