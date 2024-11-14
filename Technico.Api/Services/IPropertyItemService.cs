using TechnicoRMP.Models;
using TechnicoRMP.Shared.Common;
using TechnicoRMP.Shared.Dtos;

namespace Technico.Api.Services;

public interface IPropertyItemService
{
    Result<CreatePropertyItemResponse> Create(CreatePropertyItemRequest createPropertyItemRequest);
    List<PropertyItem> ReadPropertyItems();
    Result Update(UpdatePropertyItemRequest updatePropertyItemRequest);
    bool Delete(int id);
    public Task<Result<CreatePropertyItemResponse>> GetById(int id);
}
