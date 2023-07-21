using BlogDomain;

namespace BlogApplication.Models.In
{
    public class LogInModel
    {
        public string Username { set; get; }
        public string Password { set; get; }
        public string Email { set; get; }

        public LogInInfo ToEntity()
        {
            return new LogInInfo()
            {
                Username = this.Username,
                Password = this.Password,
                Email = this.Email
            };
        }
    }
}