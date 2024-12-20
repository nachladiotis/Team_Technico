﻿using Microsoft.AspNetCore.Mvc;
using Technico.Api.Services;
using TechnicoRMP.Database.DataAccess;
using TechnicoRMP.Models;
using TechnicoRMP.Shared.Common;
using TechnicoRMP.Shared.Dtos;
using Microsoft.EntityFrameworkCore;

namespace Technico.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class RepairController(IPropertyRepairService propertyRepairService, DataStore datastore) : ControllerBase
{
    private readonly DataStore _datastore = datastore;
    private readonly IPropertyRepairService _propertyRepairService = propertyRepairService;

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
    public async Task<ActionResult<List<PropertyRepairResponseDTO>>> GetAllRepairs(DateTime? startDate, DateTime? endDate)
    {
        var repairsResult = await _propertyRepairService.GetByDateRange(startDate, endDate);
        return Ok(repairsResult);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Result<PropertyRepairResponseDTO>>> GetRepairById([FromRoute] int id)
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

    [HttpGet("user/{userId}")]
    public async Task<IActionResult> GetRepairsByUserId(long userId)
    {
        if (userId <= 0)
        {
            return BadRequest(new { Message = "Invalid UserId provided." });
        }

        var userExists = await _datastore.Users
            .AnyAsync(u => u.Id == userId);

        if (!userExists)
        {
            return NotFound(new { Message = "User not found." });
        }

        var result = await _propertyRepairService.GetByUserId(userId);

        if (result.Status == -1)
        {
            return BadRequest(new { Message = result.Message });
        }

        return Ok(result.Value);
    }

    [HttpPatch("{id}")]
    public async Task<ActionResult<Result>> Update(int id, [FromBody] UpdatePropertyRepair updatePropertyRepair)
    {
        try
        {
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

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _propertyRepairService.Delete(id);
        if (result)
            return NoContent();
        return Problem();
    }

    [HttpPut("deactivate/{repairId}")]
    public async Task<ActionResult<Result<PropertyRepair>>> SoftDeleteRepairForUser(int repairId)
    {
        try
        {
            var result = await _propertyRepairService.SoftDeleteRepairForUser(repairId);

            if (result.Status == 0)
            {
                return Ok();
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




