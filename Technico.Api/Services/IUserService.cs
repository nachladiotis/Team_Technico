using TechnicoRMP.Models;
using TechnicoRMP.Shared.Common;
using TechnicoRMP.Shared.Dtos;

namespace Technico.Api.Services;

public interface IUserService
{
    bool Delete(int id);
    Result<CreateUserResponse> Create(CreatUserRequest creatUserDto);
    List<CreateUserResponse> DisplayAll();
    CreateUserResponse DisplayUser(int id);
    CreateUserResponse ReplaceUser(CreateUserResponse user);
    UpdateUserRequest UpdateUser(UpdateUserRequest updateUserRequest);
    bool SoftDeleteUser(int id);
}