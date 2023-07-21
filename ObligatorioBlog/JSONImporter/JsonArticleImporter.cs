using BlogImporterDomain;
using Importers.Interfaces;
using Newtonsoft.Json;

namespace JSONImporter
{
    public class JsonArticleImporter : IArticleImporter
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
            => "JSON Importer";

        public List<Parameter> GetRequiredParameters()
            => parameters;

        public List<ImportedArticle> Import(List<Parameter> parameters)
        {
            Parameter fileNameParam = parameters.Find(p => p.Name.Equals("File name"));
            string fileName = fileNameParam.Value.ToString();

            string json = File.ReadAllText("./JSONFiles/" + fileName + ".json");

            JsonArticleModel? model = JsonConvert.DeserializeObject<JsonArticleModel>(json);
            return model.Articles;
        }
    }
}