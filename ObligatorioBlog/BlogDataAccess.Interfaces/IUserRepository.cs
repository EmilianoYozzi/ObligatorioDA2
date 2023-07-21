using BlogDomain;

namespace BlogDataAccess.Interfaces
{
    public interface IUserRepository
    {
        User Add(User user);
        void Delete(Func<User,bool> func);
        User Get(Func<User, bool> func);
        User[] GetAllUsers();
        User GetByEmail(string email);
        User GetByUsername(string username);
        User Update(User user);
        bool Exists(Func<User, bool> func);
        bool Exists(string username);
        void DeleteByUsername(string username);
        User[] GetMultiple(Func<User, bool> func);
    }
}
