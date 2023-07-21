using BlogDataAccess.Interfaces;
using BlogServicesInterfaces;
using BlogDomain;
using BlogDomain.DomainInterfaces;

namespace BlogServices
{
    public class RankingService : IRankingService
    {
        private IArticleRepository articleRepo;

        private readonly Func<IContent, bool> ACTIVITY_CRITERIA = c => true;
        private readonly Func<IContent, bool> OFFENSE_CRITERIA = c => c.HadOffensiveWords();

        public RankingService( IArticleRepository articleRepo)
        {
            this.articleRepo = articleRepo;
        }

        public UserScore[] GetUserActivityRanking(DateRange range)
            => GetUserRanking(range, ACTIVITY_CRITERIA);

        public UserScore[] GetUserOffensesRanking(DateRange range)
            => GetUserRanking(range, OFFENSE_CRITERIA);

        private UserScore[] GetUserRanking(DateRange range, Func<IContent, bool> predicate)
        {
            Dictionary<string, int> points = new Dictionary<string, int>();
            Article[] articles = GetArticlesBetweenDateRange(range);

            foreach (Article article in articles)
                SetContentPoints(points, article, predicate);

            UserScore[] ranking = points.Select(p => new UserScore()
            {
                Username = p.Key,
                Points = p.Value
            }).ToArray();

            return ranking.OrderByDescending(u => u.Points).ToArray();
        }

        private Article[] GetArticlesBetweenDateRange(DateRange range)
            => articleRepo.GetMultiple(a =>
                range.Start <= a.DateLog.LastModification &&
                range.End >= a.DateLog.LastModification);
        
        private void SetContentPoints(Dictionary<string, int> score, Article article, Func<IContent, bool> predicate)
        {
            if (predicate(article))
                AddPoint(score, article.OwnerUsername);

            foreach (Comment comment in article.Comments)
                SetCommentContentPoints(score, comment, predicate);
        }

        private void SetCommentContentPoints(Dictionary<string, int> score, Comment comment, Func<IContent, bool> predicate)
        {
            if (predicate(comment))
                AddPoint(score, comment.OwnerUsername);

            Comment answer = comment.Answer;

            if (answer != null && predicate(answer))
                AddPoint(score, answer.OwnerUsername);
        }

        private void AddPoint(Dictionary<string, int> score, string key)
        {
            if (score.ContainsKey(key))
                score[key]++;
            else
                score.Add(key, 1);
        }
    }
}