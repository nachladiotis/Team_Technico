using TechnicoRMP.Models;

namespace TechnicoRMP.Dtos;

public class CreateUserResponseService
{

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