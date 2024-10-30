using Azure;
using TechnicoRMP.Models;

namespace TechnicoRMP.Services;

public interface IPropertyRepairService
{
    void Display(long id);
    Response Update(PropertyRepair propertyRepair);
    bool Delete(long id);
    Response<PropertyRepair> Create();
}
