using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using TechnicoRMP.DataAccess;
using TechnicoRMP.Models;

namespace TechnicoRMP.Services;

public class PropertyItemService(DataStore dataStore) : IPropertyItemService
{
    private readonly DataStore _dataStore = dataStore;

    public PropertyItem Create()
    {
        Console.WriteLine("ΕΙΣΑΓΕΤΕ ΤΑ ΣΤΟΙΧΕΙΑ ΤΟΥ ΑΚΙΝΗΤΟΥ:");

        Console.Write("ΑΡΙΘΜΟΣ Ε9: ");
        string e9Number = Console.ReadLine() ?? string.Empty;
        if (e9Number == string.Empty)
        {
            Console.WriteLine("ΤΟ Ε9 ΕΙΝΑΙ ΥΠΟΧΡΕΩΤΙΚΟ");
            return null!;
        }

        Console.Write("ΔΙΕΥΘΥΝΣΗ: ");
        string address = Console.ReadLine() ?? string.Empty;
         if (address == string.Empty)
        {
            Console.WriteLine("Η ΔΙΕΥΘΥΝΣΗ ΕΙΝΑΙ ΥΠΟΧΡΕΩΤΙΚΗ");
            return null!;
        }

        Console.Write("ΕΤΟΣ ΚΑΤΑΣΚΕΥΗΣ: ");
        string yearOfConstruction = Console.ReadLine() ?? string.Empty;
        if (yearOfConstruction == string.Empty)
        {
            Console.WriteLine("ΤΟ ΕΤΟΣ ΚΑΤΑΣΚΕΥΗΣ ΕΙΝΑΙ ΥΠΟΧΡΕΩΤΙΚΟ");
            return null!;
        }

        bool res = int.TryParse(yearOfConstruction, out int year);
        if(!res || year > DateTime.Now.Year || year < 0)
        {
            Console.WriteLine("Η ΧΡΟΝΙΑ ΕΙΝΑΙ ΛΑΘΟΣ");
        }
       

        Console.WriteLine("ΤΥΠΟΣ ΑΚΙΝΗΤΟΥ (1 για ΔΙΑΜΕΡΙΣΜΑ, 2 για ΜΟΝΟΚΑΤΟΙΚΙΑ, 3 για ΜΕΖΟΝΕΤΑ): ");
        EnPropertyType propertyType = EnPropertyType.Apartment; 
        switch (Console.ReadLine())
        {
            case "1":
                propertyType = EnPropertyType.Apartment;
                break;
            case "2":
                propertyType = EnPropertyType.DetachedHouse;
                break;
            case "3":
                propertyType = EnPropertyType.Maisonet;
                break;
            default:
                Console.WriteLine("ΑΚΥΡΗ ΕΠΙΛΟΓΗ. Ο ΤΥΠΟΣ ΑΚΙΝΗΤΟΥ ΘΑ ΟΡΙΣΤΕΙ ΣΕ ΔΙΑΜΕΡΙΣΜΑ.");
                break;
        }
        var propertyItem = new PropertyItem
        {
            E9Number = e9Number,
            Address = address,
            YearOfConstruction = year,
            EnPropertyType = propertyType,
            IsActive = true
        };
        _dataStore.Add(propertyItem);
        _dataStore.SaveChanges();
        Console.WriteLine("ΤΟ ΔΙΑΜΕΡΙΣΑΜ ΔΗΜΙΟΥΡΓΉΘΗΚΕ");
        return propertyItem;   
    }


    public bool Delete(string e9Number)
    {
        if (e9Number == string.Empty)
        {
            Console.WriteLine("ΤΟ Ε9 ΕΙΝΑΙ ΥΠΟΧΡΕΩΤΙΚΟ");
            return false;
        }
        var propertyItem = _dataStore.PropertyItems.FirstOrDefault(s => s.E9Number == e9Number);
        if (propertyItem is null)
        {
            Console.WriteLine("ΤΟ ΑΚΙΝΗΤΟ ΔΕΝ ΒΡΕΘΗΚΕ");
            return false;
        }
        _dataStore.PropertyItems.Remove(propertyItem);
        var deleted = _dataStore.SaveChanges();
        return deleted > 0;
    }

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
        foreach ( var propertyOwnership in propertyItem.PropertyOwnerships)
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

    public void Update(PropertyItem propertyItem)
    {
        if (propertyItem is null)
        {
            Console.WriteLine("ΠΡΕΠΕΙ ΝΑ ΔΩΣΕΙΣ ΣΤΟΙΧΕΙΑ ΑΚΙΝΗΤΟΥ");
            return;
        }
        if (propertyItem.E9Number is null)
        {
            Console.WriteLine("ΠΡΕΠΕΙ ΝΑ ΔΩΣΕΙΣ ΤΟ Ε9");
            return;
        }
        if (propertyItem.Address is null)
        {
            Console.WriteLine("ΠΡΕΠΕΙ ΝΑ ΔΩΣΕΙΣ ΔΙΕΥΘΥΝΣΗ ");
            return;
        }
        if (propertyItem.YearOfConstruction < 0 || propertyItem.YearOfConstruction > DateTime.Now.Year)
        {
            Console.WriteLine("ΠΡΕΠΕΙ ΝΑ ΔΩΣΕΙΣ ΣΩΣΤΟ ΕΤΟΣ ΚΑΤΑΣΚΕΥΗΣ ");
            return;
        }

        var propertyItemFromDb = _dataStore.PropertyItems.FirstOrDefault(p => p.E9Number == propertyItem.E9Number);
        if (propertyItemFromDb != null)
        {
            Console.WriteLine("ΔΕΝ ΒΡΕΘΗΚΕ ΑΚΙΝΗΤΟ ΜΕ ΑΥΤΟ ΤΟ Ε9");
        }
        _dataStore.Update(propertyItem);
        _dataStore.SaveChanges();
    }
}
