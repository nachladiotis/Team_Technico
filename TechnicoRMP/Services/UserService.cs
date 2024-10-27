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

    public User Create(User user)
    {
        throw new NotImplementedException();
    }
}
