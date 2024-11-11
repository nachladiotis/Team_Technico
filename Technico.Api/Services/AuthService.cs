using Microsoft.EntityFrameworkCore;
using TechnicoRMP.Database.DataAccess;
using TechnicoRMP.Shared.Common;
using TechnicoRMP.Shared.Dtos;


namespace Technico.Api.Services
{
    public class AuthService : IAuthService
    {
        private readonly DataStore _datastore;


        public AuthService(DataStore datastore)
        {
            _datastore = datastore;
        }

        public async Task<Result<CreateUserResponse>> RegisterAsync(CreateUserRequest createUserRequest)
        {
            var user = _datastore.Users.FirstOrDefault(u => u.Email == createUserRequest.Email);
            if (user != null)
            {
                return new Result<CreateUserResponse>
                {
                    Status = 0,
                    Message = "Already exist user with this Email"
                };
            }

            var newUser = RegisterService.CreateRegisterUserFromDto(createUserRequest);

            _datastore.Users.Add(newUser);
            await _datastore.SaveChangesAsync();

            return new Result<CreateUserResponse>
            {
                Status = 0,
                Message = "User success registered",
                Value = CreateUserResponseService.CreateFromEntity(newUser)
            };
        }

        public async Task<Result<CreateUserResponse>> LoginAsync(LoginDto loginDto)
        {
            var user = await _datastore.Users
                  .FirstOrDefaultAsync(u => u.Email == loginDto.Email && u.Password == loginDto.Password);
            if (user == null)
            {
                return new Result<CreateUserResponse>
                {
                    Status = 0,
                    Message = "Not user found",
                    Value = null
                };
            }

          
            return new Result<CreateUserResponse>
            {
                Status = 1,
                Message = "User loggin is success",
                Value = CreateUserResponseService.CreateFromEntity(user)
            };   
        }
    }
}
