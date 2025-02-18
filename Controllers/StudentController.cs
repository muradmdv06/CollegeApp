using AutoMapper;
using CollegeApp.Data;
using CollegeApp.Data.Repository;
using CollegeApp.Models;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CollegeApp.Controllers
{
    [Route("api/students")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private readonly ILogger<StudentController> _logger;
        private readonly IMapper _mapper;
        private readonly IStudentRepository _studentRepository;

        public StudentController(ILogger<StudentController> logger, IMapper mapper, IStudentRepository studentRepository)
        {
            _logger = logger;
            _mapper = mapper;
            _studentRepository = studentRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<StudentDTO>>> GetStudents()
        {
            _logger.LogInformation("Fetching all students.");
            var students = await _studentRepository.GetAllAsync();
            return Ok(_mapper.Map<IEnumerable<StudentDTO>>(students));
        }

        [HttpGet("{id:int}", Name = "GetStudentById")]
        public async Task<ActionResult<StudentDTO>> GetStudentById(int id)
        {
            if (id <= 0) return BadRequest("Invalid student ID.");

            var student = await _studentRepository.GetByIdAsync(id);
            if (student == null) return NotFound($"Student with ID {id} not found.");

            return Ok(_mapper.Map<StudentDTO>(student));
        }

        [HttpGet("name/{name}", Name = "GetStudentByName")]
        public async Task<ActionResult<StudentDTO>> GetStudentByName(string name)
        {
            if (string.IsNullOrWhiteSpace(name)) return BadRequest("Name cannot be empty.");

            var student = await _studentRepository.GetByNameAsync(name);
            if (student == null) return NotFound($"Student with name {name} not found.");

            return Ok(_mapper.Map<StudentDTO>(student));
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult<bool>> DeleteStudent(int id)
        {
            if (id <= 0) return BadRequest("Invalid student ID.");

            var student = await _studentRepository.GetByIdAsync(id);
            if (student == null) return NotFound($"Student with ID {id} not found.");

            await _studentRepository.DeleteAsync(id);
            return Ok(true);
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult<bool>> EditStudent(int id, [FromBody] StudentDTO updatedStudentDto)
        {
            if (id <= 0 || updatedStudentDto == null)
                return BadRequest("Invalid request.");

            var student = await _studentRepository.GetByIdAsync(id);
            if (student == null) return NotFound($"Student with ID {id} not found.");

            _mapper.Map(updatedStudentDto, student);
            await _studentRepository.UpdateAsync(student);

            return Ok(true);
        }

        [HttpPost]
        public async Task<ActionResult<StudentDTO>> CreateStudent([FromBody] StudentDTO dto)
        {
            if (dto == null) return BadRequest("Invalid student data.");

            var student = _mapper.Map<Student>(dto);
            await _studentRepository.CreateAsync(student);

            return CreatedAtRoute("GetStudentById", new { id = student.Id }, _mapper.Map<StudentDTO>(student));
        }

        [HttpPut("update")]
        public async Task<ActionResult> UpdateStudent([FromBody] StudentDTO dto)
        {
            if (dto == null || dto.Id <= 0)
                return BadRequest("Invalid student data.");

            var existingStudent = await _studentRepository.GetByIdAsync(dto.Id);
            if (existingStudent == null)
                return NotFound($"Student with ID {dto.Id} not found.");

            _mapper.Map(dto, existingStudent);
            await _studentRepository.UpdateAsync(existingStudent);

            return NoContent();
        }

        [HttpPatch("updatepartial/{id}")]
        public async Task<ActionResult> UpdateStudentPartial(int id, [FromBody] JsonPatchDocument<StudentDTO> patchDoc)
        {
            if (patchDoc == null) return BadRequest("Invalid patch document.");

            var existingStudent = await _studentRepository.GetByIdAsync(id);
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















