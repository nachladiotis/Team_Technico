using TechnicoRMP.Models;

namespace TechnicoRMP.Shared.Dtos;

public class CreatePropertyItemResponse : IDto
{
    public long Id { get; set; }
    public required string E9Number { get; set; }
    public required string Address { get; set; }
    public required int YearOfConstruction { get; set; }
    public EnPropertyType EnPropertyType { get; set; }
    public bool IsActive { get; set; } = true;
    

}
