using TechnicoRMP.Database.DataAccess;
using TechnicoRMP.Models;
using TechnicoRMP.Shared.Common;
using TechnicoRMP.Shared.Dtos;

namespace Technico.Api.Services;

public class PropertyItemService(DataStore dataStore) : IPropertyItemService
{
    private readonly DataStore _dataStore = dataStore;

    public Result<CreatePropertyItemResponse> Create(CreatePropertyItemRequest createPropertyItemRequest)
    {
        var response = new Result<CreatePropertyItemResponse>()
        {
            Status = -1
        };
        try
        {
            var propertyItem = new PropertyItem
            {
                E9Number = createPropertyItemRequest.E9Number,
                Address = createPropertyItemRequest.Address,
                YearOfConstruction = createPropertyItemRequest.YearOfConstruction,
                EnPropertyType = createPropertyItemRequest.EnPropertyType,
                
                IsActive = true,
            };

            _dataStore.Add(propertyItem);
            _dataStore.SaveChanges();

            response.Message = "ΕΠΙΤΥΧΕΣ";
            response.Status = 0;
            response.Value = CreatePropertyItemResponseService.CreateFromEntity(propertyItem);
            return response;
        }
        catch (Exception ex)
        {
            return response;
        }

    }

    public List<PropertyItem> ReadPropertyItems()
    {
        try
        {

        return [.. _dataStore.PropertyItems];
        }
        catch(Exception ex)
        {

        }
        return [];
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

    public Result Update(UpdatePropertyItemRequest updatePropertyItemRequest)
    {
        var response = new Result()
        {
            Status = -1
        };

        var propertyItemFromDb = _dataStore.PropertyItems.FirstOrDefault(p => p.E9Number == updatePropertyItemRequest.E9Number);
        if (propertyItemFromDb is null)
        {
            response.Message = "ΔΕΝ ΒΡΕΘΗΚΕ ΑΚΙΝΗΤΟ ΜΕ ΑΥΤΟ ΤΟ Ε9";
            return response;
        }
        if (updatePropertyItemRequest.Address is not null)
        {
            propertyItemFromDb!.Address = updatePropertyItemRequest.Address;
        }
        if (updatePropertyItemRequest.IsActive is not null)
        {
            propertyItemFromDb!.IsActive = updatePropertyItemRequest.IsActive.Value;
        }
        if (updatePropertyItemRequest.EnPropertyType is not null)
        {
            propertyItemFromDb!.EnPropertyType = updatePropertyItemRequest.EnPropertyType.Value;
        }
        if (updatePropertyItemRequest.YearOfConstruction is not null)
        {
            propertyItemFromDb!.YearOfConstruction = updatePropertyItemRequest.YearOfConstruction.Value;
        }

        _dataStore.SaveChanges();
        response.Status = 0;
        response.Message = "ΕΠΙΤΥΧΕΣ";
        return response;
    }
}
