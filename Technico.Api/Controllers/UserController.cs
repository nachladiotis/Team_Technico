using Microsoft.AspNetCore.Mvc;
using Technico.Api.Services;
using TechnicoRMP.Models;
using TechnicoRMP.Shared.Dtos;
using TechnicoRMP.Shared.Exceptions;

namespace Technico.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private static List<User> users = new List<User>();

        private readonly ILogger<UserController> _logger;

        private IUserService _service;

        public UserController(IUserService service)
        {
            _service = service;
        }

        //Read all users
        [HttpGet]
        public async Task<ActionResult<List<UserDto>>> Get()
        {
            var response = await _service.DisplayAll();
            return Ok(response);
        }


        //[HttpGet, Route("withpropertyitems")]
        //public ActionResult<List<UserWithProperyItemsDTO>> GetWithPropertyItems()
        //{
        //    var response = _service.GetAllUsersWithPropertyItems();
        //    return Ok(response);
        //}

        //Read User with ID
        [HttpGet, Route("{id}")]
        public async Task<ActionResult<UserDto>> GetUserDetails(int id)
        {
            var response = await _service.DisplayUser(id);
            return Ok(response);
        }


        //Partial Update
        [HttpPut, Route("{id}")]
        public async Task<ActionResult<UserDto>> PutAsync([FromBody] UpdateUserRequest userDTO)
        {
            try
            {
                var response = await _service.ReplaceUser(userDTO);
                return Ok(response);
            }
            catch (BadRequestException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (NotFoundException ex)
            {
                return BadRequest(ex.Message);
            }
        }


        //Complete update
        [HttpPatch]
        public async Task<ActionResult<UpdateUserRequest>> Patch([FromBody] UpdateUserRequest userDTO)
        {
            try
            {
                var response = await _service.UpdateUser(userDTO);
                return Ok(response);
            }
            catch (BadHttpRequestException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (NotFoundException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //Hard Delete
        [HttpDelete, Route("{id}")]
        public async Task<ActionResult<bool>> DeleteAsync([FromRoute] int id)
        {
            bool response = await _service.Delete(id);

            if (response) return Ok(response);
            else return NotFound(response);
        }

        //Soft Delete
        [HttpPost, Route("SoftDelete/{id}")]
        public async Task<ActionResult<bool>> SoftDeleteAsync([FromRoute] int id)
        {
            try
            {
                bool response = await _service.SoftDeleteUser(id);  
                if (response)
                    return Ok(response);
                else
                    return NotFound(response);
            }
            catch (NotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }
    }
}
