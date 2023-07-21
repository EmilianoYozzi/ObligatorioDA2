using BlogDataAccess.Interfaces;
using BlogDomain;
using BlogServices.Interfaces;

namespace BlogServices
{
    public class OffensiveWordsService : IOffensiveWordsService
    {
        private IOffensiveWordsRepository data;
        public OffensiveWordsService(IOffensiveWordsRepository data)
        {
            this.data = data;
        }

        public string[] AddOffensiveWords(string[] offensiveWords)
        {
            OffensiveWordCollection words = new OffensiveWordCollection(offensiveWords);
            VerifyOffensiveWords(offensiveWords);
            return data.Add(words).offensiveWords.ToArray();
        }

        public void DeleteOffensiveWords(string[] offensiveWords)
        {
            OffensiveWordCollection words = new OffensiveWordCollection(offensiveWords);
            data.Delete(words);
        }

        public string[] GetOffensiveWords()
        {
            return data.Get().offensiveWords.ToArray();
        }
        public void VerifyOffensiveWords(string[] offensiveWords)
        {
            if (offensiveWords == null || offensiveWords.Length == 0)
            {
                throw new ArgumentException("Offensive word is needed.");
            }
            for (int i = 0; i < offensiveWords.Length; i++)
            {
                if (offensiveWords[i] == null || offensiveWords[i] == "")
                {
                    throw new ArgumentException("Offensive word is needed.");
                }
                if (offensiveWords[i].Split(' ').Length > 1)
                {
                    throw new FormatException("Offensive word cannot contain spaces.");
                }
            }
        }
    }

}
