using Microsoft.EntityFrameworkCore;
using TechnicoRMP.Database.DataAccess;
using TechnicoRMP.Models;
using TechnicoRMP.Shared.Common;
using TechnicoRMP.Shared.Dtos;

namespace Technico.Api.Services;

public class PropertyRepairService(DataStore dataStore) : IPropertyRepairService
{
    private readonly DataStore _dataStore = dataStore;

    public async Task<Result<PropertyRepairResponseDTO>> AddRepair(CreatePropertyRepairRequest createPropertyRepairRequest)
    {
        var response = new Result<PropertyRepairResponseDTO>()
        {
            Status = -1
        };

        try
        {
            if (createPropertyRepairRequest.UserId <= 0)
            {
                response.Message = "Repairer ID is required and must be greater than 0.";
                return response;
            }

            if (string.IsNullOrWhiteSpace(createPropertyRepairRequest.Address) || createPropertyRepairRequest.Address == "string")
            {
                response.Message = "Address is required and must be a valid address.";
                return response;
            }

            if (createPropertyRepairRequest.Date <= DateTime.UtcNow)
            {
                response.Message = "The repair date must be in the future.";
                return response;
            }

            var propertyOwner = await _dataStore.Users
                .FirstOrDefaultAsync(p => p.Id == createPropertyRepairRequest.UserId);

            if (propertyOwner == null)
            {
                response.Message = "User with this ID not found.";
                return response;
            }

                var propertyItem = await _dataStore.PropertyItems
               .Include(p => p.PropertyOwnerships) 
               .FirstOrDefaultAsync(p =>
                   p.Address == createPropertyRepairRequest.Address &&
                   p.PropertyOwnerships.Any(o => o.PropertyOwnerId == createPropertyRepairRequest.UserId));


            if (propertyItem == null)
            {
                response.Message = "No property found with the given address for this user.";
                return response;
            }

            var propertyRepairToStore = new PropertyRepair
            {
                Date = DateTime.UtcNow,
                Address = createPropertyRepairRequest.Address,
                TypeOfRepair = createPropertyRepairRequest.TypeOfRepair,
                Cost = createPropertyRepairRequest.Cost,
                UserId = createPropertyRepairRequest.UserId,
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

    public async Task<bool> Delete(int repairId)
    {
        var repair = await _dataStore.PropertyRepairs.FirstOrDefaultAsync(s => s.Id == repairId);

        if (repair is null)
            return false;

        _dataStore.PropertyRepairs.Remove(repair);
        var changes = await _dataStore.SaveChangesAsync();
        return changes > 0 ;
    }

    public async Task<List<PropertyRepairResponseDTO>> GetAll()
    {
       var repairs = await _dataStore.PropertyRepairs
                .Where(r => r.IsActive)
                .Select(r => CreatePropertyRepairResponseService.CreateFromEntity(r))
                .ToListAsync();

        return repairs;
    }

    public async Task<List<PropertyRepairResponseDTO>> GetByDateRange(DateTime? startDate, DateTime? endDate)
    {
        var repairs = _dataStore.PropertyRepairs.AsQueryable();

       
        if (startDate.HasValue)
        {
            repairs = repairs.Where(r => r.Date >= startDate.Value);
        }
        if (endDate.HasValue)
        {
            repairs = repairs.Where(r => r.Date <= endDate.Value);
        }

        var result = await repairs.Select(r => new PropertyRepairResponseDTO
        {
            Id = r.Id,
            Date = r.Date,
            TypeOfRepair = r.TypeOfRepair,
            Address = r.Address,
            RepairStatus = r.RepairStatus,
            Cost = r.Cost,
            UserId = r.UserId,
            IsActive = r.IsActive
        }).ToListAsync();

        return result;
    }

    public async Task<Result<PropertyRepairResponseDTO>> GetById(long id)
    {
        var result = new Result<PropertyRepairResponseDTO>
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
    public async Task<Result<List<PropertyRepairResponseDTO>>> GetByUserId(long userId)
    {
        var result = new Result<List<PropertyRepairResponseDTO>>
        {
            Status = -1,
            Message = "An error occurred while retrieving repairs."
        };

        try
        {
            if (userId <= 0)
            {
                result.Message = "Invalid User Id provided.";
                return result;
            }

            var userExists = await _dataStore.Users
                .AnyAsync(u => u.Id == userId );

            if (!userExists)
            {
                result.Message = "User not found.";
                return result;
            }

            var repairs = await _dataStore.PropertyRepairs
                .Where(r => r.UserId == userId && r.IsActive)
                .ToListAsync();

            if (!repairs.Any())
            {
                result.Message = "No repairs found for the specified user.";
                return result;
            }

            var repairDtos = repairs.Select(CreatePropertyRepairResponseService.CreateFromEntity).ToList();

            result.Status = 0;
            result.Message = "Repairs retrieved successfully.";
            result.Value = repairDtos;

            return result;
        }
        catch (Exception ex)
        {
            result.Status = -1;
            result.Message = $"An error occurred: {ex.Message}";
            return result;
        }
    }


   
    public async Task<Result<PropertyRepair>> SoftDeleteRepairForUser(int repairId)
    {
        var result = new Result<PropertyRepair>
        {
            Status = -1,
            Message = "An error occurred while attempting to deactivate the repair."
        };

        try
        {
            // Ελέγχουμε αν τα userId και repairId είναι έγκυρα
            if (repairId <= 0)
            {
                result.Message = "The user ID and repair ID must be greater than zero.";
                return result;
            }

            // Βρίσκουμε την επισκευή που αντιστοιχεί στο userId και repairId και είναι ενεργή
            var repair = await _dataStore.PropertyRepairs
                .FirstOrDefaultAsync(p => p.Id == repairId && p.IsActive );

            // Ελέγχουμε αν βρέθηκε η επισκευή
            if (repair == null)
            {
                result.Message = "Repair not found, or it does not belong to the user, or it is already inactive.";
                return result;
            }

            // Ελέγχουμε αν η επισκευή είναι ήδη ανενεργή
            if (!repair.IsActive)
            {
                result.Message = "The repair is already inactive.";
                return result;
            }

            // Ενημερώνουμε την επισκευή και την κάνουμε ανενεργή (soft delete)
            repair.IsActive = false;

            // Ενημέρωση της επισκευής στη βάση δεδομένων
            _dataStore.PropertyRepairs.Update(repair);
            await _dataStore.SaveChangesAsync();

            result.Status = 0;
            result.Message = "Repair was successfully deactivated.";
            result.Value = repair;

            return result;
        }
        catch (Exception ex)
        {
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

            bool isUpdated = false;

            if (!string.IsNullOrEmpty(updatePropertyRepair.Address) && updatePropertyRepair.Address != repair.Address)
            {
                repair.Address = updatePropertyRepair.Address;
                isUpdated = true;
            }

            if (updatePropertyRepair.TypeOfRepair.HasValue && updatePropertyRepair.TypeOfRepair != repair.TypeOfRepair)
            {
                repair.TypeOfRepair = updatePropertyRepair.TypeOfRepair.Value;
                isUpdated = true;
            }

            if (updatePropertyRepair.Cost.HasValue && updatePropertyRepair.Cost != repair.Cost)
            {
                repair.Cost = updatePropertyRepair.Cost.Value;
                isUpdated = true;
            }

            if (updatePropertyRepair.RepairStatus.HasValue && updatePropertyRepair.RepairStatus != repair.RepairStatus)
            {
                repair.RepairStatus = updatePropertyRepair.RepairStatus.Value;
                isUpdated = true;
            }

            if (updatePropertyRepair.IsActive.HasValue && updatePropertyRepair.IsActive != repair.IsActive)
            {
                repair.IsActive = updatePropertyRepair.IsActive.Value;
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