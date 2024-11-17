using TechnicoRMP.Shared.Dtos;

namespace Technico.Api.Validations
{
    public interface IPropertyItemValidation
    {
        bool PropertyItemValidator(CreatePropertyItemRequest createPropertyItemRequest);
    }
}
