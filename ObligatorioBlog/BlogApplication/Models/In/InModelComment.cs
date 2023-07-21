using BlogDomain;

namespace BlogApplication.Models.In
{
    public class InModelComment
    {
        public string OwnerUsername { get; set; }

        public string Text { get; set; }

        public string contentId { get; set; }

        public Comment ToEntity()
        {
            return new Comment()
            { 
                OwnerUsername = OwnerUsername,
                Text = Text
            };
        }
    }
}
