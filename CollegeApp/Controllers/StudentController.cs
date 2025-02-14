using CollegeApp.Models;
using CollegeApp.MyLogging;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace CollegeApp.Controllers
{
    [Route("api/students")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        



        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<IEnumerable<Student>> GetStudents()
        {
            return Ok(CollegeRepository.Students);
        }

        [HttpGet("{id:int}", Name = "GetStudentById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<Student> GetStudentById(int id)
        {
            if (id <= 0) return BadRequest("Invalid student ID.");

            var student = CollegeRepository.Students.SingleOrDefault(n => n.Id == id);
            return student != null ? Ok(student) : NotFound($"Student with ID {id} not found.");
        }

        [HttpGet("name/{name}", Name = "GetStudentByName")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<Student> GetStudentByName(string name)
        {
            if (string.IsNullOrWhiteSpace(name)) return BadRequest("Name cannot be empty.");

            var student = CollegeRepository.Students.FirstOrDefault(n => n.StudentName == name);
            return student != null ? Ok(student) : NotFound($"Student with name {name} not found.");
        }

        [HttpDelete("{id:int}", Name = "DeleteStudentById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<bool> DeleteStudent(int id)
        {
            if (id <= 0) return BadRequest("Invalid student ID.");

            var student = CollegeRepository.Students.SingleOrDefault(n => n.Id == id);
            if (student == null) return NotFound($"Student with ID {id} not found.");

            CollegeRepository.Students.Remove(student);
            return Ok(true);
        }

        [HttpPut("{id:int}", Name = "EditStudentById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<bool> EditStudent(int id, [FromBody] Student updatedStudent)
        {
            if (id <= 0 || updatedStudent == null) return BadRequest("Invalid request.");

            var student = CollegeRepository.Students.SingleOrDefault(n => n.Id == id);
            if (student == null) return NotFound($"Student with ID {id} not found.");

            student.StudentName = updatedStudent.StudentName;
            student.Email = updatedStudent.Email;
            student.Address = updatedStudent.Address;

            return Ok(true);
        }

        [HttpPost("create")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<StudentDTO> CreateStudent([FromBody] StudentDTO model)
        {
            if (model == null) return BadRequest("Invalid student data.");

            int newId = CollegeRepository.Students.Any() ? CollegeRepository.Students.Max(s => s.Id) + 1 : 1;
            Student student = new Student
            {
                Id = newId,
                StudentName = model.StudentName,
                Address = model.Address,
                Email = model.Email
            };

            CollegeRepository.Students.Add(student);
            model.Id = student.Id;

            return CreatedAtRoute("GetStudentById", new { id = student.Id }, model);
        }

        [HttpPut("update")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult UpdateResult([FromBody] StudentDTO model)
        {
            if (model == null || model.Id <= 0) return BadRequest("Invalid student data.");

            var existingStudent = CollegeRepository.Students.SingleOrDefault(s => s.Id == model.Id);
            if (existingStudent == null) return NotFound($"Student with ID {model.Id} not found.");

            existingStudent.StudentName = model.StudentName;
            existingStudent.Email = model.Email;
            existingStudent.Address = model.Address;

            return NoContent();
        }


        [HttpPatch("updatepartial/{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult UpdateStudentPartial(int id, [FromBody] JsonPatchDocument<StudentDTO> patchDoc)
        {
            if (patchDoc == null) return BadRequest("Invalid patch document.");

            var existingStudent = CollegeRepository.Students.SingleOrDefault(s => s.Id == id);
            if (existingStudent == null) return NotFound($"Student with ID {id} not found.");


            var studentDto = new StudentDTO
            {
                StudentName = existingStudent.StudentName,
                Email = existingStudent.Email,
                Address = existingStudent.Address
            };


            patchDoc.ApplyTo(studentDto);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }


            existingStudent.StudentName = studentDto.StudentName;
            existingStudent.Email = studentDto.Email;
            existingStudent.Address = studentDto.Address;



            return NoContent();
        }

    }
}











