namespace TechnicoRMP.Models;

public class PropertyRepair
{
    public long Id { get; set; }
    public DateTime Date { get; set; }
    public EnTypeOfRepair TypeOfRepair { get; set; }
    public required string Address { get; set; }
    public EnRepairStatus RepairStatus { get; set; } = EnRepairStatus.Pending;
    public double Cost { get; set; }
    public User? User { get; set; }
    public long UserId { get; set; } 
    public bool IsActive { get; set; } = true;
}
