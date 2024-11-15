using TechnicoRMP.Shared.Common;
using TechnicoRMP.Shared.Dtos;

namespace Technico.Api.Services;

public interface IUserService
{
    Task<bool> Delete(int id);
    Result<UserDto> Create(CreateUserRequest creatUserDto);
    Task<List<UserDto>> DisplayAll();
    Task<UserDto> DisplayUser(int id);
    Task<UserDto> ReplaceUser(UpdateUserRequest dto);
    Task<UpdateUserRequest> UpdateUser(UpdateUserRequest updateUserRequest);
    Task<bool> SoftDeleteUser(int id);
}