using TechnicoRMP.Models;

namespace TechnicoRMP.Shared.Dtos;

public class UpdateUserRequest : IDto
{
    public long Id { get; set; }
    public string? Name { get; set; }
    public string? VatNumber { get; set; }
    public string? Surname { get; set; }
    public string? Address { get; set; }
    public string? PhoneNumber { get; set; }
    public string? Email { get; set; }
    public string? Password { get; set; }
    public EnUserType? TypeOfUser { get; set; }
}
