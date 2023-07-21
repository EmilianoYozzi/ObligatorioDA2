using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogDomain
{
    public class UserScore
    {
        public int Points { get; set; }
        public string Username { get; set; }

        public UserScore() 
        {
            
        }

        public override bool Equals(object? obj)
            => Equals(obj as UserScore);
        
        private bool Equals(UserScore other)
            => Username.Equals(other.Username) &&
            Points.Equals(other.Points);
    }
}
