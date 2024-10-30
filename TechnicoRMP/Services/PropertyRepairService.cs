
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using TechnicoRMP.DataAccess;
using TechnicoRMP.Models;
using TechnicoRMP.Responses;

namespace TechnicoRMP.Services;

public class PropertyRepairService(DataStore dataStore) : IPropertyRepairService
{
    private readonly DataStore _dataStore = dataStore;
    public Response<PropertyRepair> Create()
    {
        Console.WriteLine("ΕΙΣΑΓΕΤΕ ΤΑ ΣΤΟΙΧΕΙΑ ΔΗΛΩΣΗΣ:");
        var response = new Response<PropertyRepair>()
        {
           Status = -1
        };

        Console.WriteLine("ΔΩΣΕ CustomerId");        
        if (!long.TryParse(Console.ReadLine(), out long customerId))
        {
            response.Message = "ΠΡΕΠΕΙ ΝΑ ΔΩΣΕΙΣ ΣΩΣΤΟ CustomerId";
            return response;
        }

        Console.WriteLine("ΔΩΣΕ ΔΙΕΥΘΥΝΣΗ");
        var propertyRepairAddress = Console.ReadLine();
        if (propertyRepairAddress == null)
        {
            response.Message = "Η ΔΙΕΥΘΥΝΣΗ ΕΙΝΑΙ ΥΠΟΧΡΕΩΤΙΚΗ";
            return response;
        }
    
        Console.WriteLine("ΤΥΠΟΣ ΕΠΙΣΚΕΥΗΣ (1 για ΒΑΨΙΜΟ, 2 για ΜΟΝΩΣΗ, 3 ΚΟΥΦΩΜΑΤΑ ,4 για ΥΔΡΑΥΛΙΚΑ , 5 για ΗΛΕΚΤΡΟΛΟΓΙΚΑ): ");
        EnTypeOfRepair typeOfRepair = EnTypeOfRepair.Painting;
        switch (Console.ReadLine())
        {
            case "1":
                typeOfRepair = EnTypeOfRepair.Painting;
                break;
            case "2":
                typeOfRepair = EnTypeOfRepair.Insulation;
                break;
            case "3":
                typeOfRepair = EnTypeOfRepair.Frames;
                break;
            case "4":
                typeOfRepair = EnTypeOfRepair.Plumbing;
                break;
            case "5":
                typeOfRepair = EnTypeOfRepair.ElectricalWork;
                break;
            default:
                Console.WriteLine("ΑΚΥΡΗ ΕΠΙΛΟΓΗ. Ο ΤΥΠΟΣ ΕΠΙΣΚΕΥΗΣ ΘΑ ΟΡΙΣΤΕΙ ΣΕ ΒΑΨΙΜΟ.");
                typeOfRepair = EnTypeOfRepair.Painting;
                break;
        }
        Console.WriteLine("ΕΙΣΑΓΕΤΕ ΤΟ ΚΟΣΤΟΣ:");
        var propertyRepairCost = Console.ReadLine();


        if (!decimal.TryParse(propertyRepairCost, out decimal propertyRepairCostDec))
        {
            propertyRepairCostDec = 0;
            response.Message = "ΕΔΩΣΕΣ ΑΚΥΡΟ ΚΟΣΤΟΣ ΟΠΟΤΕ ΘΑ ΕΧΕΙ ΤΙΜΗ 0";
            return response;
        }

        Console.WriteLine("ΚΑΤΑΣΤΑΣΗ ΕΠΙΣΚΕΥΗΣ (1 για ΟΛΟΚΛΗΡΩΘΗΚΕ, 2 για ΣΕ ΕΞΕΛΙΞΗ, αφήστε κενό για ΕΚΚΡΕΜΕΙ): ");
        EnRepairStatus repairStatus = EnRepairStatus.Pending;
        string? statusInput = Console.ReadLine();
        if (statusInput == "1")
        {
            repairStatus = EnRepairStatus.Complete;
        }
        else if (statusInput == "2")
        {
            repairStatus = EnRepairStatus.Inprogress;
        }

        Console.WriteLine("ΔΩΣΕ ID ΧΡΗΣΤΗ ΤΟΥ ΑΚΙΝΗΤΟΥ ΠΟΥ ΖΗΤΗΣΕ ΕΠΙΣΚΕΥΗ");
        var propertyOwnerId = Console.ReadLine();
        if(!long.TryParse(propertyOwnerId, out long propertyOwnerIdToSearch))
        {
            response.Message = "ΤΟ ID ΕΙΝΑΙ ΥΠΟΧΡΕΩΤΙΚΟ";
            return response;
        }

        var propertyOwner = _dataStore.Users
          .FirstOrDefault(p => p.Id == propertyOwnerIdToSearch);
        if (propertyOwner == null)
        {
            response.Message = "ΔΕΝ ΒΡΕΘΗΚΕ ΧΡΗΣΤΗΣ ΜΕ ΑΥΤΟ ΤΟ ID";
            return response;
        }

        var propertyRepairToStore = new PropertyRepair
        {
            Date = DateTime.UtcNow,
            Address = propertyRepairAddress,
            TypeOfRepair = typeOfRepair,
            RepairStatus = repairStatus,
            Cost = propertyRepairCostDec,
            CustomerId = customerId,
            IsActive = true
        };
        _dataStore.Add(propertyRepairToStore);
        _dataStore.SaveChanges();

        response.Status = 0;
        response.Value = propertyRepairToStore;
        response.Message = "ΕΠΙΤΥΧΕΣ";

        return response;
    }

