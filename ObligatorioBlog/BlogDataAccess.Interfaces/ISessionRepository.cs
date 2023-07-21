using BlogDomain;

namespace BlogDataAccess.Interfaces
{
    public interface ISessionRepository
    {
        Session Add(LogInInfo loginInfo);
        Session Get(Func<Session, bool> func);
        void Delete(Func<Session, bool> func);
        bool Exists(Func<Session, bool> func);
        void DeleteByToken(Guid token);
        Session GetByToken(Guid token);
        bool Exists(Guid token);
    }
}
