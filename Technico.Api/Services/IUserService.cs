using TechnicoRMP.Models;
using TechnicoRMP.Shared.Common;
using TechnicoRMP.Shared.Dtos;

namespace Technico.Api.Services;

public interface IUserService
{
    Result Update(UpdateUserRequest updateUserRequest);
    bool Delete(string vatNumber);
    Result<CreateUserResponse> Create(CreatUserRequest creatUserDto);
}