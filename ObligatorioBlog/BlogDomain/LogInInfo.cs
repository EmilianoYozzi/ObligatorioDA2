namespace BlogDomain
{
    public class LogInInfo
    {
        public string Username { set; get; }
        public string Password { set; get; }
        public string Email { set; get; }

        public LogInInfo()
        {
            Username = "";
            Password = "";
            Email = "";
        }
    }
}