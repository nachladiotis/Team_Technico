using Microsoft.EntityFrameworkCore;
using TechnicoRMP.DataAccess;
using TechnicoRMP.Models;

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
            Console.WriteLine("Ο ΧΡΗΣΤΗΣ ΜΕ ΑΥΤΟ ΤΟ ΑΦΜ ΔΕΝ ΥΠΑΡΧΕΙ");
            return;
        }
        DisplayUserDetails(user);
        DisplayUserPropertyItemsDetails(user);
        DisplayUserRepairHistoryDetails(user);
    }

    public void Update(User user)
    {
        if (user is null)
        {
            return;
        }
        if (user.VatNumber is null)
        {
            Console.WriteLine("ΤΟ ΠΕΔΙΟ ΑΦΜ ΕΙΝΑΙ ΥΠΟΧΡΕΩΤΙΚΟ");
        }
       var storedUser = _dataStore.Users.FirstOrDefault(s => s.VatNumber == user.VatNumber);
        if (storedUser is null)
        {
            Console.WriteLine("Ο ΧΡΗΣΤΗΣ ΔΕΝ ΒΡΕΘΗΚΕ");
            return;
        }
        _dataStore.Users.Update(user);
        _dataStore.SaveChanges();

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

    public  User Create()
    {
        Console.WriteLine("ΕΙΣΑΓΕΤΕ ΤΑ ΣΤΟΙΧΕΙΑ ΤΟΥ ΧΡΗΣΤΗ:");

        Console.Write("ΟΝΟΜΑ: ");
        string name = Console.ReadLine() ?? string.Empty;
        if (string.IsNullOrEmpty(name))
        {
            Console.WriteLine("ΤΟ ΟΝΟΜΑ ΕΙΝΑΙ ΥΠΟΧΡΕΩΤΙΚΟ");
            return null!;
        }

        Console.Write("ΕΠΩΝΥΜΟ: ");
        string surname = Console.ReadLine() ?? string.Empty;
        if (string.IsNullOrEmpty(surname))
        {
            Console.WriteLine("ΤΟ ΕΠΙΘΕΤΟ ΕΙΝΑΙ ΥΠΟΧΡΕΩΤΙΚΟ");
            return null!;
        }

        Console.Write("Α.Φ.Μ.: ");
        string vatNumber = Console.ReadLine() ?? string.Empty;
        if (string.IsNullOrEmpty(vatNumber))
        {
            Console.WriteLine("ΤΟ ΑΦΜ ΕΙΝΑΙ ΥΠΟΧΡΕΩΤΙΚΟ");
            return null!;
        }

        Console.Write("ΔΙΕΥΘΥΝΣΗ (ΠΡΟΑΙΡΕΤΙΚΟ): ");
        string? address = Console.ReadLine();

        Console.Write("ΑΡΙΘΜΟΣ ΤΗΛΕΦΩΝΟΥ (ΠΡΟΑΙΡΕΤΙΚΟ): ");
        string? phoneNumber = Console.ReadLine();

        Console.Write("EMAIL: ");
        string email = Console.ReadLine() ?? string.Empty;
        if (string.IsNullOrEmpty(email))
        {
            Console.WriteLine("ΤΟ EMAIL ΕΙΝΑΙ ΥΠΟΧΡΕΩΤΙΚΟ");
            return null!;
        }

        Console.Write("ΚΩΔΙΚΟΣ: ");
        string password = Console.ReadLine() ?? string.Empty;
        if (string.IsNullOrEmpty(password) || password.Length < 8 )
        {
            Console.WriteLine("Ο ΚΩΔΙΚΟΣ ΕΙΝΑΙ ΥΠΟΧΡΕΩΤΙΚΟΣ ΚΑΙ ΠΡΕΠΕΙ ΝΑ ΕΙΝΑΙ ΤΟΥΛΑΧΙΣΤΟΝ 8 ΧΑΡΑΚΤΗΡΕΣ");
            return null!;
        }

        Console.Write("ΤΥΠΟΣ ΧΡΗΣΤΗ (1 ΓΙΑ ΠΕΛΑΤΗΣ, 2 ΓΙΑ ΕΠΙΣΚΕΥΑΣΤΗΣ): ");
        var typeofUser = Console.ReadLine();
        if (typeofUser is not "2" || typeofUser is not "1")
        {
            Console.WriteLine("ΠΡΕΠΕΙ ΝΑ ΔΩΣΕΙΣ 1 Η 2 ΥΠΟΧΡΕΩΤΙΚΑ");
            return null!;
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
        Console.WriteLine("ΕΠΙΤΥΧΊΑ ΔΗΜΗΟΥΡΓΙΑΣ ΧΡΗΣΤΗ");
        return user;
    }

}
