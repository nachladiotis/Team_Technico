using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechnicoRMP.Models;

namespace TechnicoRMP.Dtos;

public class UpdateUserRequest:IDto
{
    public long Id { get; set; }
    public required string Name { get; set; }
    public required string VatNumber { get; set; }
    public required string Surname { get; set; }
    public string? Address { get; set; }
    public string? PhoneNumber { get; set; }
    public required string Email { get; set; }
    public required string Password { get; set; }
    public EnUserType TypeOfUser { get; set; } = EnUserType.Customer;
}
