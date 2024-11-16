using Microsoft.EntityFrameworkCore;
using TechnicoRMP.Database.DataAccess;
using TechnicoRMP.Shared.Common;
using TechnicoRMP.Shared.Dtos;
using BCrypt.Net;

namespace Technico.Api.Services
{
    public class AuthService(DataStore datastore) : IAuthService
    {
        private readonly DataStore _datastore = datastore;

        private const int LogginUSerSesionInDays = 7;

        public async Task<Result<UserDto>> RegisterAsync(CreateUserRequest createUserRequest)
        {
            var user = _datastore.Users.FirstOrDefault(u => u.Email == createUserRequest.Email);
            if (user != null)
            {
                return new Result<UserDto>
                {
                    Status = 0,
                    Message = "Already exist user with this Email"
                };
            }

            HashPassword(createUserRequest);

            var newUser = RegisterService.CreateRegisterUserFromDto(createUserRequest);

            _datastore.Users.Add(newUser);
            await _datastore.SaveChangesAsync();

            return new Result<UserDto>
            {
                Status = 1,
                Message = "User success registered",
                Value = CreateUserResponseService.CreateFromEntity(newUser)
            };
        }

        private static void HashPassword(CreateUserRequest createUserRequest)
        {
            var hasedPassword = BCrypt.Net.BCrypt.HashPassword(createUserRequest.Password);
            createUserRequest.Password = hasedPassword;
        }

        public async Task<Result<UserLoginResponse>> LoginAsync(LoginDto loginDto)
        {
            var user = await _datastore.Users
                .FirstOrDefaultAsync(u => u.Email == loginDto.Email);

            var errorMesage = "Invalid username or password. Please try again.";

            if (user == null)
            {
                return new Result<UserLoginResponse>
                {
                    Status = 0,
                    Message = errorMesage,
                };
            }

            bool passwordMatch = BCrypt.Net.BCrypt.Verify(loginDto.Password, user.Password);

            if (!passwordMatch)
            {
                return new Result<UserLoginResponse>
                {
                    Status = 0,
                    Message = errorMesage,
                };
            }

            var logginResponse = new UserLoginResponse(CreateUserResponseService.CreateFromEntity(user), LogginUSerSesionInDays);
          
            return new Result<UserLoginResponse>
            {
                Status = 1,
                Message = "User loggin is success",
                Value = logginResponse
            };   
        }
    }
}
