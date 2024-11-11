using TechnicoRMP.Shared.Common;
using TechnicoRMP.Shared.Dtos;

namespace Technico.Api.Services;

public interface IUserService
{
    Task<bool> Delete(int id);
    Result<CreateUserResponse> Create(CreatUserRequest creatUserDto);
    Task<List<CreateUserResponse>> DisplayAll();
    Task<CreateUserResponse> DisplayUser(int id);
    Task<CreateUserResponse> ReplaceUser(UpdateUserRequest dto);
    Task<UpdateUserRequest> UpdateUser(UpdateUserRequest updateUserRequest);
    Task<bool> SoftDeleteUser(int id);
}