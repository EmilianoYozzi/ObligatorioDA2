using BlogDomain;

namespace BlogApplication.Models.Out
{
    public class OutModelNotification
    {

        public string Id { get; set; }
        public string Message { get; set; }
        public string Uri { get; set; }
        public DateTime Date { get; private set; }

        public OutModelNotification(Notification notification) {
            this.Id = notification.Id;
            this.Message = notification.Message;
            this.Uri = notification.Uri;
            this.Date = notification.Date;
        }

        public override bool Equals(object? obj) => Equals(obj as OutModelNotification);
        
        public bool Equals(OutModelNotification model) => 
            this.Id.Equals(model.Id) &&
            this.Message.Equals(model.Message) &&
            this.Uri.Equals(model.Uri) &&
            this.Date.Equals(model.Date);
    }
}