using BlogDataAccess.Interfaces;
using BlogDomain;
using Moq;

namespace BlogServices.Test
{
    [TestClass]

    public class WordControlTest
    {
        private string text;
        private OffensiveWordCollection words;
        private Mock<IOffensiveWordsRepository> wordRepo;
        private WordControl control;

        [TestInitialize]
        public void Initialize()
        {
            words = new OffensiveWordCollection() { offensiveWords = new List<string>() { "Moron", "Shit" }};
            wordRepo = new Mock<IOffensiveWordsRepository>(MockBehavior.Strict);
            control = new WordControl(wordRepo.Object);
            wordRepo.Setup(a => a.Get()).Returns(words);
        }

        [TestMethod]
        public void CheckANonOffensiveText()
        {
            text = "Great Post! Have a nice day!";
            List<string> expected = new List<string>();
            List<string> result = control.CheckOffensiveWords(text);
            
            CollectionAssert.AreEqual(expected, result);
        }

        [TestMethod]
        public void CheckAnOffensiveText()
        {
            text = "Shut up Moron.";
            List<string> expected = new List<string>() { "Moron" };
            List<string> result = control.CheckOffensiveWords(text);

            CollectionAssert.AreEqual(expected, result);
        }

        [TestMethod]
        public void CheckATextWithMultipleOffensiveWords()
        {
            text = "They are all Morons. Dont know Shit!";
            List<string> expected = new List<string>() { "Moron", "Shit" };
            List<string> result = control.CheckOffensiveWords(text);

            CollectionAssert.AreEquivalent(expected, result);
        }

        [TestMethod]
        public void DetectOffensiveWordsInDifferentCase()
        {
            text = "mORon";
            List<string> expected = new List<string>() { "Moron" };
            List<string> result = control.CheckOffensiveWords(text);

            CollectionAssert.AreEquivalent(expected, result);
        }

        [TestMethod]
        public void DetectOffensiveWordsSeparatedWithSpaces()
        {
            text = "Your blog is s h i t.";
            List<string> expected = new List<string>() { "Shit" };
            List<string> result = control.CheckOffensiveWords(text);

            CollectionAssert.AreEquivalent(expected, result);
        }

        [TestMethod]
        public void DetectOffensiveWordsWithNonAlphanumericCharsInTheMiddle()
        {
            text = "Your blog is s-h*i.t.";
            List<string> expected = new List<string>() { "Shit" };
            List<string> result = control.CheckOffensiveWords(text);

            CollectionAssert.AreEquivalent(expected, result);
        }
    }
}


