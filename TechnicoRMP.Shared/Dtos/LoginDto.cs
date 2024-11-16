namespace TechnicoRMP.Shared.Dtos;

public class LoginDto : IDto
{
    public required string Email { get; set; }
    public required string Password { get; set; }

}
