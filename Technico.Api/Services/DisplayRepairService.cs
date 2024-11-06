
using TechnicoRMP.Database.DataAccess;
using TechnicoRMP.Models;


namespace Technico.Api.Services;

public class DisplayRepairService(DataStore dataStore) : IDisplayService<long>
{
    private readonly DataStore _dataStore = dataStore;
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
    static void FindUserFromDbWithId(User? userFromPropertyRepair)
    {
        if (userFromPropertyRepair == null)
        {
            Console.WriteLine("ΔΕΝ ΕΧΕΙ ΟΡΙΣΤΕΙ ΧΡΗΣΤΗΣ");
            return;
        }
        Console.WriteLine($"Ο ΧΡΗΣΤΗΣ ΤΟΥ ΕΙΝΑΙ {userFromPropertyRepair.Name} {userFromPropertyRepair.Surname} ΜΕ ΑΦΜ {userFromPropertyRepair.VatNumber}");
    }

    public void DisplayAll()
    {
        var propertyRepairs = _dataStore
              .PropertyRepairs
              .ToList();

        foreach (var item in propertyRepairs)
        {
            ShowDetailsPropertyRepairFromDb(item);
        }


    }
}
