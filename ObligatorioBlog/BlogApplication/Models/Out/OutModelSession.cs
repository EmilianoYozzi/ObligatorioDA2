using BlogDomain;
using BlogDomain.DomainEnums;

namespace BlogApplication.Models.Out
{
    public class OutModelSession
    {
        public string Token { set; get; }
        public string Role { set; get; }

        public OutModelSession(Session session) {
            this.Token = (session.Token).ToString();
            this.Role = Enum.GetName(typeof(UserRole), session.Role);
        }

        public override bool Equals(object? obj) => Equals(obj as OutModelSession);

        public bool Equals(OutModelSession model) =>
            this.Token == model.Token && 
            this.Role == model.Role;
    }
}