using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechnicoRMP.Models;

namespace TechnicoRMP.Dtos;

public class CreateUserResponse :IDto
{
    public long Id { get; set; }
    public required string Name { get; set; }
    public required string VatNumber { get; set; }
    public required string Surname { get; set; }
    public string? Address { get; set; }
    public string? PhoneNumber { get; set; }
    public required string Email { get; set; }
    public EnUserType TypeOfUser { get; set; } = EnUserType.Customer;

    public static CreateUserResponse CreateFromEntity(User user)
    {
        return new CreateUserResponse
        {
            Id = user.Id,
            Name = user.Name,
            VatNumber = user.VatNumber,
            Surname = user.Surname,
            Email = user.Email,
            TypeOfUser = user.TypeOfUser,
            Address = user.Address,
            PhoneNumber = user.PhoneNumber,
        };
    }
        
}
