using BlogDomain;

namespace BlogApplication.Models.Out
{
    public class OutModelUser
    {
        public string Username { set; get; }
        public string Name { set; get; }
        public string Lastname { set; get; }

        public OutModelUser(User user) {
            this.Username = user.Username;
            this.Name = user.Name;
            this.Lastname = user.Lastname;
        }

        public override bool Equals(object? obj) => Equals(obj as OutModelUser);
        
        public bool Equals(OutModelUser model) => 
            this.Username == model.Username && 
            this.Name == model.Name &&    
            this.Lastname == model.Lastname;
    }
}