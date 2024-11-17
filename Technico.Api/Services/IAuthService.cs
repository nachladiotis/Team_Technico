using TechnicoRMP.Shared.Common;
using TechnicoRMP.Shared.Dtos;

namespace Technico.Api.Services;

public interface IAuthService
{
    Task<Result> RegisterAsync(CreateUserRequest createUserRequest);
    Task<Result<UserLoginResponse>> LoginAsync(LoginDto dto);

} 