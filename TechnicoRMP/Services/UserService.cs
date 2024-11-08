using Microsoft.EntityFrameworkCore;
using TechnicoRMP.Common;
using TechnicoRMP.DataAccess;
using TechnicoRMP.Dtos;
using TechnicoRMP.Models;

namespace TechnicoRMP.Services;

public class UserService(DataStore dataStore) : IUserService
{
    private readonly DataStore _dataStore = dataStore;
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

    public List<UserWithPropertyItemsDTO> GetAllUsersWithPropertyItems()
    {
        return _libContext.Users
            .Include(a => a.Books)
            .Select(a => a.ConvertUserWithPropertyItems())
            .ToListAsync();
    }

    public async Task<AuthorDTO> ReplaceAuthor(AuthorDTO dto)
    {
        if (dto.FirstName == null || dto.LastName == null)
            throw new BadRequestException("Bad Request: The author first and last name must be specified!");

        var author = await _libContext.Authors.FindAsync(dto.Id);

        if (author == null)
            throw new NotFoundException("Not Found: The author with the given id was not found!");

        author.FirstName = dto.FirstName;
        author.LastName = dto.LastName;
        await _libContext.SaveChangesAsync();

        return author.ConvertAuthor();
    }
}
