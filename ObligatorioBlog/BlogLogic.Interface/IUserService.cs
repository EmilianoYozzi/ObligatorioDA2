using BlogDomain;

namespace BlogServicesInterfaces
{
    public interface IUserService
    {
        User AddUser(User user);
        User[] GetAllUsers();
        User GetUserByUsername(string username);
        bool IsAdmin(string loggedUser);
        User UpdateUser(User user);
        void DeleteUser(string username);
        Notification[] GetUserNotifications(string username);
        void NotifyAdmins(Notification notification);
        void NotifyUser(Notification notification, string username);
        bool Exists(string ownerUsername);
        Article[] GetAllArticlesFromUser(string username);
        Article[] GetVisibleArticlesFromUser(string username);
    } 
}