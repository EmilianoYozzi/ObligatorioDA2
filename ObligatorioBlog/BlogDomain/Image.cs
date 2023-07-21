
namespace BlogDomain
{
    public class Image
    {
        public string IdAttachedTo { get; set; }
        public Guid Id { get; set; }
        public string Content { get; set; }

        public Image()
        {
            Id = Guid.NewGuid();
            Content = "";
        }
    }
}