using BlogDomain;

namespace BlogDataAccess.Interfaces
{
    public interface ICommentRepository
    {
        Comment Add(Comment comment);
        bool Exists(Func<Comment, bool> func);
        Comment Get(Func<Comment, bool> func);
        Comment GetById(string id);
        bool Exists(string id);
        Comment Update(Comment comment);
        void Delete(string id);
    }
}
