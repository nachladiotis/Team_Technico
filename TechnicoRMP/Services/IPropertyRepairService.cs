using TechnicoRMP.Common;
using TechnicoRMP.Models;

namespace TechnicoRMP.Services;

public interface IPropertyRepairService
{
    Result Update(PropertyRepair propertyRepair);
    bool Delete(long id);
    Result<PropertyRepair> Create();
}
