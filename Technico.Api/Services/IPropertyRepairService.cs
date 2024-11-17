using TechnicoRMP.Models;
using TechnicoRMP.Shared.Common;
using TechnicoRMP.Shared.Dtos;

namespace Technico.Api.Services;

public interface IPropertyRepairService
{
    public Task<List<PropertyRepairResponseDTO>> GetAll();
    public Task<List<PropertyRepairResponseDTO>> GetByDateRange(DateTime? startDate, DateTime? endDate);
    public Task<Result<PropertyRepairResponseDTO>> GetById(long id);
    public Task<Result<List<PropertyRepairResponseDTO>>> GetByUserId(long userId);
    public Task<Result> Update(UpdatePropertyRepair updatePropertyRepair);
    public Task<Result<PropertyRepair>> SoftDeleteRepairForUser( int repairId);
    public Task<bool> Delete( int repairId);
    public Task<Result<PropertyRepairResponseDTO>> AddRepair(CreatePropertyRepairRequest createPropertyRepairRequest);
}
