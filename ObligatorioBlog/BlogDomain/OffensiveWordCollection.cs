using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogDomain
{
    public class OffensiveWordCollection
    {
        public int Id { get; private set; } = 1;
        public List<string> offensiveWords { get; set; }
        public OffensiveWordCollection()
        {
            offensiveWords = new List<string>();
        }
        public OffensiveWordCollection(string[] offensiveWords)
        {
            this.offensiveWords = offensiveWords.ToList();
        }
        public override bool Equals(object? obj)
        {
            return Equals(obj as OffensiveWordCollection);
        }
        private bool Equals(OffensiveWordCollection other)
        {
            return this.offensiveWords.SequenceEqual(other.offensiveWords);
        }

    }
}
