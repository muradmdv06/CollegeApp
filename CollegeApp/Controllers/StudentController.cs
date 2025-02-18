using CollegeApp.Data;
using CollegeApp.Models;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CollegeApp.Controllers
{
    [Route("api/students")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private readonly ILogger<StudentController> _logger;
        private readonly CollegeDBContext _dbContext;

        public StudentController(ILogger<StudentController> logger, CollegeDBContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<Student>>> GetStudents()
        {
            _logger.LogInformation("Fetching all students from the database.");
            var students = await _dbContext.Students.ToListAsync();
            return Ok(students);
        }

        [HttpGet("{id:int}", Name = "GetStudentById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Student>> GetStudentById(int id)
        {
            if (id <= 0) return BadRequest("Invalid student ID.");

            var student = await _dbContext.Students.FindAsync(id);
            if (student == null)
                return NotFound($"Student with ID {id} not found.");

            return Ok(student);
        }

        [HttpGet("name/{name}", Name = "GetStudentByName")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Student>> GetStudentByName(string name)
        {
            if (string.IsNullOrWhiteSpace(name)) return BadRequest("Name cannot be empty.");

            var student = await _dbContext.Students.FirstOrDefaultAsync(n => n.StudentName == name);
            if (student == null)
                return NotFound($"Student with name {name} not found.");

            return Ok(student);
        }

        [HttpDelete("{id:int}", Name = "DeleteStudentById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<bool>> DeleteStudent(int id)
        {
            if (id <= 0) return BadRequest("Invalid student ID.");

            var student = await _dbContext.Students.FindAsync(id);
            if (student == null)
                return NotFound($"Student with ID {id} not found.");

            _dbContext.Students.Remove(student);
            await _dbContext.SaveChangesAsync();

            return Ok(true);
        }

        [HttpPut("{id:int}", Name = "EditStudentById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<bool>> EditStudent(int id, [FromBody] Student updatedStudent)
        {
            if (id <= 0 || updatedStudent == null)
                return BadRequest("Invalid request.");

            var student = await _dbContext.Students.FindAsync(id);
            if (student == null)
                return NotFound($"Student with ID {id} not found.");

            student.StudentName = updatedStudent.StudentName;
            student.Email = updatedStudent.Email;
            student.Address = updatedStudent.Address;

            await _dbContext.SaveChangesAsync();
            return Ok(true);
        }

        [HttpPost("create")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<StudentDTO>> CreateStudent([FromBody] StudentDTO model)
        {
            if (model == null)
                return BadRequest("Invalid student data.");

            var student = new Student
            {
                StudentName = model.StudentName,
                Address = model.Address,
                Email = model.Email
            };

            _dbContext.Students.Add(student);
            await _dbContext.SaveChangesAsync();

            model.Id = student.Id;

            return CreatedAtRoute("GetStudentById", new { id = student.Id }, model);
        }

        [HttpPut("update")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> UpdateResult([FromBody] StudentDTO model)
        {
            if (model == null || model.Id <= 0)
                return BadRequest("Invalid student data.");

            var existingStudent = await _dbContext.Students.FindAsync(model.Id);
            if (existingStudent == null)
                return NotFound($"Student with ID {model.Id} not found.");

            var newRecord = new Student()
            {
                Id = existingStudent.Id,
                StudentName = existingStudent.StudentName,
                Email = existingStudent.Email,
                Address = existingStudent.Address,
            };
            _dbContext.Students.Update(newRecord);



            //existingStudent.StudentName = model.StudentName;
            //existingStudent.Email = model.Email;
            //existingStudent.Address = model.Address;

            await _dbContext.SaveChangesAsync();

            return NoContent();
        }

        [HttpPatch("updatepartial/{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> UpdateStudentPartial(int id, [FromBody] JsonPatchDocument<StudentDTO> patchDoc)
        {
            if (patchDoc == null)
                return BadRequest("Invalid patch document.");

            var existingStudent = await _dbContext.Students.FindAsync(id);
            if (existingStudent == null)
                return NotFound($"Student with ID {id} not found.");

            // Map existing student data to DTO
            var studentDto = new StudentDTO
            {
                StudentName = existingStudent.StudentName,
                Email = existingStudent.Email,
                Address = existingStudent.Address
            };

            // Apply the patch (without ModelState)
            patchDoc.ApplyTo(studentDto);

            // Ensure no null values are introduced (basic validation)
            if (string.IsNullOrWhiteSpace(studentDto.StudentName) ||
                string.IsNullOrWhiteSpace(studentDto.Email) ||
                string.IsNullOrWhiteSpace(studentDto.Address))
            {
                return BadRequest("Updated fields cannot be empty.");
            }

            // Apply validated changes to entity
            existingStudent.StudentName = studentDto.StudentName;
            existingStudent.Email = studentDto.Email;
            existingStudent.Address = studentDto.Address;

            await _dbContext.SaveChangesAsync();

            return NoContent();
        }

    }
}












