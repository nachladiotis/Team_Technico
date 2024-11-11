using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Technico.Api.Services;
using TechnicoRMP.Database.DataAccess;
using TechnicoRMP.Models;
using TechnicoRMP.Shared.Common;
using TechnicoRMP.Shared.Dtos;

namespace Technico.Api.Controllers
{
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
        public async Task<ActionResult<CreatePropertyRepairResponse>> Post([FromBody] CreatePropertyRepairRequest createPropertyRepairRequest)
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
                return StatusCode(500, new Result<CreatePropertyRepairResponse>
                {
                    Status = 500,
                    Message = $"An error occurred: {ex.Message}"
                });
            }
        }

        
        [HttpGet]
        public async Task<ActionResult<Result<List<CreatePropertyRepairResponse>>>> GetAllRepairs()
        {
            try
            {
                var repairsResult = await _propertyRepairService.GetAll();

                if (repairsResult.Status != 0)
                {
                    return StatusCode(500, repairsResult);
                }

                if (repairsResult.Value == null || repairsResult.Value.Count == 0)
                {
                    return NotFound(new Result<List<CreatePropertyRepairResponse>>()
                    {
                        Status = -1,
                        Message = "No repairs found"
                    });
                }

                return Ok(repairsResult);
            }
            catch (Exception ex)
            {
                var result = new Result<List<CreatePropertyRepairResponse>>()
                {
                    Status = 500,
                    Message = $"Internal server error: {ex.Message}"
                };
                return StatusCode(500, result);
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Result<CreatePropertyRepairResponse>>> GetRepairById(int id)
        {
            try
            {
                var repairsResult = await _propertyRepairService.GetById(id);

                if (repairsResult.Status != 0)
                {
                    if (repairsResult.Status == -1)
                    {
                        return NotFound(new Result<CreatePropertyRepairResponse>
                        {
                            Status = repairsResult.Status,
                            Message = repairsResult.Message = $"Repair not found with ID {id}"
                        });
                    }
                    return StatusCode(500, new Result<CreatePropertyRepairResponse>
                    {
                        Status = repairsResult.Status,
                        Message = repairsResult.Message = "An unexpected error occurred."
                    });
                }

                return Ok(repairsResult);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new Result<CreatePropertyRepairResponse>
                {
                    Status = 500,
                    Message = $"Internal server error: {ex.Message}"
                });
            }
        }

        [HttpPut("{id}")]
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

        [HttpPut("{repairId}/deactivate")]
        public async Task<ActionResult<Result<PropertyRepair>>> SoftDeleteRepairForUser(int userId, int repairId)
        {
            try
            {
                var result = await _propertyRepairService.SoftDeleteRepairForUser(userId, repairId);

                if (result.Status == 0)
                {
                    return Ok(result);
                }

                return BadRequest(result);
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

}


