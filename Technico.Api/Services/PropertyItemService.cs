using Microsoft.EntityFrameworkCore;
using Technico.Api.Validations;
using TechnicoRMP.Database.DataAccess;
using TechnicoRMP.Models;
using TechnicoRMP.Shared.Common;
using TechnicoRMP.Shared.Dtos;

namespace Technico.Api.Services;

public class PropertyItemService(DataStore dataStore, IPropertyItemValidation validation, ILogger<PropertyItemService> logger) : IPropertyItemService
{
    private readonly DataStore _dataStore = dataStore;
    private readonly IPropertyItemValidation _validation;
    private readonly ILogger<PropertyItemService> _logger = logger;

    public Result<CreatePropertyItemResponse> Create(CreatePropertyItemRequest createPropertyItemRequest)
    {
        if (!validation.PropertyItemValidator(createPropertyItemRequest))
        {
            return null!;
        }
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
            var proprtyOwnership = new PropertyOwnership
            {
                PropertyItemId = propertyItem.Id,
                PropertyOwnerId = createPropertyItemRequest.UserId,
            };
            _dataStore.Add(proprtyOwnership);
            _dataStore.SaveChanges();
            response.Message = "ΕΠΙΤΥΧΕΣ";
            response.Status = 0;
            response.Value = CreatePropertyItemResponseService.CreateFromEntity(propertyItem);
            return response;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            return response;
        }

    }
    public List<PropertyItem> ReadPropertyItems()
    {
        try
        {

            return [.. _dataStore.PropertyItems];
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
        }
        return [];
    }
    public async Task<Result<CreatePropertyItemResponse>> GetById(int id)
    {
        var result = new Result<CreatePropertyItemResponse>
        {
            Status = -1,
            Message = "An error occurred while retrieving the item."
        };

        try
        {
            if (id <= 0)
            {
                result.Message = "Invalid ID provided.";
                return result;
            }


            var propertyItem = await _dataStore.PropertyItems
                .FirstOrDefaultAsync(r => r.Id == id && r.IsActive);

            if (propertyItem == null)
            {
                result.Message = "Item not found.";
                return result;
            }

            result.Status = 0;
            result.Message = "Item found successfully.";
            result.Value = CreatePropertyItemResponseService.CreateFromEntity(propertyItem);

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            result.Status = -1;
            result.Message = $"An error occurred: {ex.Message}";
            return result;
        }
    }

    public async Task<Result<PropertyItemsByUserDto>> GetPropertyItemByUserId(int userId)
    {
        var result = new Result<PropertyItemsByUserDto>
        {
            Status = -1,
            Message = "An error occurred while retrieving the item."
        };

        try
        {
            if (userId <= 0)
            {
                result.Message = "Invalid ID provided.";
                return result;
            }
            var propertyItem = await _dataStore
            .PropertyOwnerships
            .Include(s => s.PropertyOwner)
            .Include(s => s.PropertyItem)
            .Where(s => s.PropertyOwnerId == userId).ToListAsync();

            if (propertyItem == null)
            {
                result.Message = "Item not found.";
                return result;
            }

            result.Status = 0;
            result.Message = "Item found successfully.";
            result.Value = CreatePropertyItemResponseService.Create(propertyItem);

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            result.Status = -1;
            result.Message = $"An error occurred: {ex.Message}";
            return result;
        }
    }
    public bool Delete(int id)
    {
        if (id == 0)
        {
            return false;
        }
        var propertyItem = _dataStore.PropertyItems.FirstOrDefault(s => s.Id == id);
        if (propertyItem is null)
        {
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

        var propertyItemFromDb = _dataStore.PropertyItems.FirstOrDefault(p => p.Id == updatePropertyItemRequest.Id);
        if (propertyItemFromDb is null)
        {
            response.Message = "No property found with the given E9 number";
            return response;
        }
        if (updatePropertyItemRequest.E9Number is not null)
        {
            propertyItemFromDb!.E9Number = updatePropertyItemRequest.E9Number;
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
        response.Message = "Property updated successfully.";
        return response;
    }
    public async Task<Result<CreatePropertyItemResponse>> CreatePropertyItemByUserId(CreatePropertyItemRequest createPropertyItemRequest)
    {
        var response = new Result<CreatePropertyItemResponse>()
        {
            Status = -1
        };

        try
        {
            if (createPropertyItemRequest.UserId <= 0)
            {
                response.Message = "Repairer ID is required and must be greater than 0.";
                return response;
            }

            if (string.IsNullOrWhiteSpace(createPropertyItemRequest.Address))
            {
                response.Message = "Address is required and must be a valid address.";
                return response;
            }

            var propertyOwner = await _dataStore.Users
                .FirstOrDefaultAsync(p => p.Id == createPropertyItemRequest.UserId);

            if (propertyOwner == null)
            {
                response.Message = "User with this ID not found.";
                return response;
            }

            var propertyItemToStore = new PropertyItem
            {
                Address = createPropertyItemRequest.Address,
                E9Number = createPropertyItemRequest.E9Number,
                EnPropertyType = createPropertyItemRequest.EnPropertyType,
                YearOfConstruction = createPropertyItemRequest.YearOfConstruction,
                IsActive = true
            };

            await _dataStore.AddAsync(propertyItemToStore);
            await _dataStore.SaveChangesAsync();

            
            var propertyOwnership = new PropertyOwnership
            {
                PropertyOwnerId = createPropertyItemRequest.UserId,
            };

            propertyItemToStore.PropertyOwnerships.Add(propertyOwnership);

            await _dataStore.AddAsync(propertyOwnership);
            await _dataStore.SaveChangesAsync();

            response.Status = 0;
            response.Message = "Item added successfully.";

            response.Value = CreatePropertyItemResponseService.CreateFromEntity(propertyItemToStore);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            response.Status = -1;
            response.Message = $"An error occurred while adding the item: {ex.Message}";
        }
        return response;
    }
    public async Task<List<PropertyItemsDto>> GetByNumber(string? e9Number)
    {

        List<PropertyItem> items;
        if (string.IsNullOrEmpty(e9Number))
        {
            items = await _dataStore.PropertyItems.ToListAsync();
        }
        else
        {
            items = await _dataStore.PropertyItems
               .Where(i => i.E9Number.Contains(e9Number))
               .AsNoTracking()
               .ToListAsync();
        }

        var dtos = items.Select(s => new PropertyItemsDto
        {
            Address = s.Address,
            E9Number = s.E9Number,
            EnPropertyType = s.EnPropertyType,
            IsActive = s.IsActive,
            YearOfConstruction = s.YearOfConstruction,
            Id = s.Id
        }).ToList();

        return dtos;
    }
}
