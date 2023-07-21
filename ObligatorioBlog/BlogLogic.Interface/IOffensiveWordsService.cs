using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogServices.Interfaces
{
    public interface IOffensiveWordsService
    {
        string[] AddOffensiveWords(string[] offensiveWords);
        void DeleteOffensiveWords(string[] offensiveWord);
        string[] GetOffensiveWords();
    }
}
