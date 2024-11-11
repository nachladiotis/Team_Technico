using TechnicoRMP.Models;

namespace TechnicoRMP.Shared.Dtos;

public class UpdatePropertyRepair : IDto
{
    public long Id { get; set; }
    public EnTypeOfRepair? TypeOfRepair { get; set; }
    public string? Address { get; set; }
    public EnRepairStatus? RepairStatus { get; set; }
    public decimal? Cost { get; set; }
    public long? RepairerId { get; set; }
    public bool? IsActive { get; set; } = true;
}
