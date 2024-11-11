using TechnicoRMP.Models;
using TechnicoRMP.Shared.Common;
using TechnicoRMP.Shared.Dtos;

namespace Technico.Api.Services;

public interface IPropertyRepairService
{
    public Task<Result<List<CreatePropertyRepairResponse>>> GetAll();
    public Task<Result<CreatePropertyRepairResponse>> GetById(int id);
    public Task<Result> Update(UpdatePropertyRepair updatePropertyRepair);
    public Task<Result<PropertyRepair>> SoftDeleteRepairForUser(int userId, int repairId);
    public Task<Result<CreatePropertyRepairResponse>> AddRepair(CreatePropertyRepairRequest createPropertyRepairRequest);
}
