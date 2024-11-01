using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechnicoRMP.Models;

namespace TechnicoRMP.Dtos;

public class CreatePropertyRepairRequest : IDto
{
    public EnTypeOfRepair TypeOfRepair { get; set; }
    public required string Address { get; set; }
    public decimal Cost { get; set; }
    public long RepairerId { get; set; }
}
