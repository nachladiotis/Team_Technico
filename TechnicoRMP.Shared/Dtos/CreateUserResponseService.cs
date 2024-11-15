

using TechnicoRMP.Models;

namespace TechnicoRMP.Shared.Dtos;

public class CreateUserResponseService
{

    public static UserDto CreateFromEntity(User user)
    {
        return new UserDto
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