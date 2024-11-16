﻿using TechnicoRMP.Models;
using TechnicoRMP.Shared.Common;
using TechnicoRMP.Shared.Dtos;

namespace Technico.Api.Services;

public interface IPropertyRepairService
{
    public Task<List<PropertyRepairResponseDTO>> GetAll();
    public Task<Result<PropertyRepairResponseDTO>> GetById(long id);
    public Task<Result> Update(UpdatePropertyRepair updatePropertyRepair);
    public Task<Result<PropertyRepair>> SoftDeleteRepairForUser(int userId, int repairId);
    public Task<Result<PropertyRepairResponseDTO>> AddRepair(CreatePropertyRepairRequest createPropertyRepairRequest);
}
