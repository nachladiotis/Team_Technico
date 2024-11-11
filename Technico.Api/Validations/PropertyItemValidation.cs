using TechnicoRMP.Models;

namespace Technico.Api.Validations
{
    public class PropertyItemValidation: IPropertyItemValidation
    {
        public bool PropertyItemValidator(PropertyItem propertyItem)
        {
            if (propertyItem == null)
                return false;
            if (propertyItem.YearOfConstruction == null || propertyItem.EnPropertyType == null)
                return false;
            return true;
        }
    }
}
