using BlogImporterDomain;
using XMLImporter;

namespace BlogImporters.Test
{
    [TestClass]
    public class XMLArticleImporterTest
    {
        private XmlArticleImporter importer;
        private List<Parameter> parameters;

        [TestInitialize]
        public void Initialize()
        {
            importer = new XmlArticleImporter();
            parameters = new List<Parameter>()
            {
                new Parameter()
                {
                    Name = "File name",
                    Necessary = true,
                    ParameterType = ParameterType.Text,
                }
            };
        }

        [TestMethod]
        public void GetName()
        {
            Assert.AreEqual("XML Importer", importer.GetName());
        }

        [TestMethod]
        public void GetRequiredParams()
        {
            List<Parameter> expected = parameters;
            List<Parameter> actual = importer.GetRequiredParameters();
            CollectionAssert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ImportArticles()
        {
            List<ImportedArticle> expected = new List<ImportedArticle>()
            {
                new ImportedArticle()
                {
                    Title = "Article1",
                    Text= "This is the text of Article1.",
                    OwnerUsername = "SupremeAdmin",
                    Template = ImportedArticleTemplate.NoImage,
                    Visibility = ImportedVisibility.Public,
                },
                new ImportedArticle()
                {
                    Title = "Article2",
                    Text= "This is the text of Article2.",
                    OwnerUsername = "Blogger",
                    Template = ImportedArticleTemplate.NoImage,
                    Visibility = ImportedVisibility.Private,
                }
            };
            parameters[0].Value = "test";
            List<ImportedArticle> actual = importer.Import(parameters);
            CollectionAssert.AreEqual(expected, actual);
        }
    }
}