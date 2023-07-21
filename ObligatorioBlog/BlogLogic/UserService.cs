using BlogDataAccess.Interfaces;
using BlogDomain;
using BlogDomain.DomainEnums;
using BlogServicesInterfaces;
using System.Text.RegularExpressions;

namespace BlogServices
{
    public class UserService : IUserService
    {
        private IUserRepository data;

        public UserService(IUserRepository data) 
        { 
            this.data = data;
        }

        public User AddUser(User user)
        {
            ValidateFields(user);
            return data.Add(user);
        }

        private void ValidateFields(User user) {
            CheckForEmptyFields(user);
            ValidateUsername(user.Username);
            ValidateEmail(user.Email);
        }

        private void CheckForEmptyFields(User user)
        {
            if (string.IsNullOrEmpty(user.Username))
                throw new ArgumentException("Username is needed.");
            if (string.IsNullOrEmpty(user.Name))
                throw new ArgumentException("Name is needed.");
            if (string.IsNullOrEmpty(user.Lastname))
                throw new ArgumentException("Lastname is needed.");
            if (string.IsNullOrEmpty(user.Email))
                throw new ArgumentException("Email is needed.");
            if (string.IsNullOrEmpty(user.Password))
                throw new ArgumentException("Password is needed.");
        }

        private void ValidateUsername(string username)
        {
            string pattern = @"^[a-zA-Z0-9]{1,12}$";
            if (!Regex.IsMatch(username, pattern))
                throw new FormatException("Invalid username.");
        }

        private void ValidateEmail(string email)
        {
            string pattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
            if (!Regex.IsMatch(email, pattern))
                throw new FormatException("Invalid email.");
        }

        public User[] GetAllUsers()
        {
            return data.GetAllUsers().Where(u => !u.Deleted).ToArray();
        }

        public User GetUserByUsername(string username)
        {
            return data.GetByUsername(username);
        }

        public bool IsAdmin(string username)
        {
            User user = data.GetByUsername(username);
            return user.HasRole(UserRole.Admin);
        }

        public User UpdateUser(User user)
        {
            ValidateFields(user);
            return data.Update(user);
        }

        public void DeleteUser(string username)
        {
            data.DeleteByUsername(username);
        }

        public Notification[] GetUserNotifications(string username)
        {
            User user = data.GetByUsername(username);
            return user.Notifications.ToArray();
        }

        public void NotifyUser(Notification notification, string username)
        {
            User user = GetUserByUsername(username);
            user.AddNotification(notification);
            UpdateUser(user);
        }

        public void NotifyAdmins(Notification notification)
        {
            User[] admins = data.GetMultiple(u => u.HasRole(UserRole.Admin));
            foreach (User user in admins)
                NotifyUser(notification, user.Username);
        }

        public bool Exists(string username)
            => data.Exists(username);

        public Article[] GetAllArticlesFromUser(string username)
        {
            User user = data.GetByUsername(username);
            return user.Articles.ToArray();
        }

        public Article[] GetVisibleArticlesFromUser(string username)
        {
            Article[] allArticles = GetAllArticlesFromUser(username);
            Article[] visibleArticles = allArticles.Where(a => !a.IsHidden()).ToArray();
            return visibleArticles;
        }
    }
}