    public bool Delete(long id)
    {
        if (id <= 0)
        {
            Console.WriteLine("ΤΟ ID ΕΙΝΑΙ ΥΠΟΧΡΕΩΤΙΚΟ ΚΑΙ ΔΕΝ ΔΈΧΕΤΑΙ ΑΡΝΗΤΙΚΆ");
            return false;
        }

        var propertyRepairFromDb = _dataStore
               .PropertyRepairs
               .FirstOrDefault(p => p.Id == id);


        if (propertyRepairFromDb == null )
        {
            Console.WriteLine("ΔΕΝ ΒΡΕΘΗΚΕ ΤΟ PROPERTYREPAIR ΜΕ ΤΟ ID ΠΟΥ ΕΔΩΣΕΣ");
            return false;
        }
        _dataStore.Remove(id);
        _dataStore.SaveChanges();
        return true;

    }

    public void Display(long id)

    {
        if (id <= 0)
        {
            Console.WriteLine("ΤΟ ID ΕΙΝΑΙ ΥΠΟΧΡΕΩΤΙΚΟ ΚΑΙ ΔΕΝ ΔΈΧΕΤΑΙ ΑΡΝΗΤΙΚΆ");
            return;
        }

        var propertyRepairFromDb = _dataStore
               .PropertyRepairs
               .FirstOrDefault(p => p.Id == id);


        if (propertyRepairFromDb == null)
        {
            Console.WriteLine("ΔΕΝ ΒΡΕΘΗΚΕ ΤΟ PROPERTYREPAIR ΜΕ ΤΟ ID ΠΟΥ ΕΔΩΣΕΣ");
            return;
        }
        Console.WriteLine($"TO PROPERTYREPAIR ΠΟΥ ΖΗΤΗΣΕΣ ΜΕ ID {id} ΕΙΝΑΙ:");
        ShowDetailsPropertyRepairFromDb(propertyRepairFromDb);
    }

    public Response Update(PropertyRepair propertyRepair)
    {
        var response = new Response()
        {
            Status = -1
        };

        if (propertyRepair == null)
        {
            response.Message = "ΠΡΕΠΕΙ ΝΑ ΔΩΣΕΙΣ ΣΤΟΙΧΕΙΑ";
            return response;
        }
        if (propertyRepair.Id == 0 || propertyRepair.Id <0)
        {
            response.Message = "ΤΟ ΠΕΔΙΟ ID ΣΤΟ PROPERTYREPAIR ΕΙΝΑΙ ΥΠΟΧΡΕΩΤΙΚΟ ΚΑΙ ΜΕΓΑΛΥΤΕΡΟ ΑΠΟ ΤΟ 0";
            return response;
        }
        var propertyRepairFromDb =_dataStore
            .PropertyRepairs
            .FirstOrDefault(p => p.Id == propertyRepair.Id);
        if (propertyRepairFromDb == null)
        {
            response.Message = "ΔΕΝ ΒΡΕΘΗΚΕ PORPERTYREPAIR ΜΕ ΤΟ ID ΤΟΥ PROPERTYREPAIR ID ΠΟΥ ΕΔΩΣΕΣ";
            return response;
        }
       
        _dataStore.Update(propertyRepair);
        _dataStore.SaveChanges();
        
        ShowDetailsPropertyRepairFromDb(propertyRepair);
        response.Message = "ΕΠΙΤΥΧΕΣ";
        response.Status = 0;
        return response;

    }
    static void FindUserFromDbWithId(User? userFromPropertyRepair)
    {
        if (userFromPropertyRepair == null)
        {
            Console.WriteLine("ΔΕΝ ΕΧΕΙ ΟΡΙΣΤΕΙ ΧΡΗΣΤΗΣ");
            return;
        }
        Console.WriteLine($"Ο ΧΡΗΣΤΗΣ ΤΟΥ ΕΙΝΑΙ {userFromPropertyRepair.Name} {userFromPropertyRepair.Surname} ΜΕ ΑΦΜ {userFromPropertyRepair.VatNumber}");
    }
    void ShowDetailsPropertyRepairFromDb(PropertyRepair propertyRepairFromDb)
    {
        Console.WriteLine(propertyRepairFromDb.Date);
        Console.WriteLine(propertyRepairFromDb.TypeOfRepair);
        Console.WriteLine(propertyRepairFromDb.Address);
        Console.WriteLine(propertyRepairFromDb.Cost);
        Console.WriteLine(propertyRepairFromDb.RepairStatus);
        Console.WriteLine(propertyRepairFromDb.IsActive ? "ΕΝΕΡΓΗ" : "ΑΝΕΝΕΡΓΗ");
        var userFromPropertyRepair = _dataStore.Users.FirstOrDefault(p => p.Id == propertyRepairFromDb.Id);
        FindUserFromDbWithId(userFromPropertyRepair);
    }
}
