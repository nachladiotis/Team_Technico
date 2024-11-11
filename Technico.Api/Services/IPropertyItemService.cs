using TechnicoRMP.Models;
using TechnicoRMP.Shared.Common;
using TechnicoRMP.Shared.Dtos;

namespace Technico.Api.Services;

public interface IPropertyItemService
{
    Result Update(UpdatePropertyItemRequest updatePropertyItemRequest);
    bool Delete(string e9Number);
    Result<CreatePropertyItemResponse> Create(CreatePropertyItemRequest createPropertyItemRequest);
   
    List<PropertyItem> ReadPropertyItems();
}
