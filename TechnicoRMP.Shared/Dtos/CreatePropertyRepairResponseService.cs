using TechnicoRMP.Models;

namespace TechnicoRMP.Shared.Dtos;

public class CreatePropertyRepairResponseService
{
    public static CreatePropertyRepairResponse CreateFromEntity(PropertyRepair propertyRepair)
    {
        return new CreatePropertyRepairResponse
        {
            RepairStatus = propertyRepair.RepairStatus,
            Address = propertyRepair.Address,
            Cost = propertyRepair.Cost,
            Date = propertyRepair.Date,
            Id = propertyRepair.Id,
            IsActive = propertyRepair.IsActive,
            RepairerId = propertyRepair.RepairerId,
            TypeOfRepair = propertyRepair.TypeOfRepair,
        };
    }
}
