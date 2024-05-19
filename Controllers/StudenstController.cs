using eSchool.DataAccess.Repositories.StudentRepo;
using eSchool.DataAccess.Repositories.UnitOfWork;
using eSchool.Models.Models;
using eSchool.Models.Models.DTO.StudentDTO;
using Mapster;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace eSchool.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentsController : ControllerBase
    {
        private readonly IUnitOfWork db;
        public StudentsController(IUnitOfWork _db) {
            db = _db;
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var Students = await db.Students.GetAllStudents();
                return Ok(Students);
            }
            catch (Exception exc)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Unexpected error occured while processing your request: {exc.Message}");
            }
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            try
            {
            var Student =  db.Students.GetStudent(id);

                return Student != null ? Ok(Student) : NotFound("Student not found.");
            }
            catch (Exception err)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error getting students: {err.Message}");
            }
        }
        [HttpPost("Add")]
        public IActionResult AddStudent(StudentDto data)
        {
            if (data == null || !ModelState.IsValid) return BadRequest();

            if(db.Students.indexNumberExists(data) == true) return BadRequest("Index number already exists.");
            try
            {
                var newStudent = new Student();
                data.Adapt(newStudent);
                db.Students.AddStudent(newStudent);
                db.SaveChanges();
                return Ok(newStudent);
            }
            catch (Exception exc)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"{exc.Message}");
            }

        }
        [HttpDelete("Delete{id}")]
        public IActionResult Delete([FromRoute]int id) {
            try
            {
            var toDelete = db.Students.RemoveStudent(id);
            if (toDelete)
                return Ok();
                return NotFound();

            }
            catch (Exception exc)
            {

                return StatusCode(StatusCodes.Status500InternalServerError, $"Unexpected error occured while processing your request: {exc.Message}");
            }
        }
    [HttpPut("Update/{id}")]
    public IActionResult UpdateStudent([FromBody]StudentDto data, [FromRoute] int id)
        {
            var toUpdate =  db.Students.GetStudent(id); 
            if(data == null || !ModelState.IsValid ||toUpdate == null) { return BadRequest(); }
            if(data.IndexNumber != toUpdate.IndexNumber && db.Students.indexNumberExists(data) == true) return BadRequest("Index number already exists.");
            try
            {
                data.Adapt(toUpdate);
                db.Students.UpdateStudent(toUpdate);
                db.SaveChanges();
                return Ok(toUpdate);
            }
            catch (Exception exc)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Unexpected error occured while processing your request: {exc.Message}");
            }
        }
        [HttpGet("Pretraga")]
        public async Task<IActionResult> Filter([FromQuery]StudentFilterDto filter)
        {
            try
            {
            var FilterResult  = await db.Students.Filter(filter);

            return Ok(FilterResult);
            }
            catch (Exception exc)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Unexpected error occured while processing your request: {exc.Message}");
            }
        }
    }

}
