using Microsoft.EntityFrameworkCore;
using TechnicoRMP.Database.DataAccess;
using TechnicoRMP.Models;
using TechnicoRMP.Shared.Common;
using TechnicoRMP.Shared.Dtos;

namespace Technico.Api.Services;

public class PropertyRepairService(DataStore dataStore) : IPropertyRepairService
{
    private readonly DataStore _dataStore = dataStore;

    public async Task<Result<CreatePropertyRepairResponse>> AddRepair(CreatePropertyRepairRequest createPropertyRepairRequest)
    {
        var response = new Result<CreatePropertyRepairResponse>()
        {
            Status = -1
        };

        try
        {
            if (createPropertyRepairRequest.RepairerId <= 0)
            {
                response.Message = "Repairer ID is required and must be greater than 0.";
                return response;
            }

            if (string.IsNullOrWhiteSpace(createPropertyRepairRequest.Address) || createPropertyRepairRequest.Address == "string")
            {
                response.Message = "Address is required and must be a valid address.";
                return response;
            }

            var propertyOwner = await _dataStore.Users
                .FirstOrDefaultAsync(p => p.Id == createPropertyRepairRequest.RepairerId);

            if (propertyOwner == null)
            {
                response.Message = "User with this ID not found.";
                return response;
            }

            var propertyRepairToStore = new PropertyRepair
            {
                Date = DateTime.UtcNow,
                Address = createPropertyRepairRequest.Address,
                TypeOfRepair = createPropertyRepairRequest.TypeOfRepair,
                Cost = createPropertyRepairRequest.Cost,
                RepairerId = createPropertyRepairRequest.RepairerId,
                IsActive = true
            };

            await _dataStore.AddAsync(propertyRepairToStore);
            await _dataStore.SaveChangesAsync();

            response.Status = 0;
            response.Message = "Repair added successfully.";
            response.Value = CreatePropertyRepairResponseService.CreateFromEntity(propertyRepairToStore);
        }
        catch (Exception ex)
        {
            response.Status = -1;
            response.Message = $"An error occurred while adding the repair: {ex.Message}";
        }
        return response;
    }

    public async Task<Result<List<CreatePropertyRepairResponse>>> GetAll()
    {
        var response = new Result<List<CreatePropertyRepairResponse>>
        {
            Status = -1,
            Message = "No active repairs found."
        };

        try
        {

            var repairs = await _dataStore.PropertyRepairs
                .Where(r => r.IsActive)
                .Select(r => CreatePropertyRepairResponseService.CreateFromEntity(r))
                .ToListAsync();


            if (repairs == null)
            {
                return response;
            }


            response.Status = 0;
            response.Message = "Repairs found successfully.";
            response.Value = repairs;
        }
        catch (Exception ex)
        {
            response.Status = -1;
            response.Message = $"An error occurred while retrieving repairs: {ex.Message}";
        }

        return response;
    }

    public async Task<Result<CreatePropertyRepairResponse>> GetById(int id)
    {
        var result = new Result<CreatePropertyRepairResponse>
        {
            Status = -1,
            Message = "An error occurred while retrieving the repair."
        };

        try
        {
            if (id <= 0)
            {
                result.Message = "Invalid ID provided.";
                return result;
            }


            var propertyRepair = await _dataStore.PropertyRepairs
                .FirstOrDefaultAsync(r => r.Id == id && r.IsActive);

            if (propertyRepair == null)
            {
                result.Message = "Repair not found.";
                return result;
            }

            result.Status = 0;
            result.Message = "Repair found successfully.";
            result.Value = CreatePropertyRepairResponseService.CreateFromEntity(propertyRepair);

            return result;
        }
        catch (Exception ex)
        {
            result.Status = -1;
            result.Message = $"An error occurred: {ex.Message}";
            return result;
        }
    }

    public async Task<Result<PropertyRepair>> SoftDeleteRepairForUser(int userId, int repairId)
    {
        var result = new Result<PropertyRepair>
        {
            Status = -1
        };

        try
        {
            if (userId <= 0 || repairId <= 0)
            {
                result.Message = "The user ID and repair ID must be greater than zero.";
                return result;
            }

            var repair = await _dataStore.PropertyRepairs
                .FirstOrDefaultAsync(p => p.Id == repairId && p.IsActive && p.RepairerId == userId);

            if (repair == null)
            {
                result.Message = "Repair not found, or it does not belong to the user, or it is already inactive.";
                return result;
            }

            if (!repair.IsActive)
            {
                result.Message = "The repair is already inactive.";
                return result;
            }

            repair.IsActive = false;

            _dataStore.PropertyRepairs.Update(repair);
            await _dataStore.SaveChangesAsync();

            result.Status = 0;
            result.Message = "Repair was successfully deleted.";
            result.Value = repair;

            return result;
        }
        catch (Exception ex)
        {
            // Διαχείριση εξαιρέσεων
            result.Status = 500;
            result.Message = $"An error occurred during the operation: {ex.Message}";
            return result;
        }
    }
    public async Task<Result> Update(UpdatePropertyRepair updatePropertyRepair)
    {
        var response = new Result
        {
            Status = -1
        };

        try
        {
            if (updatePropertyRepair.Id <= 0)
            {
                response.Message = "The ID is required and must be greater than 0.";
                return response;
            }

            var repair = await _dataStore.PropertyRepairs
                .FirstOrDefaultAsync(p => p.Id == updatePropertyRepair.Id);

            if (repair == null)
            {
                response.Message = "Repair with the given ID was not found.";
                return response;
            }

            if (updatePropertyRepair.RepairerId.HasValue && updatePropertyRepair.RepairerId.Value != repair.RepairerId)
            {
                response.Message = "Repairer ID cannot be updated.";
                return response;
            }

            if (updatePropertyRepair.Id != repair.Id)
            {
                response.Message = "Repair ID cannot be changed.";
                return response;
            }

            bool isUpdated = false;

            if (!string.IsNullOrEmpty(updatePropertyRepair.Address))
            {
                if (updatePropertyRepair.Address == "string")
                {
                    response.Message = "Address cannot be updated to 'string'.";
                    return response;
                }

                repair.Address = updatePropertyRepair.Address;
                isUpdated = true;
            }

            if (updatePropertyRepair.TypeOfRepair.HasValue)
            {
                repair.TypeOfRepair = updatePropertyRepair.TypeOfRepair.Value;
                isUpdated = true;
            }

            if (updatePropertyRepair.Cost.HasValue)
            {
                repair.Cost = updatePropertyRepair.Cost.Value;
                isUpdated = true;
            }

            if (updatePropertyRepair.IsActive.HasValue)
            {
                repair.IsActive = updatePropertyRepair.IsActive.Value;
                isUpdated = true;
            }

            if (updatePropertyRepair.RepairStatus.HasValue)
            {
                repair.RepairStatus = updatePropertyRepair.RepairStatus.Value;
                isUpdated = true;
            }

            if (!isUpdated)
            {
                response.Status = 1;
                response.Message = "No updates were made as no changes were found.";
                return response;
            }

            await _dataStore.SaveChangesAsync();

            response.Status = 0;
            response.Message = "Update successful.";
            return response;
        }
        catch (Exception ex)
        {
            response.Status = 500;
            response.Message = $"An error occurred during the update: {ex.Message}";
            return response;
        }
    }
}