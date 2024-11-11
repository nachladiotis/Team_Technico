using TechnicoRMP.Models;

namespace TechnicoRMP.Shared.Dtos;

public class CreateUserRequest : IDto
{
    public required string Name { get; set; }
    public required string VatNumber { get; set; }
    public required string Surname { get; set; }
    public string? Address { get; set; }
    public string? PhoneNumber { get; set; }
    public required string Email { get; set; }
    public required string Password { get; set; }
    public EnUserType TypeOfUser { get; set; } = EnUserType.Customer;
}
