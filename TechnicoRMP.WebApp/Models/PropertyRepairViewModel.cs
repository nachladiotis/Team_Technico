using TechnicoRMP.Models;

namespace TechnicoRMP.WebApp.Models;
public class PropertyRepairViewModel
{
    public long Id { get; set; }
    public DateTime Date { get; set; }
    public  EnTypeOfRepair TypeOfRepair { get; set; }
    public string Address { get; set; }
    public EnRepairStatus RepairStatus { get; set; }
    public double Cost { get; set; }
    public long UserId { get; set; }
    public bool IsActive { get; set; }
}
