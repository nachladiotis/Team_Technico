using TechnicoRMP.Models;
using TechnicoRMP.Shared.Dtos;

namespace TechnicoRMP.WebApp;

public static class ActiveUser
{
    public static UserDto? User { get; private set; }
    public static EnRoleType? UserRole { get => User?.TypeOfUser; }
    public static void SetUser(UserDto? user)
    {
        User = user;
    }
}

