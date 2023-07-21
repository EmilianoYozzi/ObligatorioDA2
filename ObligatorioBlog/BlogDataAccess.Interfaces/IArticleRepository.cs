using BlogDomain;

namespace BlogDataAccess.Interfaces
{
    public interface IArticleRepository
    {
        Article Add(Article article);
        Article Update(Article article);
        Article Get(Func<Article, bool> func);
        void Delete(Func<Article, bool> func);
        bool Exists(Func<Article, bool> func);
        Article GetById(string id);
        void DeleteById(string id);
        bool Exists(string id);
        Article[] GetMultiple(Func<Article, bool> func);
        Article[] GetAll();
    }
}
