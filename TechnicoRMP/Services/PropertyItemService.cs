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
using TechnicoRMP.Common;
using TechnicoRMP.DataAccess;
using TechnicoRMP.Models;

namespace TechnicoRMP.Services;

public class PropertyItemService(DataStore dataStore) : IPropertyItemService
{
    private readonly DataStore _dataStore = dataStore;

    public Result<PropertyItem> Create()
    {
        var response = new Result<PropertyItem>()
        {
            Status = -1
        };
        Console.WriteLine("ΕΙΣΑΓΕΤΕ ΤΑ ΣΤΟΙΧΕΙΑ ΤΟΥ ΑΚΙΝΗΤΟΥ:");

        Console.Write("ΑΡΙΘΜΟΣ Ε9: ");
        string e9Number = Console.ReadLine() ?? string.Empty;
        if (e9Number == string.Empty)
        {
            response.Message = "ΤΟ Ε9 ΕΙΝΑΙ ΥΠΟΧΡΕΩΤΙΚΟ";
            return response;
        }

        Console.Write("ΔΙΕΥΘΥΝΣΗ: ");
        string address = Console.ReadLine() ?? string.Empty;
         if (address == string.Empty)
        {
            response.Message = "Η ΔΙΕΥΘΥΝΣΗ ΕΙΝΑΙ ΥΠΟΧΡΕΩΤΙΚΗ";
            return response;
        }

        Console.Write("ΕΤΟΣ ΚΑΤΑΣΚΕΥΗΣ: ");
        string yearOfConstruction = Console.ReadLine() ?? string.Empty;
        if (yearOfConstruction == string.Empty)
        {
            response.Message = "ΤΟ ΕΤΟΣ ΚΑΤΑΣΚΕΥΗΣ ΕΙΝΑΙ ΥΠΟΧΡΕΩΤΙΚΟ";
            return response;
        }

        bool res = int.TryParse(yearOfConstruction, out int year);
        if(!res || year > DateTime.Now.Year || year < 0)
        {
            response.Message = "Η ΧΡΟΝΙΑ ΕΙΝΑΙ ΛΑΘΟΣ";
            return response;
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
                response.Message = "ΑΚΥΡΗ ΕΠΙΛΟΓΗ. Ο ΤΥΠΟΣ ΑΚΙΝΗΤΟΥ ΘΑ ΟΡΙΣΤΕΙ ΣΕ ΔΙΑΜΕΡΙΣΜΑ";
                return response;
                
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
        response.Message = "ΕΠΙΤΥΧΕΣ";
        response.Status = 0;
        response.Value = propertyItem;
        return response;   
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

    public Result Update(PropertyItem propertyItem)
    {
        var response = new Result()
        {
            Status = -1
        };
        if (propertyItem is null)
        {
            response.Message = "ΠΡΕΠΕΙ ΝΑ ΔΩΣΕΙΣ ΣΤΟΙΧΕΙΑ ΑΚΙΝΗΤΟΥ";
            return response;
        }
        if (propertyItem.E9Number is null)
        {
            response.Message = "ΠΡΕΠΕΙ ΝΑ ΔΩΣΕΙΣ ΤΟ Ε9";
            return response;
        }
        if (propertyItem.Address is null)
        {
            response.Message = "ΠΡΕΠΕΙ ΝΑ ΔΩΣΕΙΣ ΔΙΕΥΘΥΝΣΗ";
            return response;
        }
        if (propertyItem.YearOfConstruction < 0 || propertyItem.YearOfConstruction > DateTime.Now.Year)
        {
            response.Message = "ΠΡΕΠΕΙ ΝΑ ΔΩΣΕΙΣ ΣΩΣΤΟ ΕΤΟΣ ΚΑΤΑΣΚΕΥΗΣ";
            return response;
        }

        var propertyItemFromDb = _dataStore.PropertyItems.FirstOrDefault(p => p.E9Number == propertyItem.E9Number);
        if (propertyItemFromDb != null)
        {
            response.Message = "ΔΕΝ ΒΡΕΘΗΚΕ ΑΚΙΝΗΤΟ ΜΕ ΑΥΤΟ ΤΟ Ε9";
            return response;
        }
        _dataStore.Update(propertyItem);
        _dataStore.SaveChanges();
        response.Status = 0;
        response.Message = "ΕΠΙΤΥΧΕΣ";
        return response;
    }
}
