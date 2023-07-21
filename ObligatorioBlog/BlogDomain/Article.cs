using BlogDomain.DomainEnums;
using BlogDomain.DomainInterfaces;

namespace BlogDomain
{
    public class Article : IContent
    {
        public string Id { get; set; }
        public string OwnerUsername { get; set; }
        public string Title { set; get; }
        public string Text { get; set; }
        public ArticleTemplate Template { set; get; }
        public Visibility Visibility { set; get; }
        public List<Comment> Comments { set; get; }
        public bool Deleted { set; get; }
        public bool Edited { private set; get; }
        public DateLog DateLog { get; }
        private List<Image> images;
        public List<Image> Images {
            set => SetAndBindImages(value);            
            get => images;
        }
        public bool WaitingForRevision { 
            set { 
                waitingForRevision = value; 
                if (value)
                    hadOffensiveWords = true;
            } 
            get => waitingForRevision; 
        }
        private bool waitingForRevision;
        private bool hadOffensiveWords;

        public Article()
        {
            Id = Guid.NewGuid().ToString(); 
            Comments = new List<Comment>();
            Visibility = Visibility.Public;
            Template = ArticleTemplate.ImageAtTopLeft;
            DateLog = new DateLog();
            Deleted = false;
            Edited = false;
            WaitingForRevision = false;
            hadOffensiveWords = false;
        }

        public void AddComment(Comment comment)
        {
             this.Comments.Add(comment);
        }

        public void Update(Article article)
        {
            this.Title = article.Title;
            this.Text = article.Text;
            this.Images = article.Images;
            this.Template = article.Template;    
            this.Visibility = article.Visibility;
            this.Edited = true;
        }

        public override bool Equals(object? obj) => Equals(obj as Article);

        private bool Equals(Article other) => 
            this.Id.Equals(other.Id) &&
            this.Title.Equals(other.Title) &&
            this.Text.Equals(other.Text) && 
            this.OwnerUsername.Equals(other.OwnerUsername);

        public string GetOwnerUsername()
            => this.OwnerUsername;

        public bool IsHidden()
            => Visibility.Equals(Visibility.Private) || WaitingForRevision;

        public bool HadOffensiveWords()
            => hadOffensiveWords;

        private void SetAndBindImages(List<Image> images)
        {
            foreach (Image img in images)
                img.IdAttachedTo = Id;
            this.images = images;
        }
    }
}