using BlogDomain;
using BlogDomain.DomainEnums;
using BlogImporterDomain;
using BlogServices.Interfaces;
using Exceptions;
using Importers.Interfaces;
using System.Reflection;

namespace BlogServices
{
    public class ImporterService : IImporterService
    {
        private readonly string IMPORTERS_PATH = "./Importers";

        public List<IArticleImporter?> GetArticleImporters() =>
            GetArticleImporters(IMPORTERS_PATH);

        public List<Article> ImportArticles(string importerName, List<Parameter> parameters)
        {
            IArticleImporter? importer = GetArticleImporterByName(importerName);
            try
            {
                return importer.Import(parameters).Select(ia => ToArticle(ia)).ToList();
            } catch (FileNotFoundException e)
            {
                throw new Exception("File not found.", e);
            }
        }

        private IArticleImporter? GetArticleImporterByName(string importerName)
        {
            try
            {
                return GetArticleImporters().First(i => i.GetName().Equals(importerName));
            } catch (Exception ex)
            {
                throw new ResourceNotFoundException("Importer not found.");
            }
        }

        private List<IArticleImporter> GetArticleImporters(string path)
        {
            List<IArticleImporter> importers = new List<IArticleImporter>();
            
            Type[] types = GetTypesFromPath(path)
                .Where(t => ImplementsInterface(t, typeof(IArticleImporter))).ToArray();

            foreach (Type type in types) 
                importers.Add(Activator.CreateInstance(type) as IArticleImporter);
            
            return importers.Where(i => i != null).ToList();
        }

        private Type[] GetTypesFromPath(string path)
        {
            Assembly[] assemblies = GetAssembliesFromPath(path);
            return GetTypesFromAssemblies(assemblies);
        }

        private Assembly[] GetAssembliesFromPath(string path)
            => Directory.GetFiles(path)
                .Where(f => f.EndsWith(".dll"))
                .Select(p => GetAssemblyFromPath(p))
                .ToArray();        

        private Assembly GetAssemblyFromPath(string path)
        {
            FileInfo info = new FileInfo(path);
            return Assembly.LoadFile(info.FullName);
        }

        private Type[] GetTypesFromAssemblies(Assembly[] assemblies)
            => assemblies.SelectMany(a => a.GetTypes()).ToArray();

        private bool ImplementsInterface(Type type, Type aInterface)
            => aInterface.IsAssignableFrom(type) && !type.IsInterface;

        private Article ToArticle(ImportedArticle article)
        {
            string templateName = Enum.GetName(typeof(ImportedArticleTemplate), article.Template);
            Enum.TryParse(templateName, out ArticleTemplate temp); 
            string visibilityName = Enum.GetName(typeof(ImportedVisibility), article.Visibility);
            Enum.TryParse(visibilityName, out Visibility vis);

            return new Article()
            {
                OwnerUsername = article.OwnerUsername,
                Title = article.Title,
                Text = article.Text,
                Visibility = vis,
                Images = article.Images.Select(i => new Image() { Content = i.Content}).ToList(),
                Template = temp
            };
        }        
    }
}
