namespace BlogDomain.DomainInterfaces
{
    public interface IContent
    {
        void AddComment(Comment comment);
        string GetOwnerUsername();
        bool IsHidden();
        bool HadOffensiveWords();
    }
}
