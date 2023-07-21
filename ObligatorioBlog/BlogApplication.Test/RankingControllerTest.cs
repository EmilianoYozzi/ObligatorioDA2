using BlogApplication.Controllers;
using BlogApplication.Models.Out;
using BlogDomain;
using BlogServicesInterfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace BlogApplication.Test
{
    [TestClass]
    public class RankingControllerTest
    {
        private RankingController controller;
        private Mock<IRankingService> service;

        private UserScore[] userScores;
        private DateRange range;

        [TestInitialize]
        public void Initialize()
        {
            service = new Mock<IRankingService>(MockBehavior.Strict);
            controller = new RankingController(service.Object);

            userScores = new UserScore[] 
            { 
                new UserScore() { Username = "OniTheDemon", Points = 37 },
                new UserScore() { Username = "Titolandia22", Points = 29 }
            };
            range = new DateRange()
            {
                Start = new DateTime(2022, 4, 15),
                End = new DateTime(2023, 7, 2)
            };
        }

        [TestMethod]
        public void GetActivityRanking()
        {
            OutModelUserScore[] expectedResult = userScores.Select(n => new OutModelUserScore(n)).ToArray();
            service.Setup(u => u.GetUserActivityRanking(It.IsAny<DateRange>())).Returns(userScores);

            IActionResult actionResult = controller.GetUserActivityRanking(range);

            service.VerifyAll();
            OkObjectResult result = actionResult as OkObjectResult;
            CollectionAssert.AreEqual(expectedResult, result.Value as OutModelUserScore[]);
            Assert.AreEqual(200, result.StatusCode);
        }

        [TestMethod]
        public void GetOffensesRanking()
        {
            OutModelUserScore[] expectedResult = userScores.Select(n => new OutModelUserScore(n)).ToArray();
            service.Setup(u => u.GetUserOffensesRanking(It.IsAny<DateRange>())).Returns(userScores);

            IActionResult actionResult = controller.GetUserOffensesRanking(range);

            service.VerifyAll();
            OkObjectResult result = actionResult as OkObjectResult;
            CollectionAssert.AreEqual(expectedResult, result.Value as OutModelUserScore[]);
            Assert.AreEqual(200, result.StatusCode);
        }
    }
}