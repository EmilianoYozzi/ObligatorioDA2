using BlogDomain;

namespace BlogApplication.Models.In
{
    public class InModelUser
    {
        public string Username { set; get; }
        public string Password { set; get; }
        public string Name { set; get; }
        public string Lastname { set; get; }
        public string Email { set; get; }

        public User ToEntity()
        {
            return new User()
            {
                Username = this.Username,
                Password = this.Password,
                Name = this.Name,
                Lastname = this.Lastname,
                Email = this.Email
            };
        }
    }
}