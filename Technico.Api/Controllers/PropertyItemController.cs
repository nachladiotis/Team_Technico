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
    public class PropertyItemController : ControllerBase
    {
        private readonly DataStore _datastore;

        private IPropertyItemService _propertyItemService;
        private IDisplayService<DisplayItemService> _displayItemService;

        public PropertyItemController(IPropertyItemService propertyItemService)
        {
            _propertyItemService = propertyItemService;
        }

        [HttpPost("propertyItem")]
        public async Task<Result> Create(CreatePropertyItemRequest createPropertyItemRequest)
        {
            var response =  _propertyItemService.Create(createPropertyItemRequest);

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


        [HttpGet("items")]
        public List<PropertyItem> GetPropertyItems()
        {
            return _propertyItemService.ReadPropertyItems();
        }

        [HttpPatch("propertyItem")]
        public async Task<Result> Update(UpdatePropertyItemRequest updatePropertyItemRequest)
        {

            //var response = await _propertyItemService.CreateAsync(createPropertyItemRequest);
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

        [HttpDelete, Route("{id}")] //api/item/1
        public async Task<Result> Delete(string id)
        {

            //var response = await _propertyItemService.CreateAsync(createPropertyItemRequest);
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




    }
}
