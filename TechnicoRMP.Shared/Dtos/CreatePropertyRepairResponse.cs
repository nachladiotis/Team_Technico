using TechnicoRMP.Models;

namespace TechnicoRMP.Shared.Dtos;

public class CreatePropertyRepairResponse : IDto
{
    public long Id { get; set; }
    public DateTime? Date { get; set; }
    public EnTypeOfRepair TypeOfRepair { get; set; }
    public required string Address { get; set; }
    public EnRepairStatus RepairStatus { get; set; }
    public double Cost { get; set; }
    public long RepairerId { get; set; }
    public bool IsActive { get; set; }
}
