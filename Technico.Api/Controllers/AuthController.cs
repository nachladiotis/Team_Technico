using Microsoft.AspNetCore.Mvc;
using Technico.Api.Services;
using TechnicoRMP.Database.DataAccess;
using TechnicoRMP.Shared.Common;
using TechnicoRMP.Shared.Dtos;

namespace Technico.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController(IAuthService authService) : ControllerBase
    {
        private IAuthService _authService = authService;

        [HttpPost("register")]
        public async Task<IActionResult> Register(CreateUserRequest createUserRequest)
        {
            var response = await _authService.RegisterAsync(createUserRequest);

            return Ok(new Result
            {
                Status = response.Status,
                Message = response.Message
            });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto loginDto)
        {
            var response = await _authService.LoginAsync(loginDto);

            return Ok(new Result<CreateUserResponse>
            {
                Status = response.Status,
                Message = response.Message,
                Value = response.Value
            });
        }

    }
}
