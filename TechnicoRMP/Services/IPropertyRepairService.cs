using TechnicoRMP.Common;
using TechnicoRMP.Dtos;
using TechnicoRMP.Models;

namespace TechnicoRMP.Services;

public interface IPropertyRepairService
{
    Result Update(UpdatePropertyRepair updatePropertyRepair);
    bool Delete(long id);
    Result<CreatePropertyRepairResponse> Create(CreatePropertyRepairRequest createPropertyRepairRequest);
}
