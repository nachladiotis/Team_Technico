using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechnicoRMP.DataAccess;
using TechnicoRMP.Models;

namespace TechnicoRMP.Services;

public class DisplayUserService(DataStore dataStore) : IDisplayService<string>
{
    private readonly DataStore _dataStore = dataStore;

    public void Display(string vatNumber)
    {
        var user = _dataStore
            .Users
            .Include(p => p.PropertyOwnerships)
            .ThenInclude(s => s.PropertyItem)
            .FirstOrDefault(p => p.VatNumber == vatNumber);
        if (user is null)
        {
            return;
        }
        DisplayUserDetails(user);
        DisplayUserPropertyItemsDetails(user);
    
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

    public void DisplayAll()
    {
        var users = _dataStore
           .Users
           .Include(p => p.PropertyOwnerships)
           .ThenInclude(s => s.PropertyItem)
           .ToList();
        foreach (var user in users)
        {
            DisplayUserDetails(user);
            DisplayUserPropertyItemsDetails(user);
        }


    }
}
