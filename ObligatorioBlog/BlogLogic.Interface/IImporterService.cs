using BlogDomain;
using BlogImporterDomain;
using Importers.Interfaces;

namespace BlogServices.Interfaces
{
    public interface IImporterService
    {
        List<IArticleImporter> GetArticleImporters();
        List<Article> ImportArticles(string importerName, List<Parameter> parameters);
    }
}
