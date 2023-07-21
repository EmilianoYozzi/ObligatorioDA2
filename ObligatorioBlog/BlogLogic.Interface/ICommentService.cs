using BlogDomain;

namespace BlogServicesInterfaces
{
    public interface ICommentService
    {
        Comment CreateComment(Comment comment, string contentId);
        void DeleteCommentById(string contentId);
        Comment GetCommentById(string commentId);
        Comment UpdateComment(Comment comment, string contentId);
        void VerifyWords(Comment comment);
    }
}
