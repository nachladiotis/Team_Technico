using TechnicoRMP.Models;
using TechnicoRMP.Shared.Common;
using TechnicoRMP.Shared.Dtos;

namespace Technico.Api.Services;

public interface IPropertyRepairService
{
    Result Update(UpdatePropertyRepair updatePropertyRepair);
    bool Delete(long id);
    Result<CreatePropertyRepairResponse> Create(CreatePropertyRepairRequest createPropertyRepairRequest);
}
