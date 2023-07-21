using BlogDomain;

namespace BlogApplication.Models.Out
{
    public class OutModelComment
    {
        public string Id { get; set; }
        public string Text { get; set; }
        public string Owner { get; set; }
        public OutModelComment Answer { get; set; }
       
        public OutModelComment(Comment comment) {
            this.Id = comment.Id;
            this.Text = comment.Text;
            this.Owner = comment.OwnerUsername;
            this.Answer = comment.Answer == null ? null : new OutModelComment(comment.Answer);
        }

        public override bool Equals(object? obj) => Equals(obj as OutModelComment);

        public bool Equals(OutModelComment model) =>
            this.Id == model.Id &&
            this.Text == model.Text &&
            this.Owner == model.Owner; 
        
    }
}