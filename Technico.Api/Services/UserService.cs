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
    private readonly ILogger<UserService> _logger;

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

        // Update fields if they have values in updateUserRequest
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

        // Uncomment if TypeOfUser should be updated only if present in the request
        // if (updateUserRequest.TypeOfUser.HasValue)
        //     storedUser.TypeOfUser = updateUserRequest.TypeOfUser.Value;

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


    public Result<CreateUserResponse> Create(CreateUserRequest creatUserDto)
    {
        var failResponse = new Result<CreateUserResponse>
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
                Password = creatUserDto.Password,
                TypeOfUser = creatUserDto.TypeOfUser
            };
            if (user == null)
            {
                throw new NotFoundException("User cannot be null.");
            }

            user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);
            _dataStore.Add(user);
            _dataStore.SaveChanges();
            return new Result<CreateUserResponse>
            {
                Status = 0,
                Message = "ΕΠΙΤΥΧΊΑ ΔΗΜΗΟΥΡΓΙΑΣ ΧΡΗΣΤΗ",
                Value = CreateUserResponseService.CreateFromEntity(user)
            };
        }
        catch (Exception)
        {
            return failResponse;
        }

    }


    public async Task<CreateUserResponse> DisplayUser(int id)
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

    //DisplayUserDetails(user);
    //DisplayUserPropertyItemsDetails(user);
    }

    //private static void DisplayUserPropertyItemsDetails(User user)
    //{
    //    foreach (var ownership in user.PropertyOwnerships)
    //    {
    //        var propertyItem = ownership.PropertyItem;
    //        if (propertyItem is null)
    //            continue;
    //        Console.WriteLine("ΠΛΗΡΟΦΟΡΙΕΣ ΑΝΤΙΚΕΙΜΕΝΟΥ:");
    //        Console.WriteLine($"E9: {propertyItem.E9Number}");
    //        Console.WriteLine($"ΔΙΕΥΘΥΝΣΗ: {propertyItem.Address}");
    //        Console.WriteLine($"ΕΤΟΣ ΚΑΤΑΣΚΕΥΗΣ: {propertyItem.YearOfConstruction}");
    //    }
    //}

    //private static void DisplayUserDetails(User user)
    //{
    //    Console.WriteLine("ΠΛΗΡΟΦΟΡΙΕΣ ΧΡΗΣΤΗ:");
    //    Console.WriteLine($"ΟΝΟΜΑ: {user.Name}");
    //    Console.WriteLine($"ΕΠΙΘΕΤΟ: {user.Surname}");
    //    Console.WriteLine($"ΑΦΜ: {user.VatNumber}");
    //    Console.WriteLine($"Email: {user.Email}");

    //    if (!string.IsNullOrEmpty(user.Address))
    //        Console.WriteLine($"ΔΙΕΥΘΥΝΣΗ: {user.Address}");
    //    if (!string.IsNullOrEmpty(user.PhoneNumber))
    //        Console.WriteLine($"ΤΗΛΕΦΩΝΟ: {user.PhoneNumber}");
    //}

    public async Task<List<CreateUserResponse>> DisplayAll()
    {
        try
        {
            var users = await _dataStore
                       .Users
                       .Include(p => p.PropertyOwnerships)
                       .ThenInclude(s => s.PropertyItem)
                       .ToListAsync();

            return users.Select(user => new CreateUserResponse
            {
                Id = (int)user.Id,
                Name = user.Name,
                Surname = user.Surname,
                VatNumber = user.VatNumber,
                Email = user.Email,
                Address = user.Address,
                PhoneNumber = user.PhoneNumber,
            }).ToList();
        }
        catch(Exception ex)
        {

        }

        return new List<CreateUserResponse> ();
       
    }



    public async Task<CreateUserResponse> ReplaceUser(UpdateUserRequest dto)
    {
        var user = await _dataStore.Users.FindAsync(dto.Id);

        if (user == null)
            throw new NotFoundException("Not Found: The user with the given id was not found!");

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
        var user = await _dataStore.Users.FirstOrDefaultAsync(u => u.Id == id);
        if (user == null)
        {
            throw new NotFoundException($"User with Id {id} not found.");
        }

        user.IsActive = false;
        await _dataStore.SaveChangesAsync();

        return true;
    }

}
