﻿using Microsoft.AspNetCore.Mvc;
using Technico.Api.Services;
using TechnicoRMP.Models;
using TechnicoRMP.Shared.Dtos;
using TechnicoRMP.Shared.Exceptions;

namespace Technico.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private static List<User> users = new List<User>();
        private readonly ILogger<UserController> _logger;
        private IUserService _service;
        public UserController(IUserService service)
        {
            _service = service;
        }

        [HttpGet]
        public ActionResult<List<CreateUserResponse>> Get()
        {
            var response = _service.DisplayAll();
            return Ok(response);
        }

        //[HttpGet, Route("withpropertyitems")]
        //public ActionResult<List<UserWithProperyItemsDTO>> GetWithPropertyItems()
        //{
        //    var response = _service.GetAllUsersWithPropertyItems();
        //    return Ok(response);
        //}

        [HttpGet, Route("{id}")]
        public ActionResult<CreateUserResponse> GetUserDetails(int id)
        {
            var response = _service.DisplayUser(id);
            return Ok(response);
        }

        //[HttpPut]
        //public ActionResult<CreateUserResponse> Put([FromBody] CreateUserResponse userDTO)
        //{
        //    try
        //    {
        //        var response = _service.ReplaceUser(userDTO);
        //        return Ok(response);
        //    }
        //    catch (BadRequestException ex)
        //    {
        //        return BadRequest(ex.Message);
        //    }
        //    catch (NotFoundException ex)
        //    {
        //        return BadRequest(ex.Message);
        //    }
        //}

        //[HttpPatch]
        //public ActionResult<UpdateUserRequest> Patch([FromBody] UpdateUserRequest userDTO)
        //{
        //    try
        //    {
        //        var response = _service.UpdateUser(userDTO);
        //        return Ok(response);
        //    }
        //    catch (BadHttpRequestException ex)
        //    {
        //        return BadRequest(ex.Message);
        //    }
        //    catch (NotFoundException ex)
        //    {
        //        return BadRequest(ex.Message);
        //    }
        //}

        [HttpDelete, Route("{vat}")]
        public ActionResult<bool> Delete([FromRoute] string vat)
        {
            bool response = _service.Delete(vat);

            if (response) return Ok(response);
            else return NotFound(response);
        }

    }

}
