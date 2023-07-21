namespace BlogImporterDomain
{
    public class ImportedImage
    {
        public string Content { get; set; }        

        public ImportedImage()
        {
            Content = "";
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as ImportedImage);
        }

        private bool Equals(ImportedImage other)
            => Content.Equals(other.Content);
    }
}