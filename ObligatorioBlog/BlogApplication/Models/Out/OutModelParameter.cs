using BlogDomain.DomainEnums;
using BlogImporterDomain;

namespace BlogApplication.Models.Out;

public class OutModelParameter
{
    public string ParameterType { get; set; }
    public string Name { get; set; }
    public bool Necessary { get; set; }
    
    public OutModelParameter(Parameter parameter)
    {
        ParameterType = Enum.GetName(typeof(ParameterType), parameter.ParameterType);
        Name = parameter.Name;
        Necessary = parameter.Necessary;
    }

    public OutModelParameter()
    {
    }

    public override bool Equals(object? obj)
    => Equals(obj as OutModelParameter);


    public bool Equals(OutModelParameter other) =>
        ParameterType.Equals(other.ParameterType) &&
        Name.Equals(other.Name) &&
        Necessary.Equals(other.Necessary);
}