using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using TechnicoRMP.Database.DataAccess;
using TechnicoRMP.Models;
using TechnicoRMP.Shared.Common;
using TechnicoRMP.Shared.Exceptions;
using TechnicoRMP.Shared.Dtos;

namespace Technico.Api.Services;

public class UserService : IUserService
{
    private readonly DataStore _dataStore;

    public UserService(DataStore context)
    {
        _dataStore = context;
    }

    public Result Update(UpdateUserRequest updateUserRequest)
    {
        var response = new Result()
        {
            Status = 0,
            Message = "ΕΠΙΤΥΧΕΣ"
        };

        var storedUser = _dataStore.Users.FirstOrDefault(s => s.Id == updateUserRequest.Id);
        if (storedUser is null)
        {
            response.Status = -1;
            response.Message = "Ο ΧΡΗΣΤΗΣ ΔΕΝ ΒΡΕΘΗΚΕ";
            return response;
        }
        if (updateUserRequest.Address != null)
        {
            storedUser.Address = updateUserRequest.Address;
        }
        if (updateUserRequest.Password != null)
        {
            storedUser.Password = updateUserRequest.Password;
        }
        if (updateUserRequest.PhoneNumber != null)
        {
            storedUser.PhoneNumber = updateUserRequest.PhoneNumber;
        }
        if (updateUserRequest.VatNumber != null)
        {
            storedUser.VatNumber = updateUserRequest.VatNumber;
        }
        if (updateUserRequest.Email != null)
        {
            storedUser.Email = updateUserRequest.Email;
        }
        if (updateUserRequest.Surname != null)
        {
            storedUser.Surname = updateUserRequest.Surname;
        }
        if (updateUserRequest.TypeOfUser != null)
        {
            storedUser.TypeOfUser = updateUserRequest.TypeOfUser.Value;
        }


        _dataStore.Users.Update(storedUser);
        _dataStore.SaveChanges();
        return response;

    }

    public bool Delete(string vatNumber)
    {

        if (string.IsNullOrEmpty(vatNumber))
        {
            return false;
        }
        var user = _dataStore.Users.FirstOrDefault(s => s.VatNumber == vatNumber);
        if (user is null)
        {
            Console.WriteLine("Ο ΧΡΗΣΤΗΣ ΔΕΝ ΒΡΕΘΗΚΕ");
            return false;
        }
        _dataStore.Users.Remove(user);
        var deleted = _dataStore.SaveChanges();
        return deleted > 0;
    }


    public Result<CreateUserResponse> Create(CreatUserRequest creatUserDto)
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

            _dataStore.Add(user);
            _dataStore.SaveChanges();
            return new Result<CreateUserResponse>
            {
                Status = 0,
                Message = "ΕΠΙΤΥΧΊΑ ΔΗΜΗΟΥΡΓΙΑΣ ΧΡΗΣΤΗ",
                Value = CreateUserResponseService.CreateFromEntity(user)
            };
        }
        catch (Exception ex)
        {
            return failResponse;
        }

    }


    public CreateUserResponse DisplayUser(int id)
    {
        var user = _dataStore
            .Users
            .Include(p => p.PropertyOwnerships)
            .ThenInclude(s => s.PropertyItem)
            .FirstOrDefault(p => p.Id == id);
        if (user == null)
        {
            throw new NotFoundException($"User with Id {id} not found.");
        }

        var response = CreateUserResponseService.CreateFromEntity(user);
        return response;

        //DisplayUserDetails(user);
        //DisplayUserPropertyItemsDetails(user);
    }

    private static void DisplayUserPropertyItemsDetails(User user)
    {
        foreach (var ownership in user.PropertyOwnerships)
        {
            var propertyItem = ownership.PropertyItem;
            if (propertyItem is null)
                continue;
            Console.WriteLine("ΠΛΗΡΟΦΟΡΙΕΣ ΑΝΤΙΚΕΙΜΕΝΟΥ:");
            Console.WriteLine($"E9: {propertyItem.E9Number}");
            Console.WriteLine($"ΔΙΕΥΘΥΝΣΗ: {propertyItem.Address}");
            Console.WriteLine($"ΕΤΟΣ ΚΑΤΑΣΚΕΥΗΣ: {propertyItem.YearOfConstruction}");
        }
    }

    private static void DisplayUserDetails(User user)
    {
        Console.WriteLine("ΠΛΗΡΟΦΟΡΙΕΣ ΧΡΗΣΤΗ:");
        Console.WriteLine($"ΟΝΟΜΑ: {user.Name}");
        Console.WriteLine($"ΕΠΙΘΕΤΟ: {user.Surname}");
        Console.WriteLine($"ΑΦΜ: {user.VatNumber}");
        Console.WriteLine($"Email: {user.Email}");

        if (!string.IsNullOrEmpty(user.Address))
            Console.WriteLine($"ΔΙΕΥΘΥΝΣΗ: {user.Address}");
        if (!string.IsNullOrEmpty(user.PhoneNumber))
            Console.WriteLine($"ΤΗΛΕΦΩΝΟ: {user.PhoneNumber}");
    }

    public List<CreateUserResponse> DisplayAll()
    {
        var users = _dataStore
            .Users
            .Include(p => p.PropertyOwnerships)
            .ThenInclude(s => s.PropertyItem)
            .ToList();

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

    ////public List<UserWithProperyItemsDTO> GetAllUsersWithPropertyItems()
    ////{
    ////    return _dataStore.Users
    ////        .Include(a => a.PropertyItem)
    ////        .Select(a => a.ConvertUserWithPropertyItems())
    ////    .ToList();
    ////}

    //public CreateUserResponse ReplaceUser(CreateUserResponse dto)
    //{
    //    if (dto.Name == null || dto.Surname == null)
    //        throw new BadRequestException("Bad Request: The user name and surname must be specified!");

    //    var user = _dataStore.Users.Find(dto.Id);

    //    if (user == null)
    //        throw new NotFoundException("Not Found: The user with the given id was not found!");

    //    user.Name = dto.Name;
    //    user.Surname = dto.Surname;
    //    _dataStore.SaveChangesAsync();

    //    return user.ConvertUser();
    //}
    //public UpdateUserRequest UpdateUser(UpdateUserRequest dto)
    //{
    //    bool firstNameNull = (dto.Name == null) ? true : false;
    //    bool lastNameNull = (dto.Surname == null) ? true : false;

    //    if (firstNameNull && lastNameNull) throw new BadRequestException
    //            ("Bad Request: Either the user name or surname must be specified");

    //    var user = _dataStore.Users.Find(dto.Id);

    //    if (user == null)
    //        throw new NotFoundException("Not Found: The user with the given id was not found!");

    //    if (!firstNameNull)
    //        user.Name = dto.Name!;

    //    if (!lastNameNull)
    //        user.Surname = dto.Surname!;

    //    _dataStore.SaveChanges();

    //    return dto;
    //}
    //public bool DeleteUser(int id)
    //{
    //    User? a = _dataStore.Users.Find(id);

    //    if (a == null) { return false; }
    //    else
    //    {
    //        _dataStore.Users.Remove(a);
    //        _dataStore.SaveChangesAsync();
    //        return true;
    //    };
    //}
}
