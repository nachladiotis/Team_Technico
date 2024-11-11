using Microsoft.AspNetCore.Mvc;
using Technico.Api.Services;
using TechnicoRMP.Database.DataAccess;
using TechnicoRMP.Shared.Common;
using TechnicoRMP.Shared.Dtos;

namespace Technico.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]


    public class RegistrateController : ControllerBase
    {
        private readonly DataStore _datastore;
   
        private IAuthService _authService;
        public RegistrateController(IAuthService authService)
        {
            _authService = authService;
        }



        [HttpPost("register")]
        public async Task<Result> Register(CreateUserRequest createUserRequest)
        {
           
            var response = await _authService.RegisterAsync(createUserRequest);

            if (response.Status < 0)
            {
                return new Result
                {
                    Status = response.Status,
                    Message = response.Message,
              };
            }

            return new Result
            {
                Status = response.Status,
                Message = response.Message
            };
        }

        [HttpPost]
        [Route("login")]
        public async Task<Result<CreateUserResponse>> Login(LoginDto loginDto)
        {
            var response = await _authService.LoginAsync(loginDto);

            if (response.Status < 0)
            {
                return new Result<CreateUserResponse>
                {
                    Status = response.Status,
                    Message = response.Message,
                    Value = response.Value
                };
            }

            return new Result<CreateUserResponse>
            {
                Status = response.Status,
                Message = response.Message,
                Value = response.Value
            };
        }

    }
}
