using AutoMapper;
using CollegeApp.Data;
using CollegeApp.Data.Repo;
using CollegeApp.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace CollegeApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ICollegeRepository<Role> _roleRepository;
        private readonly ILogger<RoleController> _logger;
        private APIResponse _apiResponse;

        public RoleController(IMapper mapper, ICollegeRepository<Role> roleRepository, ILogger<RoleController> logger)
        {
            _mapper = mapper;
            _roleRepository = roleRepository;
            _logger = logger;
            _apiResponse = new APIResponse();
        }

        [HttpPost]
        [Route("Create")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<APIResponse>> CreateRole([FromBody] RoleDTO dto)
        {
            try
            {
                if (dto == null)
                    return BadRequest(new APIResponse { Status = false, StatusCode = HttpStatusCode.BadRequest });

                Role role = _mapper.Map<Role>(dto);
                role.IsDeleted = false;
                role.CreatedDate = DateTime.UtcNow;
                role.ModifiedDate = DateTime.UtcNow;

                var result = await _roleRepository.CreateAsync(role);
                dto.Id = result.Id;

                _apiResponse.data = dto;
                _apiResponse.Status = true;
                _apiResponse.StatusCode = HttpStatusCode.Created;

                return CreatedAtRoute("GetRoleById", new { id = dto.Id }, _apiResponse);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating role");
                _apiResponse.Status = false;
                _apiResponse.StatusCode = HttpStatusCode.InternalServerError;
                _apiResponse.Errors.Add(ex.Message);
                return StatusCode((int)HttpStatusCode.InternalServerError, _apiResponse);
            }
        }

        [HttpGet]
        [Route("All", Name = "GetAllRoles")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<APIResponse>> GetRolesAsync()
        {
            try
            {
                var roles = await _roleRepository.GetAllAsync();
                _apiResponse.data = _mapper.Map<List<RoleDTO>>(roles);
                _apiResponse.Status = true;
                _apiResponse.StatusCode = HttpStatusCode.OK;
                return Ok(_apiResponse);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving all roles");
                _apiResponse.Status = false;
                _apiResponse.StatusCode = HttpStatusCode.InternalServerError;
                _apiResponse.Errors.Add(ex.Message);
                return StatusCode((int)HttpStatusCode.InternalServerError, _apiResponse);
            }
        }

        [HttpGet]
        [Route("{id:int}", Name = "GetRoleById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<APIResponse>> GetRoleByIdAsync(int id)
        {
            try
            {
                if (id <= 0)
                    return BadRequest(new APIResponse { Status = false, StatusCode = HttpStatusCode.BadRequest, Errors = new List<string> { "Invalid ID." } });

                var role = await _roleRepository.GetAsync(role => role.Id == id);

                if (role == null)
                    return NotFound(new APIResponse { Status = false, StatusCode = HttpStatusCode.NotFound, Errors = new List<string> { $"Role not found with ID: {id}" } });

                _apiResponse.data = _mapper.Map<RoleDTO>(role);
                _apiResponse.Status = true;
                _apiResponse.StatusCode = HttpStatusCode.OK;

                return Ok(_apiResponse);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving role by ID");
                _apiResponse.Status = false;
                _apiResponse.StatusCode = HttpStatusCode.InternalServerError;
                _apiResponse.Errors.Add(ex.Message);
                return StatusCode((int)HttpStatusCode.InternalServerError, _apiResponse);
            }
        }

        [HttpGet]
        [Route("ByName/{name}", Name = "GetRoleByName")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<APIResponse>> GetRoleByNameAsync(string name)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(name))
                    return BadRequest(new APIResponse { Status = false, StatusCode = HttpStatusCode.BadRequest, Errors = new List<string> { "Role name cannot be empty." } });

                var role = await _roleRepository.GetAsync(r => r.RoleName == name);

                if (role == null)
                    return NotFound(new APIResponse { Status = false, StatusCode = HttpStatusCode.NotFound, Errors = new List<string> { $"Role not found with name: {name}" } });

                _apiResponse.data = _mapper.Map<RoleDTO>(role);
                _apiResponse.Status = true;
                _apiResponse.StatusCode = HttpStatusCode.OK;

                return Ok(_apiResponse);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving role by name");
                _apiResponse.Status = false;
                _apiResponse.StatusCode = HttpStatusCode.InternalServerError;
                _apiResponse.Errors.Add(ex.Message);
                return StatusCode((int)HttpStatusCode.InternalServerError, _apiResponse);
            }
        }

        [HttpPut]
        [Route("Update")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

        public async Task<ActionResult<APIResponse>> UpdateRole(RoleDTO dto)
        {
            try
            {
                if (dto == null || dto.Id <= 0)
                    return BadRequest();

                var existingRole=_roleRepository.GetAsync(role => role.Id == dto.Id,true);

                if (existingRole == null)
                    return BadRequest($"Role not found with id:{dto.Id} to update");

                var newRole = _mapper.Map<Role>(dto);

                await _roleRepository.UpdateAsync(newRole);

                _apiResponse.Status = true;
                _apiResponse.StatusCode=HttpStatusCode.OK;
                _apiResponse.data = newRole;

                return Ok(_apiResponse);

            }
            catch(Exception ex)
            {
                _apiResponse.Status = false;
                _apiResponse.StatusCode = HttpStatusCode.InternalServerError;
                _apiResponse.Errors.Add(ex.Message);
                return _apiResponse;
                
            }

        }

        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<APIResponse>> DeleteRoleAsync(int id)
        {
            try
            {
                if(id <= 0)
                    return BadRequest();

                var role = await _roleRepository.GetAsync(role => role.Id == id);

                if (role == null)
                    return BadRequest($"Role not found with id :{id} to delete");

                await _roleRepository.DeleteAsync(role);
                

                _apiResponse.Status = true;
                _apiResponse.StatusCode = HttpStatusCode.OK;
                _apiResponse.data = true;

                return Ok(_apiResponse);

            }
            catch (Exception ex)
            {
                _apiResponse.Status = false;
                _apiResponse.StatusCode = HttpStatusCode.InternalServerError;
                _apiResponse.Errors.Add(ex.Message);
                return _apiResponse;

            }

        }
    }
}



