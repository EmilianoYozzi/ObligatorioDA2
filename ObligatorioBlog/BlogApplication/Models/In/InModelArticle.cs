using BlogDomain;
using BlogDomain.DomainEnums;

namespace BlogApplication.Models.In
{
    public class InModelArticle
    {
        public string Title { set; get; }
        public string Text { set; get; }
        public string Visibility { set; get; }
        public List<string> Images { set; get; }
        public string Template { set; get; }
        public string OwnerUsername { set; get; }

        public Article ToEntity()
        {
            Enum.TryParse(Visibility, out Visibility vis);
            Enum.TryParse(Template, out ArticleTemplate template);

            return new Article()
            {
                Title = this.Title,
                Text = this.Text,
                Images = this.Images.Select(i => new Image() { Content = i}).ToList(),
                Visibility = vis,
                Template = template,
                OwnerUsername = this.OwnerUsername,
            };
        }
    }
}