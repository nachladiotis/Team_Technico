using TechnicoRMP.Models;
using TechnicoRMP.Shared.Common;
using TechnicoRMP.Shared.Dtos;

namespace Technico.Api.Services;

public interface IAuthService
{
    Task<Result<CreateUserResponse>> RegisterAsync(CreateUserRequest createUserRequest);
    Task<Result<CreateUserResponse>> LoginAsync(LoginDto dto);

} 