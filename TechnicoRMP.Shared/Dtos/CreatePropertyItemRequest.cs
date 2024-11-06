using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechnicoRMP.Models;

namespace TechnicoRMP.Shared.Dtos;

public class CreatePropertyItemRequest : IDto
{
    public required string E9Number { get; set; }
    public required string Address { get; set; }
    public required int YearOfConstruction { get; set; }
    public EnPropertyType EnPropertyType { get; set; }
    public bool IsActive { get; set; } = true;
}
