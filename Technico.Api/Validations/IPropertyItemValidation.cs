using TechnicoRMP.Models;

namespace Technico.Api.Validations
{
    public interface IPropertyItemValidation
    {
        bool PropertyItemValidator(PropertyItem propertyItem);
    }
}
