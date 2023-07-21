using BlogDataAccess.Interfaces;
using BlogDomain;
using Moq;

namespace BlogServices.Test
{

    [TestClass]
    public class OffensiveWordsServiceTest
    {
        string offensiveWord;
        string[] offensiveWords;
        OffensiveWordsService service;
        Mock<IOffensiveWordsRepository> offensiveWordRepo;
        OffensiveWordCollection offensiveWordCollection;

        [TestInitialize]
        public void Initialize()
        {
            offensiveWord = "offensiveWord";
            offensiveWords = new string[] { offensiveWord };
            offensiveWordRepo = new Mock<IOffensiveWordsRepository>(MockBehavior.Strict);
            service = new OffensiveWordsService(offensiveWordRepo.Object);
            offensiveWordCollection = new OffensiveWordCollection(offensiveWords);
            offensiveWordRepo.Setup(o => o.Add(It.IsAny<OffensiveWordCollection>())).Returns(offensiveWordCollection);
            offensiveWordRepo.Setup(o => o.Delete(It.IsAny<OffensiveWordCollection>()));
            offensiveWordRepo.Setup(o => o.Get()).Returns(offensiveWordCollection);
        }

        [TestMethod]
        public void AddOffensiveWords()
        {
            string[] result = service.AddOffensiveWords(offensiveWords);
            CollectionAssert.AreEqual(offensiveWords, result);
        }

        [TestMethod]
        public void DeleteOffensiveWords()
        {
            service.DeleteOffensiveWords(offensiveWords);
        }

        [TestMethod]
        public void GetOffensiveWords()
        {
            string[] result = service.GetOffensiveWords();
            CollectionAssert.AreEqual(offensiveWords, result);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException), "Offensive word is needed.")]
        public void AddOffensiveWordWithoutWord()
        {
            string[] offensiveWords = new string[] { "" };
            service.AddOffensiveWords(offensiveWords);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException), "Offensive word is needed.")]
        public void AddOffensiveWordWithNull()
        {
            string[] offensiveWords = new string[] { offensiveWord, "" };
            service.AddOffensiveWords(offensiveWords);
        }

        [TestMethod]
        [ExpectedException(typeof(FormatException), "Offensive word cannot contain spaces.")]
        public void AddOffensiveWordWithSpace()
        {
            string[] offensiveWords = new string[] { "offensive Word" };
            service.AddOffensiveWords(offensiveWords);
        }
    }
}
