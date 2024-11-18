using Microsoft.EntityFrameworkCore;
using TechnicoRMP.Database.DataAccess;
using TechnicoRMP.Models;


namespace Technico.Api.Services;


public class DisplayItemService(DataStore datastore) : IDisplayService<string>
{
    private readonly DataStore _dataStore = datastore;

    public void Display(string e9Number)
    {
        if (e9Number == string.Empty)
        {
            return;
        }

        var propertyItem = _dataStore.PropertyItems
            .Include(p => p.PropertyOwnerships)
            .ThenInclude(p => p.PropertyOwner)
            .FirstOrDefault(s => s.E9Number == e9Number);
        if (propertyItem is null)
        {
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
        switch (propertyItem.EnPropertyType)
        {
            case EnPropertyType.DetachedHouse:
                break;
            case EnPropertyType.Apartment:
                break;
            case EnPropertyType.Maisonet:
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
        }
    }


}
