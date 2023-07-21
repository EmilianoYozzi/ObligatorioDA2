using BlogDataAccess.Interfaces;
using BlogDomain;
using BlogServicesInterfaces;

namespace BlogServices
{
    public class SessionService : ISessionService
    {
        private readonly ISessionRepository sessionRepository;
        private readonly IUserRepository userRepository;
        public SessionService(ISessionRepository data, IUserRepository userRepository)
        {
            this.sessionRepository = data;
            this.userRepository = userRepository;
        }

        public User GetUserByToken(Guid token)
        {
            Session session = sessionRepository.Get(s => s.Token.Equals(token));
            User user = userRepository.GetByUsername(session.Username);
            return user;
        }

        public bool IsValidToken(Guid token)
        {
            return sessionRepository.Exists(token);
        }

        public Session LogIn(LogInInfo info)
        {
            VerifyLoginInfoIsNotEmpty(info);
            User user = GetUserByUserNameOrEmail(info);
            CheckPassword(user, info);
            Session session = sessionRepository.Add(info);
            session.Role = user.Role;
            return session;
        }

        private void CheckPassword(User user, LogInInfo info)
        {
            if (user.Password != info.Password)
                throw new ArgumentException("Password is incorrect");
        }

        private User GetUserByUserNameOrEmail(LogInInfo info)
        {
            string username = info.Username;
            string email = info.Email;

            return IsEmpty(username) ?
                userRepository.GetByEmail(email) :
                userRepository.GetByUsername(username);
        }

        private void VerifyLoginInfoIsNotEmpty(LogInInfo info)
        {
            string username = info.Username;
            string email = info.Email;

            if (IsEmpty(username) && IsEmpty(email))
                throw new ArgumentException("Username or Email is required");
        }

        private bool IsEmpty(string info)
        {
            return string.IsNullOrEmpty(info);
        }

        public void LogOut(Guid token)
        {
             sessionRepository.DeleteByToken(token);
        }
    }
}
