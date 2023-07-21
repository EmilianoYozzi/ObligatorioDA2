using BlogDataAccess.Interfaces;
using BlogDomain;
using Exceptions;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Linq.Expressions;

namespace BlogDataAccess.Repositories
{
    public class SessionRepository : ISessionRepository
    {
        private readonly DbContext dbContext;

        public SessionRepository(DbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public Session Add(LogInInfo info)
        {
            try
            {
                Session session = new Session()
                {
                    Username = info.Username,
                    Token = Guid.NewGuid()
                };
                dbContext.Set<Session>().Add(session);
                dbContext.SaveChanges();
                return session;
            }
            catch (Exception e)
            {
                throw new UnexpectedDataAccessException(e);
            }
        }

        public Session Get(Func<Session, bool> func)
        {
            try
            {
                Func<Session, bool> combined = s => !s.Deleted && func(s);
                return dbContext.Set<Session>().First(combined);
            }
            catch (InvalidOperationException e)
            {
                throw new ResourceNotFoundException("Session not found.");
            }
            catch (Exception e)
            {
                throw new UnexpectedDataAccessException(e);
            }
        }

        public bool Exists(Func<Session, bool> func)
        {
            try
            {
                Func<Session, bool> combined = s => !s.Deleted && func(s);
                return dbContext.Set<Session>().Any(combined);
            }
            catch (Exception e)
            {
                throw new UnexpectedDataAccessException(e);
            }
        }
                         
        public void Delete(Func<Session, bool> func)
        {
            try
            {
                Session session = Get(func);
                session.Deleted = true;
                dbContext.SaveChanges();
            }
            catch (InvalidOperationException e)
            {
                throw new ResourceNotFoundException("Token not found.");
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

        public void DeleteByToken(Guid token) => Delete(s => s.Token.Equals(token));

        public Session GetByToken(Guid token) => Get(s => s.Token.Equals(token));
        
        public bool Exists(Guid Token) => Exists(s => s.Token.Equals(Token));
    }
}
