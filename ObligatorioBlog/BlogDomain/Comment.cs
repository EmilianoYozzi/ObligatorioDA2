using BlogDomain.DomainInterfaces;

namespace BlogDomain
{
    public class Comment : IContent
    {
        public string? Id { get; set; }
        public string? OwnerUsername { get; set; }
        public string? Text { get; set; }
        public string? IdAttachedTo { get; set; }
        public Comment? Answer { get; set; }
        public bool? Edited { get; set; }
        public bool? Deleted { get; set; }
        public bool WaitingForRevision
        {
            set
            {
                waitingForRevision = value;
                if (value)
                    hadOffensiveWords = true;
            }
            get => waitingForRevision;
        }
        private bool waitingForRevision;
        private bool hadOffensiveWords;

        public Comment()
        {
            Id = Guid.NewGuid().ToString();
            waitingForRevision = false;
            hadOffensiveWords = false;
            Edited = false;
            Deleted = false;
        }

        public void AddComment(Comment comment)
        {
            Answer = comment;
            Answer.IdAttachedTo = Id;
        }

        public override bool Equals(object? obj) => Equals(obj as Comment);        

        private bool Equals(Comment other) =>
            this.Id.Equals(other.Id);

        public string GetOwnerUsername()
            => OwnerUsername;

        public bool IsHidden()
            => WaitingForRevision;

        public bool HadOffensiveWords()
            => hadOffensiveWords;
    }
}