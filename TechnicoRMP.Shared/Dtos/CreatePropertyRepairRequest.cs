using TechnicoRMP.Models;

namespace TechnicoRMP.Shared.Dtos;

public class CreatePropertyRepairRequest : IDto
{
    public EnTypeOfRepair TypeOfRepair { get; set; }
    public required string Address { get; set; }
    public double Cost { get; set; }
    public long RepairerId { get; set; }
}
