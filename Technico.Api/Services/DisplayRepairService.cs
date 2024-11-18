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
            return;
        }

        var propertyRepairFromDb = _dataStore
               .PropertyRepairs
               .FirstOrDefault(p => p.Id == id);


        if (propertyRepairFromDb == null)
        {
            return;
        }
        ShowDetailsPropertyRepairFromDb(propertyRepairFromDb);
    }

    void ShowDetailsPropertyRepairFromDb(PropertyRepair propertyRepairFromDb)
    {
        var userFromPropertyRepair = _dataStore.Users.FirstOrDefault(p => p.Id == propertyRepairFromDb.Id);
        FindUserFromDbWithId(userFromPropertyRepair);
    }
    static void FindUserFromDbWithId(User? userFromPropertyRepair)
    {

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
