using Microsoft.EntityFrameworkCore;
using TechnicoRMP.Common;
using TechnicoRMP.DataAccess;
using TechnicoRMP.Dtos;
using TechnicoRMP.Models;

namespace TechnicoRMP.Servicesp;

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

        storedUser.Address = updateUserRequest.Address;
        storedUser.Password = updateUserRequest.Password;
        storedUser.PhoneNumber = updateUserRequest.PhoneNumber;
        storedUser.VatNumber = updateUserRequest.VatNumber;
        storedUser.Email = updateUserRequest.Email;
        storedUser.Surname = updateUserRequest.Surname;
        storedUser.TypeOfUser = updateUserRequest.TypeOfUser;

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

   
    public  Result<CreateUserResponse> Create(CreatUserRequest creatUserDto)
    {
        Console.WriteLine("ΕΙΣΑΓΕΤΕ ΤΑ ΣΤΟΙΧΕΙΑ ΤΟΥ ΧΡΗΣΤΗ:");

        var failResponse = new Result<User>
        {
            Status = -1
        };

        /*
        Console.Write("ΟΝΟΜΑ: ");
        string name = Console.ReadLine() ?? string.Empty;
        if (string.IsNullOrEmpty(name))
        {
            failResponse.Message ="ΤΟ ΟΝΟΜΑ ΕΙΝΑΙ ΥΠΟΧΡΕΩΤΙΚΟ";          
            return failResponse;
        }

        Console.Write("ΕΠΩΝΥΜΟ: ");
        string surname = Console.ReadLine() ?? string.Empty;
        if (string.IsNullOrEmpty(surname))
        {
            failResponse.Message = "ΤΟ ΕΠΙΘΕΤΟ ΕΙΝΑΙ ΥΠΟΧΡΕΩΤΙΚΟ";
            return failResponse;
        }

        Console.Write("Α.Φ.Μ.: ");
        string vatNumber = Console.ReadLine() ?? string.Empty;
        if (string.IsNullOrEmpty(vatNumber))
        {
            failResponse.Message = "ΤΟ ΑΦΜ ΕΙΝΑΙ ΥΠΟΧΡΕΩΤΙΚΟ";
            return failResponse;
        }

        Console.Write("ΔΙΕΥΘΥΝΣΗ (ΠΡΟΑΙΡΕΤΙΚΟ): ");
        string? address = Console.ReadLine();

        Console.Write("ΑΡΙΘΜΟΣ ΤΗΛΕΦΩΝΟΥ (ΠΡΟΑΙΡΕΤΙΚΟ): ");
        string? phoneNumber = Console.ReadLine();

        Console.Write("EMAIL: ");
        string email = Console.ReadLine() ?? string.Empty;
        if (string.IsNullOrEmpty(email))
        {
            failResponse.Message = "ΤΟ EMAIL ΕΙΝΑΙ ΥΠΟΧΡΕΩΤΙΚΟ";
            return failResponse;
        }

        Console.Write("ΚΩΔΙΚΟΣ: ");
        string password = Console.ReadLine() ?? string.Empty;
        if (string.IsNullOrEmpty(password) || password.Length < 8 )
        {
            failResponse.Message = "Ο ΚΩΔΙΚΟΣ ΕΙΝΑΙ ΥΠΟΧΡΕΩΤΙΚΟΣ ΚΑΙ ΠΡΕΠΕΙ ΝΑ ΕΙΝΑΙ ΤΟΥΛΑΧΙΣΤΟΝ 8 ΧΑΡΑΚΤΗΡΕΣ";
            return failResponse;
        }

        Console.Write("ΤΥΠΟΣ ΧΡΗΣΤΗ (1 ΓΙΑ ΠΕΛΑΤΗΣ, 2 ΓΙΑ ΕΠΙΣΚΕΥΑΣΤΗΣ): ");
        var typeofUser = Console.ReadLine();
        if (typeofUser is not "2" || typeofUser is not "1")
        {
            failResponse.Message = "ΠΡΕΠΕΙ ΝΑ ΔΩΣΕΙΣ 1 Η 2 ΥΠΟΧΡΕΩΤΙΚΑ";
            return failResponse;
        }
        EnUserType userType = (Console.ReadLine() == "2") ? EnUserType.Provider : EnUserType.Customer;
        
        */
        
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
            Value = CreateUserResponse.CreateFromEntity(user)
        }; ;
    }

}
