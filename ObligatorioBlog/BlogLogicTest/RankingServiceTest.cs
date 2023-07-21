using BlogDataAccess.Interfaces;
using BlogDomain;
using BlogServices;
using Moq;

namespace BlogServicesTest
{
    [TestClass]
    public class RankingServiceTest
    {
        private string username1;
        private string username2;
        private List<Article> articles;
        private Mock<IArticleRepository> articleRepo;
        private RankingService service;
        private DateRange range;
        private UserScore[] activityRanking;
        private UserScore[] offensesRanking;

        [TestInitialize]
        public void Initialize()
        {
            articles = new List<Article>();
            username1 = "Nobuwu";
            username2 = "Nobowo";
            activityRanking = new UserScore[] {
                new UserScore() { Username = username1, Points = 2 }, 
                new UserScore() { Username = username2, Points = 1 }
            };
            offensesRanking = new UserScore[]
            {
                new UserScore() { Username = username1, Points = 1 }
            };

            range = new DateRange()
            {
                Start = new DateTime(1986, 4, 18),
                End = DateTime.Now.AddDays(1)
            };

            articleRepo = new Mock<IArticleRepository>(MockBehavior.Strict);

            articleRepo.Setup(a => a.GetMultiple(It.IsAny<Func<Article, bool>>()))
                .Returns((Func<Article, bool> f) => articles.Where(f).ToArray());

            service = new RankingService(articleRepo.Object);
        }

        [TestMethod]
        public void GetActivityRankingWithNoActivity()
        {
            UserScore[] ranking = Array.Empty<UserScore>();

            UserScore[] result = service.GetUserActivityRanking(range);

            CollectionAssert.AreEqual(ranking, result);
        }

        [TestMethod]
        public void GetActivityRankingOnlyWithArticles()
        {

            articles = new List<Article>()
            {
                new Article() { OwnerUsername = username1 },
                new Article() { OwnerUsername = username2 },
                new Article() { OwnerUsername = username1 },
            };

            UserScore[] result = service.GetUserActivityRanking(range);

            CollectionAssert.AreEqual(activityRanking, result);
        }

        [TestMethod]
        public void GetActivityRankingWithArticlesAndComments()
        {
            articles = new List<Article>()
            {
                new Article() { 
                    OwnerUsername = username2,
                    Comments = new List<Comment>()
                    {
                        new Comment() { OwnerUsername = username1 },
                        new Comment() { OwnerUsername = username1 }
                    }
                },
            };

            UserScore[] result = service.GetUserActivityRanking(range);

            CollectionAssert.AreEqual(activityRanking, result);
        }

        [TestMethod]
        public void GetActivityRankingWithArticlesCommentsAndAnswers()
        {
            articles = new List<Article>()
            {
                new Article() {
                    OwnerUsername = username1,
                    Comments = new List<Comment>()
                    {
                        new Comment() 
                        { 
                            OwnerUsername = username2, 
                            Answer = new Comment()
                            {
                                OwnerUsername = username1,
                            }
                        }
                    }
                },
            };

            UserScore[] result = service.GetUserActivityRanking(range);

            CollectionAssert.AreEqual(activityRanking, result);
        }

        [TestMethod]
        public void GetOffensesRankingWithNoActivity()
        {
            UserScore[] ranking = Array.Empty<UserScore>();

            UserScore[] result = service.GetUserOffensesRanking(range);

            CollectionAssert.AreEqual(ranking, result);
        }

        [TestMethod]
        public void GetOffensesRankingOnlyWithArticles()
        {
            articles = new List<Article>()
            {
                new Article() { OwnerUsername = username1 },
                new Article() { OwnerUsername = username2 },
                new Article() { OwnerUsername = username1 },
            };
            articles[0].WaitingForRevision = true;

            UserScore[] result = service.GetUserOffensesRanking(range);

            CollectionAssert.AreEqual(offensesRanking, result);
        }

        [TestMethod]
        public void GetOffensesRankingWithArticlesAndComments()
        {
            Comment comment = new Comment() { OwnerUsername = username1 };
            comment.WaitingForRevision = true;
            articles = new List<Article>()
            {
                new Article() {
                    OwnerUsername = username2,
                    Comments = new List<Comment>()
                    {
                        comment,
                        new Comment() { OwnerUsername = username1 }
                    }
                },
            };

            UserScore[] result = service.GetUserOffensesRanking(range);

            CollectionAssert.AreEqual(offensesRanking, result);
        }

        [TestMethod]
        public void GetOffensesRankingWithArticlesCommentsAndAnswers()
        {
            Comment answer = new Comment() { OwnerUsername = username1 };
            answer.WaitingForRevision = true;
            articles = new List<Article>()
            {
                new Article() {
                    OwnerUsername = username1,
                    Comments = new List<Comment>()
                    {
                        new Comment()
                        {
                            OwnerUsername = username2,
                            Answer = answer
                        }
                    }
                },
            };

            UserScore[] result = service.GetUserOffensesRanking(range);

            CollectionAssert.AreEqual(offensesRanking, result);
        }
    }
}