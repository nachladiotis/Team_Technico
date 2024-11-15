using TechnicoRMP.Shared.Common;
using TechnicoRMP.Shared.Dtos;

namespace Technico.Api.Services;

public interface IAuthService
{
    Task<Result<UserDto>> RegisterAsync(CreateUserRequest createUserRequest);
    Task<Result<UserDto>> LoginAsync(LoginDto dto);

} 