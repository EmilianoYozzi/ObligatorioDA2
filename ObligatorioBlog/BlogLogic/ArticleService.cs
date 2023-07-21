using BlogDataAccess.Interfaces;
using BlogDomain;
using BlogDomain.DomainEnums;
using BlogServicesInterfaces;

namespace BlogServices
{
    public class ArticleService : IArticleService
    {
        private IArticleRepository articleRepo;
        private IWordControl wordControl;
        private IUserService userService;
        private const int FEED_SIZE = 10;

        public ArticleService(IArticleRepository articleRepo, IUserService userService, IWordControl control)
        {
            this.articleRepo = articleRepo;
            this.wordControl = control;
            this.userService = userService;
        }

        public Article AddArticle(Article article)
        {
            Verify(article);
            return articleRepo.Add(article);
        }

        private void Verify(Article article)
        {
            VerifyTitle(article);
            VerifyVisibility(article);
        }

        private void VerifyVisibility(Article article)
        {
            Visibility visibility = article.Visibility;
            if (!(visibility == Visibility.Public || visibility == Visibility.Private))
                throw new FormatException("Article visibilty is not valid.");
        }

        private void VerifyTitle(Article article)
        {
            if (string.IsNullOrEmpty(article.Title))
                throw new ArgumentException("Title is needed.");
        }

        public void VerifyWords(Article article)
        {
            List<string> offensiveWordsTitle = wordControl.CheckOffensiveWords(article.Title);
            List<string> offensiveWordsBody = wordControl.CheckOffensiveWords(article.Text);

            if (offensiveWordsTitle.Count + offensiveWordsBody.Count > 0)
            {
                article.WaitingForRevision = true;
                string message = "Offensive words detected." + 
                    " Title: " + string.Join(", ", offensiveWordsTitle) + "." +
                    " Body: " + string.Join(", ", offensiveWordsBody) + ".";
                
                Notification notification = new Notification() 
                { 
                    Message = message,
                    Uri = "articles/" + article.Id,
                };
                userService.NotifyAdmins(notification);
                userService.NotifyUser(notification, article.OwnerUsername);
            }
        }

        public void DeleteArticleById(string articleId)
        {
            articleRepo.DeleteById(articleId);
        }

        public Article UpdateArticle(Article article)
        {
            Verify(article);            
            return articleRepo.Update(article);
        }

        public Article GetArticleById(string articleId)
        {
            return articleRepo.GetById(articleId);
        }

        public bool IsOwner(string articleId, string username)
        {
            Article article = articleRepo.GetById(articleId);
            return article.OwnerUsername.Equals(username);
        }

        public bool IsHidden(string contentId)
        {
            Article article = articleRepo.GetById(contentId);
            return article.IsHidden();
        }

        public Article[] GetFeed()
        {
            Article[] articles = articleRepo.GetAll()
                .OrderByDescending(a => a.DateLog)
                .Where(a => !a.IsHidden())
                .ToArray();
            return articles.Take(FEED_SIZE).ToArray();
        }

        public Article[] GetArticlesByKeyword(string keyword)
        {
            string keywordLower = keyword.ToLower();
            return articleRepo.GetMultiple(a => a.Text.ToLower().Contains(keywordLower) || a.Title.ToLower().Contains(keywordLower));
        }

        public bool Exists(string id)
            => articleRepo.Exists(id);

    }

}
