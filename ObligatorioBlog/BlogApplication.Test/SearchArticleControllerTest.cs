using BlogApplication.Controllers;
using BlogApplication.Models.Out;
using BlogDomain;
using BlogDomain.DomainEnums;
using BlogServicesInterfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogApplication.Test
{
    [TestClass]
    public class SearchArticleControllerTest
    {
        private Mock<IArticleService> service;
        private SearchArticleController controller;
        string keyword;
        Article article;

        [TestInitialize]
        public void Initialize()
        {
            service = new Mock<IArticleService>(MockBehavior.Strict);
            controller = new SearchArticleController(service.Object);
            article = new Article()
            {
                Title = "The Complexity of Redstone in Minecraft",
                Text = "...",
                Visibility = Visibility.Public,
                Images = new List<Image>() { new Image() { Content = "image.png" } },
                Template = ArticleTemplate.ImageAtBottom,
                OwnerUsername = "oni",
                Id = "1"
            };

        }

        [TestMethod]
        public void SearchArticle()
        {
            Article[] articles = new Article[] { article };

            OutModelArticle[] expectedResult = new OutModelArticle[] { new OutModelArticle(articles[0]) };
            service.Setup(a => a.GetArticlesByKeyword(It.IsAny<string>())).Returns(articles);

            IActionResult actionResult = controller.GetArticlesByKeyword("Redstone");

            service.VerifyAll();
            OkObjectResult? result = actionResult as OkObjectResult;
            CollectionAssert.AreEqual(expectedResult, result?.Value as OutModelArticle[]);
        }
    }
}
