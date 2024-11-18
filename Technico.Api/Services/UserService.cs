using Microsoft.EntityFrameworkCore;
using TechnicoRMP.Models;
using TechnicoRMP.Shared.Common;
using TechnicoRMP.Shared.Exceptions;
using TechnicoRMP.Shared.Dtos;
using Technico.Api.Validations;
using TechnicoRMP.Database.DataAccess;

namespace Technico.Api.Services;

public class UserService : IUserService
{
    private readonly DataStore _dataStore;
    private static List<User> Users = new List<User>();
    private readonly ILogger<IUserService> _logger;

    public UserService(DataStore context)
    {
        _dataStore = context;
    }

    public async Task<UpdateUserRequest> UpdateUser(UpdateUserRequest updateUserRequest)
    {
        var storedUser = await _dataStore.Users.FirstOrDefaultAsync(s => s.Id == updateUserRequest.Id);
        if (storedUser is null)
        {
            throw new NotFoundException($"User with Id {updateUserRequest.Id} not found.");
        }

        if (!string.IsNullOrEmpty(updateUserRequest.Address))
            storedUser.Address = updateUserRequest.Address;

        if (!string.IsNullOrEmpty(updateUserRequest.Password))
            storedUser.Password = BCrypt.Net.BCrypt.HashPassword(updateUserRequest.Password);

        if (!string.IsNullOrEmpty(updateUserRequest.PhoneNumber))
            storedUser.PhoneNumber = updateUserRequest.PhoneNumber;

        if (!string.IsNullOrEmpty(updateUserRequest.VatNumber) &&
            await _dataStore.Users.AnyAsync(u => u.VatNumber != storedUser.VatNumber))
        {
            storedUser.VatNumber = updateUserRequest.VatNumber;
        }

        if (await _dataStore.Users.AnyAsync(u => u.VatNumber == updateUserRequest.VatNumber))
        {
            throw new UserException("User already exists with the same VAT.");
        }

        if (!string.IsNullOrEmpty(updateUserRequest.Email))
            storedUser.Email = updateUserRequest.Email;

        if (!string.IsNullOrEmpty(updateUserRequest.Surname))
            storedUser.Surname = updateUserRequest.Surname;

        if (!string.IsNullOrEmpty(updateUserRequest.Name))
            storedUser.Name = updateUserRequest.Name;

        bool isUpdateValid = UserValidators.ValidateUserForUpdate(storedUser);
        if (!isUpdateValid)
        {
            throw new BadRequestException("User does not have right email, VAT, or password");
        }

        _dataStore.Users.Update(storedUser);
        await _dataStore.SaveChangesAsync();

        return new UpdateUserRequest
        {
            Id = storedUser.Id,
            Name = storedUser.Name,
            Surname = storedUser.Surname,
            Address = storedUser.Address,
            PhoneNumber = storedUser.PhoneNumber,
            Email = storedUser.Email,
            Password = storedUser.Password,
            VatNumber = storedUser.VatNumber,
            TypeOfUser = storedUser.TypeOfUser
        };
    }



    public async Task<bool> Delete(int id)
    {
        var user = await _dataStore.Users.FirstOrDefaultAsync(s => s.Id == id);

        if (user is null)
        {
            return false;
        }

        _dataStore.Users.Remove(user);

        var deleted = await _dataStore.SaveChangesAsync();
        return deleted > 0;
    }


    public async Task< Result<UserDto>> Create(CreateUserRequest creatUserDto)
    {
        var failResponse = new Result<UserDto>
        {
            Status = -1
        };
        try
        {
            var user = new User
            {
                Name = creatUserDto.Name,
                Surname = creatUserDto.Surname,
                VatNumber = creatUserDto.VatNumber,
                Address = creatUserDto.Address,
                PhoneNumber = creatUserDto.PhoneNumber,
                Email = creatUserDto.Email,
                Password = creatUserDto.Password
            } ?? throw new NotFoundException("User cannot be null.");

            user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);
            _dataStore.Add(user);
            _dataStore.SaveChanges();
            return new Result<UserDto>
            {
                Value = CreateUserResponseService.CreateFromEntity(user)
            };
        }
        catch (Exception)
        {
            return failResponse;
        }

    }


    public async Task<UserDto> DisplayUser(int id)
    {
        var user = await _dataStore
            .Users
            .Include(p => p.PropertyOwnerships)
            .ThenInclude(s => s.PropertyItem)
            .FirstOrDefaultAsync(p => p.Id == id);

        if (user == null)
        {
            throw new NotFoundException($"User with Id {id} not found.");
        }

        var response = CreateUserResponseService.CreateFromEntity(user);
        return response;

   
    }


    public async Task<List<UserDto>> DisplayAll()
    {
        try
        {
            var users = await _dataStore
                       .Users
                       .Include(p => p.PropertyOwnerships)
                       .ThenInclude(s => s.PropertyItem)
                       .ToListAsync();

            return users.Select(user => new UserDto
            {
                Id = (int)user.Id,
                Name = user.Name,
                Surname = user.Surname,
                VatNumber = user.VatNumber,
                Email = user.Email,
                Address = user.Address,
                PhoneNumber = user.PhoneNumber,
                IsActive = user.IsActive 
            }).ToList();
        }
        catch(Exception ex)
        {

        }

        return [];
       
    }

    public async Task<UserDto> ReplaceUser(UpdateUserRequest dto)
    {
        var user = await _dataStore.Users.FindAsync(dto.Id)
            ?? throw new NotFoundException("Not Found: The user with the given id was not found!");
        
        if (!string.IsNullOrEmpty(dto.Name) && dto.Name != "string")
            user.Name = dto.Name;

        if (!string.IsNullOrEmpty(dto.Surname) && dto.Surname != "string")
            user.Surname = dto.Surname;

        if (!string.IsNullOrEmpty(dto.VatNumber) && dto.VatNumber != "string")
        {
            if (await _dataStore.Users.AnyAsync(u => u.VatNumber == dto.VatNumber && u.Id != user.Id))
            {
                throw new BadRequestException("User already exists with the same VAT.");
            }
            user.VatNumber = dto.VatNumber;
        }

        if (await _dataStore.Users.AnyAsync(u => u.VatNumber == dto.VatNumber && u.Id != user.Id))
        {
            throw new UserException("User already exists with the same VAT.");
        }

        if (!string.IsNullOrEmpty(dto.Email) && dto.Email != "string")
            user.Email = dto.Email;

        if (!string.IsNullOrEmpty(dto.Address) && dto.Address != "string")
            user.Address = dto.Address;

        if (!string.IsNullOrEmpty(dto.PhoneNumber) && dto.PhoneNumber != "string")
            user.PhoneNumber = dto.PhoneNumber;

        if (!string.IsNullOrEmpty(dto.Password) && dto.Password != "string")
        {
            user.Password = BCrypt.Net.BCrypt.HashPassword(dto.Password);
        }

        bool isValid = UserValidators.ValidateUserForReplace(user);
        if (!isValid)
        {
            throw new BadRequestException("Invalid user data.");
        }

        await _dataStore.SaveChangesAsync();

        return CreateUserResponseService.CreateFromEntity(user);
    }


    public async Task<bool> SoftDeleteUser(int id)
    {
        var user = await _dataStore.Users.FirstOrDefaultAsync(u => u.Id == id)
            ?? throw new NotFoundException($"User with Id {id} not found.");

        user.IsActive = false;
        await _dataStore.SaveChangesAsync();

        return true;
    }

}
