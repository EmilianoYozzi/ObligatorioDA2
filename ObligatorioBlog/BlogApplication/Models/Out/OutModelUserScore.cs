using BlogDomain;

namespace BlogApplication.Models.Out
{
    public class OutModelUserScore
    {
        public string Username { set; get; }
        public int Points { set; get; }

        public OutModelUserScore(UserScore score)
        {
            this.Username = score.Username;
            this.Points = score.Points;
        }

        public override bool Equals(object? obj) => Equals(obj as OutModelUserScore);

        public bool Equals(OutModelUserScore model) =>
            this.Username == model.Username &&
            this.Points == model.Points;
    }
}
