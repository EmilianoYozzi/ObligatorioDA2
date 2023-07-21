using BlogDomain.DomainEnums;

namespace BlogDomain
{
    public class User
    {
        public string Username { set; get; }
        public string Password { set; get; }
        public string Name { set; get; }
        public string Lastname { set; get; }
        public string Email { set; get; }
        public UserRole Role { set; get; }
        public List<Article> Articles { set; get; }
        public bool Deleted { get; set; }
        public List<Notification> Notifications { set; get; }

        public User()
        {
            this.Role = UserRole.Blogger;
            this.Articles = new List<Article>();
            this.Deleted = false;
            this.Notifications = new List<Notification>();
        }

        public bool HasRole(UserRole role)
        {
            return Role.Equals(role);
        }

        public override bool Equals(object? obj) => Equals(obj as User);
        
        private bool Equals(User? user) => Username == user?.Username;
        
        public void AddNotification(Notification notification) =>
            this.Notifications.Add(notification);
        
        public void Update(User user)
        {
            Password = user.Password;
            Name = user.Name;
            Lastname = user.Lastname;
            Email = user.Email;
        }
    }
}