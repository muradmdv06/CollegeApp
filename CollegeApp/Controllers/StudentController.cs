using CollegeApp.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq; // Required for LINQ

namespace CollegeApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        [HttpGet]
        [Route("All", Name = "GetAllStudents")]
        public ActionResult<IEnumerable<Student>> GetStudents()
        {

            return Ok(CollegeRepository.Students);
        }

        [HttpGet]
        [Route("{id:int}", Name = "GetStudentById")] // Restrict to integer ID
        public ActionResult<Student> GetStudentById(int id)
        {

            if (id <= 0)
            
                return BadRequest();

            var student = CollegeRepository.Students.Where(n=>n.Id==id).FirstOrDefault();
            if(student == null)
                return NotFound($"The student with id {id} not found");
            
            

            return Ok(student) ;
        }

        [HttpGet]
        [Route("{name:alpha}", Name = "GetStudentByName")] // Updated route
        public ActionResult<Student> GetStudentByName(string name)
        {
            if (string.IsNullOrEmpty(name))

                return BadRequest();

            var student = CollegeRepository.Students.Where(n => n.StudentName == name).FirstOrDefault();
            if (student == null)
                return NotFound($"The student with name {name} not found");



            return Ok(student);
            

        }

        [HttpDelete]
        [Route("{id:min(1):max(100)}", Name = "DeleteStudentById")] // Fixed route name
        public ActionResult<bool> DeleteStudent(int id)
        {
            if (id <= 0)

                return BadRequest();

            var student = CollegeRepository.Students.Where(n => n.Id == id).FirstOrDefault();
            if (student == null)
                return NotFound($"The student with id {id} not found");

            
            CollegeRepository.Students.Remove(student);

            return Ok(true);
        }
    }
}



