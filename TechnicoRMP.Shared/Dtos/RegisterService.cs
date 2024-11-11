using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechnicoRMP.Models;

namespace TechnicoRMP.Shared.Dtos;

public class RegisterService:IDto
{
    public static User CreateRegisterUserFromDto(CreateUserRequest createUserRequest)
    {
        return new User
        {
            Name = createUserRequest.Name,
            VatNumber = createUserRequest.VatNumber,
            Surname = createUserRequest.Surname,
            Email = createUserRequest.Email,
            TypeOfUser = createUserRequest.TypeOfUser,
            Address = createUserRequest.Address,
            PhoneNumber = createUserRequest.PhoneNumber, 
            Password = createUserRequest.Password,
        };
    }
}
