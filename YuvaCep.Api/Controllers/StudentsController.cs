using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using YuvaCep.Domain.Entities;
using YuvaCep.Persistence.Contexts;

namespace YuvaCep.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentsController : ControllerBase
    {
        private readonly YuvaCepDbContext _context;

        public StudentsController(YuvaCepDbContext context)
        {
            _context = context;
        }

        // GET: api/Students
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Student>>> GetStudents()
        {
            // Sınıf bilgisiyle beraber getir (Include)
            return await _context.Students
                                 .Include(c => c.Class)
                                 .ToListAsync();
        }

        // POST: api/Students
        [HttpPost]
        public async Task<ActionResult<Student>> PostStudent(Student student)
        {
            // Burada student nesnesi gelir.
            // ClassId ZORUNLUDUR (Veritabanı kuralı).
            // Class nesnesi ise [JsonIgnore] olduğu için API istemez.

            _context.Students.Add(student);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetStudents), new { id = student.Id }, student);
        }
    }
}