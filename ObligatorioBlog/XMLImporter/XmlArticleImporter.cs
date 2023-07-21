using BlogImporterDomain;
using Importers.Interfaces;
using Newtonsoft.Json;
using System.Xml.Linq;

namespace XMLImporter
{
    public class XmlArticleImporter : IArticleImporter
    {
        private List<Parameter> parameters = new List<Parameter>()
            {
                new Parameter()
                {
                    Name = "File name",
                    Necessary = true,
                    ParameterType = ParameterType.Text
                }
            };

        public string GetName()
            => "XML Importer";

        public List<Parameter> GetRequiredParameters()
            => parameters;

        public List<ImportedArticle> Import(List<Parameter> parameters)
        {
            Parameter fileNameParam = parameters.Find(p => p.Name.Equals("File name"));
            string fileName = fileNameParam.Value.ToString();

            string xml = File.ReadAllText("./XMLFiles/" + fileName + ".xml");
            var doc = XDocument.Parse(xml);
            string json = JsonConvert.SerializeXNode(doc.Root);

            Root? model = JsonConvert.DeserializeObject<Root>(json);
            return model.root.Articles;
        }
    }
}