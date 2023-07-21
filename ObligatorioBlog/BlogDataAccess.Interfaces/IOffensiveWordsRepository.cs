using BlogDomain;

namespace BlogDataAccess.Interfaces
{
    public interface IOffensiveWordsRepository
    {
        OffensiveWordCollection Add(OffensiveWordCollection offensiveWords);
        void Delete(OffensiveWordCollection offensiveWords);
        OffensiveWordCollection Get();
    }
}
