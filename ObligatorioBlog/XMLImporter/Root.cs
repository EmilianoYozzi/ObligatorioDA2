using BlogImporterDomain;
using System.Xml.Serialization;

namespace XMLImporter
{
    public class Root
    {
        public Root root { get; set; }
        public List<ImportedArticle> Articles { get; set; }
    }
}
