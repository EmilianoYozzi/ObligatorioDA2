namespace BlogImporterDomain
{
    public class ImportedArticle
    {
        public string OwnerUsername { get; set; }
        public string Title { set; get; }
        public string Text { get; set; }
        public List<ImportedImage> Images { set; get; }
        public ImportedArticleTemplate Template { set; get; }
        public ImportedVisibility Visibility { set; get; }

        public ImportedArticle()
        {
            Images = new List<ImportedImage>();
            Visibility = ImportedVisibility.Public;
            Template = ImportedArticleTemplate.ImageAtTopLeft;
        }

        public override bool Equals(object? obj) => Equals(obj as ImportedArticle);

        private bool Equals(ImportedArticle other) =>
            this.Title.Equals(other.Title) &&
            this.Text.Equals(other.Text) &&
            this.Images.SequenceEqual(other.Images) &&
            this.OwnerUsername.Equals(other.OwnerUsername);
    }

    public enum ImportedArticleTemplate
    {
        NoImage,
        ImageAtTop,
        ImageAtBottom,
        ImageAtTopLeft,
        ImageAtTopAndBottom
    }

    public enum ImportedVisibility
    {
        Public,
        Private
    }
}