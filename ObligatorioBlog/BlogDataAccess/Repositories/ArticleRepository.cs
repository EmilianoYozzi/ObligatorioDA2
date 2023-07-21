using BlogDataAccess.Interfaces;
using BlogDomain;
using Exceptions;
using Microsoft.EntityFrameworkCore;

namespace BlogDataAccess.Repositories
{
    public class ArticleRepository : IArticleRepository
    {
        private readonly DbContext dbContext;

        public ArticleRepository(DbContext context)
        {
            dbContext = context;
        }

        public Article Add(Article article)
        {
            try
            {
                dbContext.Set<Article>().Add(article);
                dbContext.SaveChanges();
                return article;
            }
            catch (Exception e)
            {
                throw new UnexpectedDataAccessException(e);
            }
        }

        public Article Update(Article article)
        {
            try
            {
                Article articleToUpdate = GetById(article.Id);
                articleToUpdate.Update(article);
                dbContext.SaveChanges();
                return articleToUpdate;
            }
            catch (ResourceNotFoundException e)
            {
                throw e;
            }
            catch (Exception e)
            {
                throw new UnexpectedDataAccessException(e);
            }
        }

        public void Delete(Func<Article, bool> func)
        {
            try
            {
                Article article = Get(func);
                article.Deleted = true;
                dbContext.SaveChanges();
            }
            catch (ResourceNotFoundException e)
            {
                throw e;
            }
            catch (Exception ex)
            {
                throw new UnexpectedDataAccessException(ex);
            }
        }

        public Article Get(Func<Article, bool> func)
        {
            try
            {
                Func<Article, bool> combined = a => !a.Deleted && func(a);
                return dbContext.Set<Article>()
                        .Include(a => a.Images)
                        .Include(a => a.Comments)
                        .ThenInclude(c => c.Answer)
                        .First(combined);
            }
            catch (InvalidOperationException e)
            {
                throw new ResourceNotFoundException("Article not found.");
            }
            catch (Exception e)
            {
                throw new UnexpectedDataAccessException(e);
            }
        }

        public bool Exists(Func<Article, bool> func)
        {
            try
            {
                Func<Article, bool> combined = a => !a.Deleted && func(a);
                return dbContext.Set<Article>().Any(combined);
            }
            catch (Exception ex)
            {
                throw new UnexpectedDataAccessException(ex);
            }
        }

        public Article[] GetMultiple(Func<Article, bool> func)
        {
            
            Func<Article, bool> combined = a => !a.Deleted && func(a);
            return dbContext.Set<Article>()
                    .Include(a => a.Images)
                    .Include(a => a.Comments)
                    .Where(combined)
                    .ToArray();
        }

        public Article GetById(string id) => Get(a => a.Id.Equals(id));

        public bool Exists(string id) => Exists(a => a.Id.Equals(id));

        public void DeleteById(string id) => Delete(a => a.Id.Equals(id));

        public Article[] GetAll()
            => GetMultiple(a => true);
    }
}
