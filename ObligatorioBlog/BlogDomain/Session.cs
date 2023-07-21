using BlogDomain.DomainEnums;

namespace BlogDomain
{
    public class Session
    {
        public string Username { set; get; }
        public Guid Token { set; get; }
        public bool Deleted { set; get; }
        public UserRole Role { set; get; }

        public Session() { 
            Deleted = false;
            Token = Guid.NewGuid();
        }
    }
}