using Azure;
using TechnicoRMP.Models;

namespace TechnicoRMP.Services;

public interface IPropertyItemService
{
    void Display(string e9Number);
    Response Update(PropertyItem propertyItem);
    bool Delete(string e9Number);
    Response<PropertyItem> Create();
}
