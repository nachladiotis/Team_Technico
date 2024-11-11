﻿using TechnicoRMP.DatabaseNew.DataAccess;
using TechnicoRMP.Models;
using TechnicoRMP.Shared.Common;
using TechnicoRMP.Shared.Dtos;

namespace Technico.Api.Services;

public class PropertyRepairService(DataStore dataStore) : IPropertyRepairService
{
    private readonly DataStore _dataStore = dataStore;

    public Task<Result<CreatePropertyRepairResponse>> AddRepair(CreatePropertyRepairRequest createPropertyRepairRequest)
    {
        throw new NotImplementedException();
    }

    public Result<CreatePropertyRepairResponse> Create(CreatePropertyRepairRequest createPropertyRepairRequest)
    {
        Console.WriteLine("ΕΙΣΑΓΕΤΕ ΤΑ ΣΤΟΙΧΕΙΑ ΔΗΛΩΣΗΣ:");
        var response = new Result<CreatePropertyRepairResponse>()
        {
            Status = -1
        };



        var propertyOwner = _dataStore.Users
          .FirstOrDefault(p => p.Id == createPropertyRepairRequest.RepairerId);
        if (propertyOwner == null)
        {
            response.Message = "ΔΕΝ ΒΡΕΘΗΚΕ ΧΡΗΣΤΗΣ ΜΕ ΑΥΤΟ ΤΟ ID";
            return response;
        }

        var propertyRepairToStore = new PropertyRepair
        {
            Date = DateTime.UtcNow,
            Address = createPropertyRepairRequest.Address,
            TypeOfRepair = createPropertyRepairRequest.TypeOfRepair,
            Cost = createPropertyRepairRequest.Cost,
            RepairerId = createPropertyRepairRequest.RepairerId,
        };
        _dataStore.Add(propertyRepairToStore);
        _dataStore.SaveChanges();

        response.Status = 0;
        response.Value = CreatePropertyRepairResponseService.CreateFromEntity(propertyRepairToStore);
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


        if (propertyRepairFromDb == null)
        {
            Console.WriteLine("ΔΕΝ ΒΡΕΘΗΚΕ ΤΟ PROPERTYREPAIR ΜΕ ΤΟ ID ΠΟΥ ΕΔΩΣΕΣ");
            return false;
        }
        _dataStore.Remove(id);
        _dataStore.SaveChanges();
        return true;

    }

    public Task<Result<List<CreatePropertyRepairResponse>>> GetAll()
    {
        throw new NotImplementedException();
    }

    public Task<Result<CreatePropertyRepairResponse>> GetById(int id)
    {
        throw new NotImplementedException();
    }

    public Task<Result<PropertyRepair>> SoftDeleteRepairForUser(int userId, int repairId)
    {
        throw new NotImplementedException();
    }

    public Result Update(UpdatePropertyRepair updatePropertyRepair)
    {
        var response = new Result()
        {
            Status = -1
        };

        //if (propertyRepair == null)
        //{
        //    response.Message = "ΠΡΕΠΕΙ ΝΑ ΔΩΣΕΙΣ ΣΤΟΙΧΕΙΑ";
        //    return response;
        //}
        //if (propertyRepair.Id == 0 || propertyRepair.Id <0)
        //{
        //    response.Message = "ΤΟ ΠΕΔΙΟ ID ΣΤΟ PROPERTYREPAIR ΕΙΝΑΙ ΥΠΟΧΡΕΩΤΙΚΟ ΚΑΙ ΜΕΓΑΛΥΤΕΡΟ ΑΠΟ ΤΟ 0";
        //    return response;
        //}
        //var propertyRepairFromDb =_dataStore
        //    .PropertyRepairs
        //    .FirstOrDefault(p => p.Id == propertyRepair.Id);
        //if (propertyRepairFromDb == null)
        //{
        //    response.Message = "ΔΕΝ ΒΡΕΘΗΚΕ PORPERTYREPAIR ΜΕ ΤΟ ID ΤΟΥ PROPERTYREPAIR ID ΠΟΥ ΕΔΩΣΕΣ";
        //    return response;
        //}

        var entity = _dataStore.PropertyRepairs.FirstOrDefault(p => p.Id == updatePropertyRepair.Id);
        var repairer = _dataStore.Users.FirstOrDefault(p => p.Id == updatePropertyRepair.RepairerId);
        if (repairer == null)
        {
            return response;
        }
        if (entity == null)
        {
            return response;
        }
        if (!string.IsNullOrEmpty(updatePropertyRepair.Address))
        {
            entity.Address = updatePropertyRepair.Address;
        }

        entity.RepairerId = (long)updatePropertyRepair.RepairerId!;

        if (updatePropertyRepair.TypeOfRepair is not null)
        {
            entity.TypeOfRepair = updatePropertyRepair.TypeOfRepair.Value;
        }
        if (updatePropertyRepair.Cost is not null)
        {
            entity.Cost = updatePropertyRepair.Cost.Value;
        }
        if (updatePropertyRepair.IsActive is not null)
        {
            entity.IsActive = updatePropertyRepair.IsActive.Value;
        }
        if (updatePropertyRepair.RepairStatus is not null)
        {
            entity.RepairStatus = updatePropertyRepair.RepairStatus.Value;
        }

        _dataStore.SaveChanges();

        response.Message = "ΕΠΙΤΥΧΕΣ";
        response.Status = 0;
        return response;

    }

    Task<Result> IPropertyRepairService.Update(UpdatePropertyRepair updatePropertyRepair)
    {
        throw new NotImplementedException();
    }
}
