using TechnicoRMP.Models;

namespace TechnicoRMP.Shared.Dtos;

public class CreatePropertyRepairResponseService
{
    public static PropertyRepairResponseDTO CreateFromEntity(PropertyRepair propertyRepair)
    {
        return new PropertyRepairResponseDTO
        {
            RepairStatus = propertyRepair.RepairStatus,
            Address = propertyRepair.Address,
            Cost = propertyRepair.Cost,
            Date = propertyRepair.Date,
            Id = propertyRepair.Id,
            IsActive = propertyRepair.IsActive,
            UserId = propertyRepair.UserId,
            TypeOfRepair = propertyRepair.TypeOfRepair,
            Description = propertyRepair.Description,
        };
    }
}
