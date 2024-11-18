using Microsoft.AspNetCore.Mvc;
using Technico.Api.Services;
using TechnicoRMP.Shared.Common;
using TechnicoRMP.Shared.Dtos;
using TechnicoRMP.Shared.Exceptions;

namespace Technico.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController(IUserService service, ILogger<UserController> logger) : ControllerBase
    {
        private readonly IUserService _service = service;

        private readonly ILogger<UserController> _logger = logger;


        [HttpPost]
        [Route("Create")]
        public async Task<ActionResult<UserDto>> Create([FromBody] CreateUserRequest createUserRequest)
        {
            try
            {
                var createUserResult =  await _service.Create(createUserRequest);

                if (createUserResult.Status != 0)
                {
                    return BadRequest(createUserResult);
                }

                return Ok(createUserResult);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new Result<UserDto>
                {
                    Status = 500,
                    Message = $"An error occurred: {ex.Message}"
                });
            }
        }


        [HttpGet]
        public async Task<ActionResult<List<UserDto>>> Get()
        {
            var response = await _service.DisplayAll();
            return Ok(response);
        }

        [HttpGet, Route("{id}")]
        public async Task<ActionResult<UserDto>> GetUserDetails(int id)
        {
            var response = await _service.DisplayUser(id);
            return Ok(response);
        }

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
                _logger.LogError(ex,ex.Message);
                return BadRequest(ex.Message);
            }
            catch (NotFoundException ex)
            {
                _logger.LogInformation(ex, ex.Message);
                return BadRequest(ex.Message);
            }
        }

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
                _logger.LogError(ex, ex.Message);
                return BadRequest(ex.Message);
            }
            catch (NotFoundException ex)
            {
                _logger.LogInformation(ex, ex.Message);
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete, Route("{id}")]
        public async Task<ActionResult<bool>> DeleteAsync([FromRoute] int id)
        {
            bool response = await _service.Delete(id);

            if (response) return Ok(response);
            else return NotFound(response);
        }

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
                _logger.LogError(ex, ex.Message);
                return NotFound(new { message = ex.Message });
            }
        }   
    }
}
