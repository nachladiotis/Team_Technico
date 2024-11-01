using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechnicoRMP.DataAccess;
using TechnicoRMP.Models;

namespace TechnicoRMP.Services;

public class DisplayItemService(DataStore datastore) : IDisplayService<string>
{
    private readonly DataStore _dataStore = datastore;

    public void Display(string e9Number)
    {
        if (e9Number == string.Empty)
        {
            Console.WriteLine("ΤΟ Ε9 ΕΙΝΑΙ ΥΠΟΧΡΕΩΤΙΚΟ");
            return;
        }

        var propertyItem = _dataStore.PropertyItems
            .Include(p => p.PropertyOwnerships)
            .ThenInclude(p => p.PropertyOwner)
            .FirstOrDefault(s => s.E9Number == e9Number);
        if (propertyItem is null)
        {
            Console.WriteLine("ΤΟ ΑΚΙΝΗΤΟ ΔΕΝ ΒΡΕΘΗΚΕ");
            return;
        }
        DisplayPropertyItem(propertyItem);
    }
    public void DisplayAll()
    {
        var propertyItems = _dataStore.PropertyItems
            .Include(p => p.PropertyOwnerships)
            .ThenInclude(p => p.PropertyOwner).ToList();

        foreach (var item in propertyItems)
        {
            DisplayPropertyItem(item);
        }
    }

    private static void DisplayPropertyItem(PropertyItem propertyItem)
    {
        Console.WriteLine("ΣΤΟΙΧΕΙΑ ΑΚΙΝΗΤΟΥ:");
        Console.WriteLine($"ΑΡΙΘΜΟΣ Ε9: {propertyItem.E9Number}");
        Console.WriteLine($"ΔΙΕΥΘΥΝΣΗ: {propertyItem.Address}");
        Console.WriteLine($"ΕΤΟΣ ΚΑΤΑΣΚΕΥΗΣ: {propertyItem.YearOfConstruction}");
        switch (propertyItem.EnPropertyType)
        {
            case EnPropertyType.DetachedHouse:
                Console.WriteLine($"ΤΥΠΟΣ ΑΚΙΝΗΤΟΥ ΜΟΝΟΚΑΤΟΙΚΙΑ");
                break;
            case EnPropertyType.Apartment:
                Console.WriteLine($"ΤΥΠΟΣ ΑΚΙΝΗΤΟΥ ΔΙΑΜΕΡΙΣΜΑ");
                break;
            case EnPropertyType.Maisonet:
                Console.WriteLine($"ΤΥΠΟΣ ΑΚΙΝΗΤΟΥ ΜΕΖΟΝΕΤΑ");
                break;
            default:
                break;
        }
        foreach (var propertyOwnership in propertyItem.PropertyOwnerships)
        {
            var propertyOwner = propertyOwnership.PropertyOwner;
            if (propertyOwner is null)
            {
                continue;
            }
            Console.WriteLine($"TO AKINHTO EXEI ΙΔΙΟΚΤΗΤH: {propertyOwner.Name} {propertyOwner.Surname}");
        }

        var isAcrtiveText = propertyItem.IsActive is true ? "ΕΝΕΡΓΟ" : "ΑΝΕΝΕΡΓΟ";
        Console.WriteLine(isAcrtiveText);
    }

 
}
