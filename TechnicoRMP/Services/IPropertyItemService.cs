using TechnicoRMP.Common;
using TechnicoRMP.Models;

namespace TechnicoRMP.Services;

public interface IPropertyItemService
{
    Result Update(PropertyItem propertyItem);
    bool Delete(string e9Number);
    Result<PropertyItem> Create();
}
