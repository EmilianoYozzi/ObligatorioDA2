using BlogApplication.Controllers;
using BlogServices.Interfaces;
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
    public class OffensiveWordsControllerTest
    {
        private OffensiveWordsController controller;
        private Mock<IOffensiveWordsService> services;
        string offensiveWord;
        string[] offensiveWords;

        [TestInitialize]
        public void SetUp()
        {
            services = new Mock<IOffensiveWordsService>(MockBehavior.Strict);
            controller = new OffensiveWordsController(services.Object);
            offensiveWord = "offensiveWord";
            offensiveWords = new string[] { offensiveWord };
            
        }
        [TestMethod]
        public void AddOffensiveWords()
        {
            services.Setup(u => u.AddOffensiveWords(It.IsAny<string[]>())).Returns(offensiveWords);
            IActionResult actionResult = controller.AddOffensiveWord(offensiveWords);
            services.VerifyAll();
            OkObjectResult? result = actionResult as OkObjectResult;
            CollectionAssert.AreEqual(offensiveWords, result?.Value as string[]);
           

        }
        [TestMethod]
        public void GetOffensiveWords()
        {
            services.Setup(u => u.GetOffensiveWords()).Returns(offensiveWords);
            IActionResult actionResult = controller.GetOffensiveWords();
            services.VerifyAll();
            OkObjectResult? result = actionResult as OkObjectResult;
            CollectionAssert.AreEqual(offensiveWords, result?.Value as string[]);
        }

        [TestMethod]
        public void DeleteOffensiveWords()
        {
            services.Setup(u => u.DeleteOffensiveWords(It.IsAny<string[]>()));
            IActionResult actionResult = controller.DeleteOffensiveWord(offensiveWords);
            services.VerifyAll();
            StatusCodeResult? result = actionResult as StatusCodeResult;
            Assert.AreEqual(200, result?.StatusCode);
        }

    }
}
