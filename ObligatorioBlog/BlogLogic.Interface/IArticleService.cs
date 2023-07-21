using BlogDomain;

namespace BlogServicesInterfaces
{
    public interface IArticleService
    {
        Article AddArticle(Article article);
        void DeleteArticleById(string articleId);
        Article GetArticleById(string articleId);
        Article UpdateArticle(Article article);
        bool IsOwner(string articleId, string username);
        bool IsHidden(string articleId);
        Article[] GetFeed();
        Article[] GetArticlesByKeyword(string keyword);
        bool Exists(string id);
        public void VerifyWords(Article article);
    } 
}