using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechnicoRMP.Shared.Dtos;

public class LoginDto : IDto
{
    public required string Email { get; set; }
    public required string Password { get; set; }
}
