using Microsoft.AspNetCore.Mvc;
using Technico.Api.Services;
using TechnicoRMP.Database.DataAccess;
using TechnicoRMP.Models;
using TechnicoRMP.Shared.Common;
using TechnicoRMP.Shared.Dtos;

namespace Technico.Api.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class PropertyItemController : ControllerBase
    {
        private readonly DataStore _datastore;

        private IPropertyItemService _propertyItemService;
        private IDisplayService<DisplayItemService> _displayItemService;

        public PropertyItemController(IPropertyItemService propertyItemService)
        {
            _propertyItemService = propertyItemService;
        }

        [HttpPost]
        public async Task<Result> Create(CreatePropertyItemRequest createPropertyItemRequest)
        {
            var response = _propertyItemService.Create(createPropertyItemRequest);

            if (response.Status < 0)
            {
                return new Result
                {
                    Status = response.Status,
                    Message = response.Message,
                };
            }
            return new Result
            {
                Status = response.Status,
                Message = response.Message,
            };
        }

        [HttpGet]
        public List<PropertyItem> GetPropertyItems()
        {
            return _propertyItemService.ReadPropertyItems();
        }

        [HttpPut]
        public async Task<Result> Update(UpdatePropertyItemRequest updatePropertyItemRequest)
        {

            var response = _propertyItemService.Update(updatePropertyItemRequest);
            if (response.Status < 0)
            {
                return new Result
                {
                    Status = response.Status,
                    Message = response.Message,
                };
            }
            return new Result
            {
                Status = response.Status,
                Message = response.Message,
            };
        }

        [HttpDelete, Route("{id}")]
        public async Task<Result> Delete(int id)
        {
            var response = _propertyItemService.Delete(id);
            if (response)
            {
                return new Result
                {
                    Status = 0,
                    Message = "Success",
                };
            }
            return new Result
            {
                Status = -1,
                Message = "No Delete",

            };
        }

        //[HttpPost, Route("{id}")]
        [HttpPost]
        public async Task<IActionResult> CreatePropertyItemByUserId(CreatePropertyItemRequest createPropertyItemRequest)
        {
            try
            {
                var response = await _propertyItemService.CreatePropertyItemByUserId(createPropertyItemRequest);

                if (response.Status < 0)
                {
                    return BadRequest(new Result
                    {
                        Status = response.Status,
                        Message = response.Message
                    });
                }
                return Ok(new Result
                {
                    Status = response.Status,
                    Message = response.Message,
                });
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
    
    [HttpGet("{id}")]
        public async Task<ActionResult<Result<CreatePropertyItemResponse>>> GetPropertyItemById(int id)
        {
            try
            {
                var repairsResult = await _propertyItemService.GetById(id);

                if (repairsResult.Status != 0)
                {
                    if (repairsResult.Status == -1)
                    {
                        return NotFound(new Result<CreatePropertyItemResponse>
                        {
                            Status = repairsResult.Status,
                            Message = repairsResult.Message = $"Item not found with ID {id}"
                        });
                    }
                    return StatusCode(500, new Result<CreatePropertyItemResponse>
                    {
                        Status = repairsResult.Status,
                        Message = repairsResult.Message = "An unexpected error occurred."
                    });
                }
                return Ok(repairsResult);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new Result<CreatePropertyItemResponse>
                {
                    Status = 500,
                    Message = $"Internal server error: {ex.Message}"
                });
            }
        }

        [HttpGet("{userId}")]
        public async Task<IActionResult> GetPropertyItemByUserId(int userId)
        {
            var response = await _propertyItemService.GetPropertyItemByUserId(userId);
            return Ok(new Result<PropertyItemsByUserDto>
            {
                Status = response.Status,
                Message = response.Message,
                Value = response.Value
            }); 
        }
    }
}
