using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using eSchool.Models.Models.DTO.SubjectDTO;
using eSchool.Models.Models;
using Mapster;
using eSchool.DataAccess;

namespace eSchool.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SubjectsController : ControllerBase
    {
        private readonly eSchoolDbContext db;
        public SubjectsController(eSchoolDbContext _db)
        {
            db = _db;   
        }
        [HttpPost]
        public IActionResult AddSubject(SubjectDto data)
        {
            if (data == null || !ModelState.IsValid) return BadRequest();
            var newSubject = new Subject();
            data.Adapt(newSubject);

        }
    }
}
