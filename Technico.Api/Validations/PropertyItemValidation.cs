using TechnicoRMP.Models;
using TechnicoRMP.Shared.Dtos;

namespace Technico.Api.Validations
{
    public class PropertyItemValidation: IPropertyItemValidation
    {
        public bool PropertyItemValidator(CreatePropertyItemRequest createPropertyItemRequest)
        {
            if (createPropertyItemRequest == null)
                return false;
            if (createPropertyItemRequest.E9Number == null)
                return false;
            return true;
        }
    }
}
