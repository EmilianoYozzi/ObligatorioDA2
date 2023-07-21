using BlogDataAccess.Interfaces;
using BlogDomain;
using BlogDomain.DomainInterfaces;
using BlogServicesInterfaces;
using Exceptions;

namespace BlogServices
{
    public class CommentService : ICommentService
    {
        private readonly ICommentRepository commentRepo;
        private readonly IArticleService articleService;
        private readonly IUserService userService;
        private IWordControl wordControl;

        public CommentService(ICommentRepository commentData, IArticleService articleService, IUserService userService, IWordControl control)
        {
            this.commentRepo = commentData;
            this.articleService = articleService;
            this.userService = userService;
            this.wordControl = control;
        }

        public Comment CreateComment(Comment comment, string contentId)
        {
            Verify(comment, contentId);
            comment.IdAttachedTo = contentId;
            commentRepo.Add(comment);
            AddCommentToContent(comment);
            NotifyOwner(comment);
            return comment;
        }

        public void Verify(Comment comment, string id)
        {
            VerifyComment(comment);
            VerifyContent(id);
        }

        private void VerifyComment(Comment comment)
        {
            if (string.IsNullOrEmpty(comment.Text))
                throw new ArgumentException("Text is needed.");
            if (string.IsNullOrEmpty(comment.OwnerUsername))
                throw new ArgumentException("Owner is needed.");
            if (!userService.Exists(comment.OwnerUsername))
                throw new ArgumentException("Comment owner does not exist.");
        }

        private void VerifyContent(string id)
        {
            if (string.IsNullOrEmpty(id))
                throw new ArgumentException("Content to comment unespecified.");

            bool isComment = commentRepo.Exists(id);
            bool isArticle = articleService.Exists(id);

            if (!(isArticle || isComment))
                throw new ArgumentException("Content to comment does not exist.");

            if (isComment)
                VerifyCommentIsNotAnswered(id);
        }

        private void VerifyCommentIsNotAnswered(string id)
        {
            Comment comment = commentRepo.GetById(id);

            if (comment.Answer != null)
                throw new InvalidOperationException("This comment can't be replied because already has a reply.");
        }

        private void AddCommentToContent(Comment comment)
        {
            IContent content = GetContent(comment.IdAttachedTo);
            content.AddComment(comment);
            if (content as Comment != null)
                commentRepo.Update(content as Comment);
            else
                articleService.UpdateArticle(content as Article);
        }

        private IContent GetContent(string contentId)
        {
            if (articleService.Exists(contentId))
                return articleService.GetArticleById(contentId);
            else if (commentRepo.Exists(contentId))
                return commentRepo.GetById(contentId);
            throw new ResourceNotFoundException();
        }

        private void NotifyOwner(Comment comment)
        {
            IContent article = GetContent(comment.IdAttachedTo);
            Notification notification = new Notification()
            {
                Message = comment.OwnerUsername + " commented your article.",
                Uri = "comments/" + comment.Id
            };
            userService.NotifyUser(notification, article.GetOwnerUsername());            
        }

        public Comment GetCommentById(string commentId)
        {
            return commentRepo.GetById(commentId);
        }

        public Comment UpdateComment(Comment comment, string contentId)
        {
            Verify(comment, contentId);
            return commentRepo.Update(comment);
        }

        public void DeleteCommentById(string contentId)
        {
            commentRepo.Delete(contentId);
        }

        public void VerifyWords(Comment comment)
        {
            List<string> offensiveWords = wordControl.CheckOffensiveWords(comment.Text);

            if (offensiveWords.Count > 0)
            {
                comment.WaitingForRevision = true;
                string message = "Offensive words detected: " +
                    string.Join(", ", offensiveWords) + ".";

                Notification notification = new Notification()
                {
                    Message = message,
                    Uri = "comments/" + comment.Id,
                };
                userService.NotifyAdmins(notification);
                userService.NotifyUser(notification, comment.OwnerUsername);
            }
        }
    }
}
