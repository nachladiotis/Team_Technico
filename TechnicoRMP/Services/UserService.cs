using Microsoft.EntityFrameworkCore;
using TechnicoRMP.DataAccess;
using TechnicoRMP.Models;
using TechnicoRMP.Responses;

namespace TechnicoRMP.Servicesp;

public class UserService(DataStore dataStore) : IUserService
{
    private readonly DataStore _dataStore = dataStore;

    public void Display(string vatNumber)
    {
        var user = _dataStore
            .Users
            .Include(p => p.RepairsHistory)
            .Include(p => p.PropertyOwnerships)
            .ThenInclude(s => s.PropertyItem)
            .FirstOrDefault(p => p.VatNumber == vatNumber);
        if (user is null)
        {
            return;
        }
        DisplayUserDetails(user);
        DisplayUserPropertyItemsDetails(user);
        DisplayUserRepairHistoryDetails(user);
    }

    public Response Update(User user)
    {
        var response = new Response()
        {
            Status = 0,
            Message = "ΕΠΙΤΥΧΕΣ"
        };
        if (user is null)
        {
            response.Status = -1;
            response.Message = "ΔΕΝ ΒΡΕΘΗΚΕ ΧΡΗΣΤΗΣ";
            return response;
        }
        if (user.VatNumber is null)
        {
            response.Status = -1;
            response.Message = "ΤΟ ΠΕΔΙΟ ΑΦΜ ΕΙΝΑΙ ΥΠΟΧΡΕΩΤΙΚΟ";
            return response;
        }
       var storedUser = _dataStore.Users.FirstOrDefault(s => s.VatNumber == user.VatNumber);
        if (storedUser is null)
        {
            response.Status = -1;
            response.Message = "Ο ΧΡΗΣΤΗΣ ΔΕΝ ΒΡΕΘΗΚΕ";
            return response;
        }
        _dataStore.Users.Update(user);
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

    private static void DisplayUserRepairHistoryDetails(User user)
    {
        foreach (var repairHistory in user.RepairsHistory)
        {
            Console.WriteLine("ΛΕΠΤΟΜΕΡΙΕΣ ΕΠΙΣΚΕΥΗΣ:");
            Console.WriteLine($"ΔΙΕΥΘΥΝΣΗ: {repairHistory.Address}");
            Console.WriteLine($"ΚΟΣΤΟΣ: {repairHistory.Cost:C}");
            var isAcrtiveText = repairHistory.IsActive is true ? "ΕΝΕΡΓΟ" : "ΑΝΕΝΕΡΓΟ";
            Console.WriteLine($"ΚΑΤΑΣΤΑΣΗ:{isAcrtiveText}");
        }
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

    public  Response<User> Create()
    {
        Console.WriteLine("ΕΙΣΑΓΕΤΕ ΤΑ ΣΤΟΙΧΕΙΑ ΤΟΥ ΧΡΗΣΤΗ:");

        var failResponse = new Response<User>
        {
            Status = -1
        };

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
        

        
        var user = new User
        {
            Name = name,
            Surname = surname,
            VatNumber = vatNumber,
            Address = address,
            PhoneNumber = phoneNumber,
            Email = email,
            Password = password,
            TypeOfUser = userType
        };

        _dataStore.Add(user);
        _dataStore.SaveChanges();
        return new Response<User>
        {
            Status = 0,
            Message = "ΕΠΙΤΥΧΊΑ ΔΗΜΗΟΥΡΓΙΑΣ ΧΡΗΣΤΗ",
            Value = user
        }; ;
    }

}
