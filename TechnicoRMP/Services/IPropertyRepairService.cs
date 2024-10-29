using TechnicoRMP.Models;

namespace TechnicoRMP.Services;

public interface IPropertyRepairService
{
    void Display(long id);
    void Update(PropertyRepair propertyRepair);
    bool Delete(long id);
    PropertyRepair Create();
}
