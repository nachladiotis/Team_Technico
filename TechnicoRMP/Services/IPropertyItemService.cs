using TechnicoRMP.Common;
using TechnicoRMP.Dtos;
using TechnicoRMP.Models;

namespace TechnicoRMP.Services;

public interface IPropertyItemService
{
    Result Update(UpdatePropertyItemRequest updatePropertyItemRequest);
    bool Delete(string e9Number);
    Result<CreatePropertyItemResponse> Create(CreatePropertyItemRequest createPropertyItemRequest);
}
