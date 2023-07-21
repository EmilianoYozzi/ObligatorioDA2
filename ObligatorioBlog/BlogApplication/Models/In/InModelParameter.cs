using BlogDomain.DomainEnums;
using BlogImporterDomain;

namespace BlogApplication.Models.In
{
    public class InModelParameter
    {
        public string ParameterType { get; set; }
        public string Name { get; set; }
        public object Value { get; set; }

        public Parameter ToEntity()
        {
            Enum.TryParse(ParameterType, out ParameterType pType);
            return new Parameter()
            {
                ParameterType = pType,
                Name = Name,
                Value = Value
            };
        }
    }
}