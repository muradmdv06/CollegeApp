using AutoMapper;
using CollegeApp.Data;
using CollegeApp.Data.Repo;
using CollegeApp.Data.Repository;
using CollegeApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace CollegeApp.Controllers
{
    [Route("api/students")]
    [ApiController]
    //[Authorize(Roles ="Superadmin,Admin")]
    public class StudentController : ControllerBase
    {
        private readonly ILogger<StudentController> _logger;
        private readonly IMapper _mapper;
        private readonly ICollegeRepository<Student> _studentRepository;
        private APIResponse _apiResponse;

        public StudentController(ILogger<StudentController> logger, IMapper mapper,
            ICollegeRepository<Student> studentRepository)
        {
            _logger = logger;
            _mapper = mapper;
            _studentRepository = studentRepository;
            _apiResponse= new ();
        }

        [HttpGet]
        //[AllowAnonymous]
        public async Task<ActionResult<IEnumerable<StudentDTO>>> GetStudents()
        {
            _logger.LogInformation("Fetching all students.");
            var students = await _studentRepository.GetAllAsync();

            _apiResponse.data=_mapper.Map<List<StudentDTO>>(students);
            _apiResponse.Status = true;
            _apiResponse.StatusCode=HttpStatusCode.OK;
            return Ok(_apiResponse);
        }

        [HttpGet("{id:int}", Name = "GetStudentById")]
        public async Task<ActionResult<StudentDTO>> GetStudentByIdAsync(int id)
        {
            if (id <= 0) return BadRequest("Invalid student ID.");

            var student = await _studentRepository.GetAsync(student => student.Id == id);
            _apiResponse.data = _mapper.Map<StudentDTO>(student);
            _apiResponse.Status = true;
            _apiResponse.StatusCode = HttpStatusCode.OK;
            if (student == null) return NotFound($"Student with ID {id} not found.");

            return Ok(_apiResponse);
        }

        [HttpGet("name/{name}", Name = "GetStudentByName")]
        public async Task<ActionResult<StudentDTO>> GetStudentByNameAsync(string name)
        {
            if (string.IsNullOrWhiteSpace(name)) return BadRequest("Name cannot be empty.");

            var student = await _studentRepository.GetAsync(student => student.StudentName.ToLower().Contains(name));
            _apiResponse.data = _mapper.Map<StudentDTO>(student);
            _apiResponse.Status = true;
            _apiResponse.StatusCode = HttpStatusCode.OK;
            if (student == null) return NotFound($"Student with name {name} not found.");

            return Ok(_apiResponse);
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult<bool>> DeleteStudentAsync(int id)
        {
            if (id <= 0) return BadRequest("Invalid student ID.");

            var student = await _studentRepository.GetAsync(student => student.Id == id);
            if (student == null) return NotFound($"Student with ID {id} not found.");

            _apiResponse.data = true;
            _apiResponse.Status = true;
            _apiResponse.StatusCode = HttpStatusCode.OK;


            return Ok(_apiResponse);
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult<bool>> EditStudentAsync(int id, [FromBody] StudentDTO updatedStudentDto)
        {
            if (id <= 0 || updatedStudentDto == null)
                return BadRequest("Invalid request.");

            var student = await _studentRepository.GetAsync(student => student.Id == id);
            if (student == null) return NotFound($"Student with ID {id} not found.");

            _mapper.Map(updatedStudentDto, student);
            await _studentRepository.UpdateAsync(student);

            return Ok(true);
        }

        [HttpPost]
        public async Task<ActionResult<StudentDTO>> CreateStudentAsync([FromBody] StudentDTO dto)
        {
            if (dto == null) return BadRequest("Invalid student data.");

            var student = _mapper.Map<Student>(dto);
            var studentAfrerCreation = await _studentRepository.CreateAsync(student);

            dto.Id = studentAfrerCreation.Id;

            _apiResponse.data = dto;
            _apiResponse.Status = true;
            _apiResponse.StatusCode = HttpStatusCode.OK;

            return CreatedAtRoute("GetStudentById", new { id = student.Id }, _apiResponse);
        }

        [HttpPut("update")]
        public async Task<ActionResult> UpdateStudentAsync([FromBody] StudentDTO dto)
        {
            if (dto == null || dto.Id <= 0)
                return BadRequest("Invalid student data.");

            var existingStudent = await _studentRepository.GetAsync(student => student.Id == dto.Id, true);
            if (existingStudent == null)
                return NotFound($"Student with ID {dto.Id} not found.");

            _mapper.Map(dto, existingStudent);
            await _studentRepository.UpdateAsync(existingStudent);

            return NoContent();
        }

        [HttpPatch("updatepartial/{id}")]
        public async Task<ActionResult> UpdateStudentPartialAsync(int id, [FromBody] JsonPatchDocument<StudentDTO> patchDoc)
        {
            if (patchDoc == null) return BadRequest("Invalid patch document.");

            var existingStudent = await _studentRepository.GetAsync(student => student.Id == id, true);
            if (existingStudent == null)
                return NotFound($"Student with ID {id} not found.");

            var studentDto = _mapper.Map<StudentDTO>(existingStudent);

            // Apply the patch correctly
            patchDoc.ApplyTo(studentDto, (error) => ModelState.AddModelError("", error.ErrorMessage));

            // Validate the patched object
            if (!TryValidateModel(studentDto))
                return BadRequest(ModelState);

            _mapper.Map(studentDto, existingStudent);
            await _studentRepository.UpdateAsync(existingStudent);

            return NoContent();
        }

    }
}
















