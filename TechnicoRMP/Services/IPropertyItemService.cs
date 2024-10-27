using TechnicoRMP.Models;

namespace TechnicoRMP.Services;

public interface IPropertyItemService
{
    void Display(string e9Number);
    void Update(PropertyItem propertyItem);
    bool Delete(string e9Number);
    PropertyItem Create();
}
