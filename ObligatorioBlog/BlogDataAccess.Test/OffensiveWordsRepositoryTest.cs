using BlogDataAccess.Repositories;
using BlogDomain;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogDataAccess.Test
{
    [TestClass]
    public class OffensiveWordsRepositoryTest
    {
        private string offensiveWord;
        private string[] offensiveWords;
        private OffensiveWordCollection offensiveWordCollection;
        private OffensiveWordsRepository repository;
        private List<OffensiveWordCollection> items;
        private Mock<DbSet<OffensiveWordCollection>> setOffensiveWords;
        private Mock<DbContext> context;

        [TestInitialize]
        public void Initialize()
        {
            offensiveWord = "offensiveWord";
            offensiveWords = new string[] { offensiveWord };
            offensiveWordCollection = new OffensiveWordCollection(offensiveWords);
            items = new List<OffensiveWordCollection>();
            setOffensiveWords = CreateMockSetOffensiveWords(items);
            context = new Mock<DbContext>();
            SetupContext();
            repository = new OffensiveWordsRepository(context.Object);
        }
        [TestMethod]
        public void AddOffensiveWord()
        {
            repository.Add(offensiveWordCollection);
            OffensiveWordCollection actual = items[0];
            Assert.AreEqual(offensiveWordCollection, actual);
        }
        [TestMethod]
        public void ThereIsOnlyOneOffensiveWordCollection()
        {
            repository.Add(offensiveWordCollection);
            repository.Add(offensiveWordCollection);
            int actual = items.Count;
            int expected = 1;
            Assert.AreEqual(expected, actual);
        }
        [TestMethod]
        public void DeleteEmptyOffensiveWordCollection()
        {
            repository.Delete(offensiveWordCollection);
            OffensiveWordCollection actual = items[0];
            OffensiveWordCollection expected = new OffensiveWordCollection();
            Assert.AreEqual(expected, actual);
        }
        [TestMethod]
        public void DeleteAndAddOffensiveWordTest()
        {
            repository.Add(offensiveWordCollection);
            repository.Delete(offensiveWordCollection);
            OffensiveWordCollection actual = items[0];
            OffensiveWordCollection expected = new OffensiveWordCollection();
            Assert.AreEqual(expected, actual);
        }
        [TestMethod]
        public void GetEmptyOffensiveWordCollectionTest()
        {
            OffensiveWordCollection actual = repository.Get();
            OffensiveWordCollection expected = new OffensiveWordCollection();
            Assert.AreEqual(expected, actual);
        }
        [TestMethod]
        public void GetOffensiveWordCollectionTest()
        {
            repository.Add(offensiveWordCollection);
            OffensiveWordCollection actual = repository.Get();
            Assert.AreEqual(offensiveWordCollection, actual);
        }


        private Mock<DbSet<OffensiveWordCollection>> CreateMockSetOffensiveWords(List<OffensiveWordCollection> items)
        {
            IQueryable<OffensiveWordCollection> data = items.AsQueryable();
            Mock<DbSet<OffensiveWordCollection>> mock = new Mock<DbSet<OffensiveWordCollection>>();
            mock.As<IQueryable<OffensiveWordCollection>>().Setup(m => m.Provider).Returns(data.Provider);
            mock.As<IQueryable<OffensiveWordCollection>>().Setup(m => m.Expression).Returns(data.Expression);
            mock.As<IQueryable<OffensiveWordCollection>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mock.As<IQueryable<OffensiveWordCollection>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());
            mock.Setup(m => m.Add(It.IsAny<OffensiveWordCollection>())).Callback<OffensiveWordCollection>((s) => items.Add(s));
            return mock;

        }

        private void SetupContext()
        {
            context.Setup(x => x.Set<OffensiveWordCollection>()).Returns(setOffensiveWords.Object);
            context.Setup(x => x.SaveChanges()).Returns(0);
        }
    }
}
