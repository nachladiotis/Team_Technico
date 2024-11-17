   using TechnicoRMP.Models;

namespace TechnicoRMP.Shared.Dtos;

public class UserDto : IDto
{
    public long Id { get; set; }
    public required string Name { get; set; }
    public required string VatNumber { get; set; }
    public required string Surname { get; set; }
    public string? Address { get; set; }
    public string? PhoneNumber { get; set; }
    public required string Email { get; set; }

    //public string? Password { get; set; }
    public EnRoleType TypeOfUser { get; set; } = EnRoleType.User;
    public bool IsActive { get; set; } 

   
}

/// <summary>
/// 
/// </summary>
/// <param name=""></param>
/// <param name="SesionExpirationInDays">The days should a cookie expire</param>
public sealed record UserLoginResponse(UserDto UserDto,int SesionExpirationInDays) : IDto;