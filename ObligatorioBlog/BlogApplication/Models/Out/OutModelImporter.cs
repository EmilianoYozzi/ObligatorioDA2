using Importers.Interfaces;

namespace BlogApplication.Models.Out;

public class OutModelImporter
{
    public string Name { get; set; }
    public List<OutModelParameter> Parameters { get; set; }
    
    public OutModelImporter(IArticleImporter importer)
    {
        Name = importer.GetName();
        Parameters = importer.GetRequiredParameters()
            .Select(p => new OutModelParameter(p)).ToList();
    }

    public OutModelImporter()
    {
    }

    public override bool Equals(object? obj)
    => Equals(obj as OutModelImporter);
    

    public bool Equals(OutModelImporter other) =>
        Name.Equals(other.Name) &&
        Parameters.SequenceEqual(other.Parameters);
}