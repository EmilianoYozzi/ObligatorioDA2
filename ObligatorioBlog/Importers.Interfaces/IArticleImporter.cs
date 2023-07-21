using BlogImporterDomain;

namespace Importers.Interfaces
{
    public interface IArticleImporter
    {
        string GetName();
        List<ImportedArticle> Import(List<Parameter> parameters);
        List<Parameter> GetRequiredParameters();
    }
}