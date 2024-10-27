using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace TechnicoRMP.Models;

public class PropertyItem
{
    public long Id { get; set; }
    public required string E9Number { get; set; }
    public required string Address { get; set; }
    public required int YearOfConstruction { get; set; }
    public EnPropertyType EnPropertyType { get; set; }
    public List<PropertyOwnership> PropertyOwnerships { get; set; } = [];
    public bool IsActive { get; set; } = true;


}
