using System.Text.RegularExpressions;
using BlogDataAccess.Interfaces;
using BlogServicesInterfaces;

namespace BlogServices
{
    public class WordControl : IWordControl
    {
        private IOffensiveWordsRepository repository;
        public WordControl(IOffensiveWordsRepository repository) {
            this.repository = repository;
        }

        public List<string> CheckOffensiveWords(string text)
        {
            List<string> wordsFound = new List<string>();
            List<string> offensiveWords = repository.Get().offensiveWords;
            string filteredText = RemoveNonAlphanumericChars(text).ToLower();

            foreach (string word in offensiveWords)
                if (filteredText.Contains(word.ToLower())) 
                    wordsFound.Add(word);
        
            return wordsFound;
        }

        private string RemoveNonAlphanumericChars(string text)
        {
            string pattern = @"[^a-zA-Z0-9]";
            Regex regex = new Regex(pattern);
            return regex.Replace(text, "");
        }
    }
}
