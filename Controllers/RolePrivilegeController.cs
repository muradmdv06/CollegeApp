using AutoMapper;
using CollegeApp.Data.Repo;
using CollegeApp.Data;
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
    public class RolePrivilegeController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ICollegeRepository<RolePrivilege> _rolePrivilegeRepository;
        private readonly ILogger<RolePrivilegeController> _logger;
        

        public RolePrivilegeController(IMapper mapper, ICollegeRepository<RolePrivilege> rolePrivilegeRepository, ILogger<RolePrivilegeController> logger)
        {
            _mapper = mapper;
            _rolePrivilegeRepository = rolePrivilegeRepository;
            _logger = logger;
        }

        
        [HttpPost("create")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<APIResponse>> CreateRolePrivilegeAsync([FromBody] RolePrivilegeDTO dto)
        {
            if (dto == null)
            {
                _logger.LogWarning("CreateRolePrivilegeAsync received a null DTO");
                return BadRequest(new APIResponse { Status = false, StatusCode = HttpStatusCode.BadRequest, Errors = new List<string> { "Invalid request data." } });
            }

            try
            {
                var rolePrivilege = _mapper.Map<RolePrivilege>(dto);
                rolePrivilege.IsDeleted = false;
                rolePrivilege.CreatedDate = DateTime.UtcNow;
                rolePrivilege.ModifiedDate = DateTime.UtcNow;

                var result = await _rolePrivilegeRepository.CreateAsync(rolePrivilege);
                dto.Id = result.Id;

                var response = new APIResponse { Status = true, StatusCode = HttpStatusCode.Created, data = dto };
                _logger.LogInformation("Role privilege created successfully with ID {Id}", dto.Id);

                return CreatedAtRoute("GetRolePrivilegeById", new { id = dto.Id }, response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating role privilege");
                return StatusCode(StatusCodes.Status500InternalServerError, new APIResponse
                {
                    Status = false,
                    StatusCode = HttpStatusCode.InternalServerError,
                    Errors = new List<string> { ex.Message }
                });
            }
        }

        
        [HttpGet("all")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<APIResponse>> GetRolePrivilegesAsync()
        {
            try
            {
                var roles = await _rolePrivilegeRepository.GetAllAsync();
                var response = new APIResponse
                {
                    Status = true,
                    StatusCode = HttpStatusCode.OK,
                    data = _mapper.Map<List<RolePrivilegeDTO>>(roles)
                };
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving all role privileges");
                return StatusCode(StatusCodes.Status500InternalServerError, new APIResponse
                {
                    Status = false,
                    StatusCode = HttpStatusCode.InternalServerError,
                    Errors = new List<string> { ex.Message }
                });
            }
        }

        
        [HttpGet("{id:int}", Name = "GetRolePrivilegeById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<APIResponse>> GetRolePrivilegeByIdAsync(int id)
        {
            if (id <= 0)
            {
                _logger.LogWarning("GetRolePrivilegeByIdAsync received invalid ID: {Id}", id);
                return BadRequest(new APIResponse { Status = false, StatusCode = HttpStatusCode.BadRequest, Errors = new List<string> { "Invalid ID." } });
            }

            try
            {
                var role = await _rolePrivilegeRepository.GetAsync(r => r.Id == id);
                if (role == null)
                {
                    return NotFound(new APIResponse { Status = false, StatusCode = HttpStatusCode.NotFound, Errors = new List<string> { $"Role privilege not found with ID: {id}" } });
                }

                var response = new APIResponse
                {
                    Status = true,
                    StatusCode = HttpStatusCode.OK,
                    data = _mapper.Map<RolePrivilegeDTO>(role)
                };
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving role privilege by ID");
                return StatusCode(StatusCodes.Status500InternalServerError, new APIResponse
                {
                    Status = false,
                    StatusCode = HttpStatusCode.InternalServerError,
                    Errors = new List<string> { ex.Message }
                });
            }
        }

        
        [HttpGet("byname/{name}", Name = "GetRolePrivilegeByName")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<APIResponse>> GetRolePrivilegeByNameAsync(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return BadRequest(new APIResponse { Status = false, StatusCode = HttpStatusCode.BadRequest, Errors = new List<string> { "Role privilege name cannot be empty." } });
            }

            try
            {
                var role = await _rolePrivilegeRepository.GetAsync(r => r.RolePrivilegeName == name);
                if (role == null)
                {
                    return NotFound(new APIResponse { Status = false, StatusCode = HttpStatusCode.NotFound, Errors = new List<string> { $"Role privilege not found with name: {name}" } });
                }

                var response = new APIResponse
                {
                    Status = true,
                    StatusCode = HttpStatusCode.OK,
                    data = _mapper.Map<RolePrivilegeDTO>(role)
                };
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving role privilege by name");
                return StatusCode(StatusCodes.Status500InternalServerError, new APIResponse
                {
                    Status = false,
                    StatusCode = HttpStatusCode.InternalServerError,
                    Errors = new List<string> { ex.Message }
                });
            }
        }

        
        [HttpPut("update")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<APIResponse>> UpdateRole(RolePrivilegeDTO dto)
        {
            if (dto == null || dto.Id <= 0)
            {
                return BadRequest(new APIResponse { Status = false, StatusCode = HttpStatusCode.BadRequest, Errors = new List<string> { "Invalid role privilege data." } });
            }

            try
            {
                var existingRole = await _rolePrivilegeRepository.GetAsync(role => role.Id == dto.Id, true);

                if (existingRole == null)
                {
                    return NotFound(new APIResponse { Status = false, StatusCode = HttpStatusCode.NotFound, Errors = new List<string> { $"RolePrivilege not found with id: {dto.Id}" } });
                }

                var updatedRolePrivilege = _mapper.Map<RolePrivilege>(dto);
                await _rolePrivilegeRepository.UpdateAsync(updatedRolePrivilege);

                var response = new APIResponse
                {
                    Status = true,
                    StatusCode = HttpStatusCode.OK,
                    data = updatedRolePrivilege
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating role privilege");
                return StatusCode(StatusCodes.Status500InternalServerError, new APIResponse
                {
                    Status = false,
                    StatusCode = HttpStatusCode.InternalServerError,
                    Errors = new List<string> { ex.Message }
                });
            }
        }

        

        [HttpDelete("delete/{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<APIResponse>> DeleteRoleAsync(int id)
        {
            var response = new APIResponse();

            try
            {
                if (id <= 0)
                {
                    _logger.LogWarning("DeleteRoleAsync received an invalid ID: {Id}", id);
                    response.Status = false;
                    response.StatusCode = HttpStatusCode.BadRequest;
                    response.Errors.Add("Invalid ID.");
                    return BadRequest(response);
                }

                var role = await _rolePrivilegeRepository.GetAsync(r => r.Id == id);
                if (role == null)
                {
                    _logger.LogWarning("RolePrivilege not found with ID: {Id}", id);
                    response.Status = false;
                    response.StatusCode = HttpStatusCode.NotFound;
                    response.Errors.Add($"Role not found with ID: {id}");
                    return NotFound(response);
                }

                await _rolePrivilegeRepository.DeleteAsync(role);

                response.Status = true;
                response.StatusCode = HttpStatusCode.OK;
                response.data = true;

                _logger.LogInformation("RolePrivilege with ID {Id} deleted successfully", id);
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting RolePrivilege with ID: {Id}", id);
                response.Status = false;
                response.StatusCode = HttpStatusCode.InternalServerError;
                response.Errors.Add(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }
        }



    }
}



