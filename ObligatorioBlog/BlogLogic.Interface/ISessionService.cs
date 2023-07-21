using BlogDomain;

namespace BlogServicesInterfaces
{
    public interface ISessionService
    {
        User GetUserByToken(Guid token);
        bool IsValidToken(Guid token);
        Session LogIn(LogInInfo info);
        void LogOut(Guid token);
    }
}