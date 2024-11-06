using TechnicoRMP.Common;
using TechnicoRMP.Dtos;
using TechnicoRMP.Models;

namespace TechnicoRMP.Services;

public interface IUserService
{
    Result Update(UpdateUserRequest updateUserRequest);
    bool Delete(string vatNumber);
    Result<CreateUserResponse> Create(CreatUserRequest creatUserDto);
}