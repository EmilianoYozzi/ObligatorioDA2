namespace BlogImporterDomain;

public class Parameter
{
    public ParameterType ParameterType { get; set; }
    public string Name { get; set; }
    public object Value { get; set; }
    public bool Necessary { get; set; }

    public override bool Equals(object? obj)
        => Equals(obj as Parameter);
    
    private bool Equals(Parameter other) =>
        Name.Equals(other.Name) && 
        Necessary.Equals(other.Necessary) &&
        ParameterType.Equals(other.ParameterType);
}