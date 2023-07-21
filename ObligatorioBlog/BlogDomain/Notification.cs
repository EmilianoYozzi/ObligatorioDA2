
namespace BlogDomain
{
    public class Notification
    {
        public string Id { get; set; }
        public string Message { get; set; }
        public string Uri { get; set; }
        public DateTime Date { get; private set; }

        public Notification()
        {
            Id = Guid.NewGuid().ToString();
            Message = "";
            Uri = "";
            Date = DateTime.Now;
        }

        public override bool Equals(object? obj) => Equals(obj as Notification);

        public bool Equals(Notification other) =>
            this.Id.Equals(other.Id);       
    }    
}
