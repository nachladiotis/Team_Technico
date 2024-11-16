using Microsoft.AspNetCore.Mvc;
using Technico.Api.Services;
using TechnicoRMP.Database.DataAccess;
using TechnicoRMP.Models;
using TechnicoRMP.Shared.Common;
using TechnicoRMP.Shared.Dtos;
using Technico.Api;

namespace Technico.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class RepairController : ControllerBase
{

    private readonly DataStore _datastore;
    private readonly IPropertyRepairService _propertyRepairService;

    public RepairController(IPropertyRepairService propertyRepairService, DataStore datastore)
    {
        _propertyRepairService = propertyRepairService;
        _datastore = datastore;
    }

    [HttpPost]
    public async Task<ActionResult<PropertyRepairResponseDTO>> Create([FromBody] CreatePropertyRepairRequest createPropertyRepairRequest)
    {
        try
        {
            var propertyRepairResult = await _propertyRepairService.AddRepair(createPropertyRepairRequest);

            if (propertyRepairResult.Status != 0)
            {
                return BadRequest(propertyRepairResult);
            }

            return Ok(propertyRepairResult.Value);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new Result<PropertyRepairResponseDTO>
            {
                Status = 500,
                Message = $"An error occurred: {ex.Message}"
            });
        }
    }


    [HttpGet]
    public async Task<ActionResult<List<PropertyRepairResponseDTO>>> GetAllRepairs()
    {
        var repairsResult = await _propertyRepairService.GetAll();
        return Ok(repairsResult);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Result<PropertyRepairResponseDTO>>> GetRepairById([FromRoute]int id)
    {
        try
        {
            var repairsResult = await _propertyRepairService.GetById(id);

            if (repairsResult.Status != 0)
            {
                if (repairsResult.Status == -1)
                {
                    return NotFound(new Result<PropertyRepairResponseDTO>
                    {
                        Status = repairsResult.Status,
                        Message = repairsResult.Message = $"Repair not found with ID {id}"
                    });
                }
                return StatusCode(500, new Result<PropertyRepairResponseDTO>
                {
                    Status = repairsResult.Status,
                    Message = repairsResult.Message = "An unexpected error occurred."
                });
            }

            return Ok(repairsResult);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new Result<PropertyRepairResponseDTO>
            {
                Status = 500,
                Message = $"Internal server error: {ex.Message}"
            });
        }
    }

    [HttpPatch("{id}")]
    public async Task<ActionResult<Result>> Update(int id, [FromBody] UpdatePropertyRepair updatePropertyRepair)
    {
        try
        {
            if (updatePropertyRepair.Id != id)
            {
                return BadRequest("The ID in the URL does not match the ID in the body.");
            }
            var result = await _propertyRepairService.Update(updatePropertyRepair);

            if (result.Status == 0)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new Result
            {
                Status = 500,
                Message = $"Internal server error: {ex.Message}"
            });
        }
    }

    [HttpPut("deactivate/{repairId}")]
    public async Task<ActionResult<Result<PropertyRepair>>> SoftDeleteRepairForUser(int userId, int repairId)
    {
        try
        {
            var result = await _propertyRepairService.SoftDeleteRepairForUser(userId, repairId);

            if (result.Status == 0)
            {
                return NoContent();
            }

            return NotFound(result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new Result<PropertyRepair>
            {
                Status = 500,
                Message = $"An error occurred during the operation: {ex.Message}"
            });
        }
    }
}




