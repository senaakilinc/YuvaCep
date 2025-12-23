using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using YuvaCep.Api.DTOs;
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
        public async Task<ActionResult<IEnumerable<StudentDto>>> GetStudents()
        {
            var students = await _context.Students
                .Include(s => s.Class)
                .Select(s => new StudentDto
                {
                    Id = s.Id,
                    Name = s.Name,
                    Surname = s.Surname,
                    TCIDNumber = s.TCIDNumber,
                    ReferenceCode = s.ReferenceCode,
                    DateOfBirth = s.DateOfBirth,
                    HealthNotes = s.HealthNotes,
                    ClassId = s.ClassId,
                    ClassName = s.Class != null ? s.Class.Name : null
                })
                .ToListAsync();

            return Ok(students);
        }

        // GET: api/Students/5
        [HttpGet("{id}")]
        public async Task<ActionResult<StudentDto>> GetStudent(Guid id)
        {
            var student = await _context.Students
                .Include(s => s.Class)
                .Where(s => s.Id == id)
                .Select(s => new StudentDto
                {
                    Id = s.Id,
                    Name = s.Name,
                    Surname = s.Surname,
                    TCIDNumber = s.TCIDNumber,
                    ReferenceCode = s.ReferenceCode,
                    DateOfBirth = s.DateOfBirth,
                    HealthNotes = s.HealthNotes,
                    ClassId = s.ClassId,
                    ClassName = s.Class != null ? s.Class.Name : null
                })
                .FirstOrDefaultAsync();

            if (student == null)
            {
                return NotFound(new { message = "Öğrenci bulunamadı" });
            }

            return Ok(student);
        }

        // POST: api/Students
        [HttpPost]
        public async Task<ActionResult<StudentDto>> PostStudent(CreateStudentDto createDto)
        {
            // Sınıfın var olup olmadığını kontrol et
            var classExists = await _context.Classes.AnyAsync(c => c.Id == createDto.ClassId);
            if (!classExists)
            {
                return BadRequest(new { message = "Belirtilen sınıf bulunamadı" });
            }

            // Referans kodunun benzersiz olup olmadığını kontrol et
            var referenceCodeExists = await _context.Students
                .AnyAsync(s => s.ReferenceCode == createDto.ReferenceCode);
            if (referenceCodeExists)
            {
                return BadRequest(new { message = "Bu referans kodu zaten kullanılıyor" });
            }

            // TC Kimlik No'nun benzersiz olup olmadığını kontrol et
            var tcExists = await _context.Students
                .AnyAsync(s => s.TCIDNumber == createDto.TCIDNumber);
            if (tcExists)
            {
                return BadRequest(new { message = "Bu TC Kimlik No zaten kayıtlı" });
            }

            var student = new Student
            {
                Id = Guid.NewGuid(),
                Name = createDto.Name,
                Surname = createDto.Surname,
                TCIDNumber = createDto.TCIDNumber,
                ReferenceCode = createDto.ReferenceCode,
                DateOfBirth = createDto.DateOfBirth,
                HealthNotes = createDto.HealthNotes,
                ClassId = createDto.ClassId
            };

            _context.Students.Add(student);
            await _context.SaveChangesAsync();

            // Oluşturulan öğrenciyi DTO olarak döndür
            var studentDto = new StudentDto
            {
                Id = student.Id,
                Name = student.Name,
                Surname = student.Surname,
                TCIDNumber = student.TCIDNumber,
                ReferenceCode = student.ReferenceCode,
                DateOfBirth = student.DateOfBirth,
                HealthNotes = student.HealthNotes,
                ClassId = student.ClassId,
                ClassName = await _context.Classes
                    .Where(c => c.Id == student.ClassId)
                    .Select(c => c.Name)
                    .FirstOrDefaultAsync()
            };

            return CreatedAtAction(nameof(GetStudent), new { id = student.Id }, studentDto);
        }

        // PUT: api/Students/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutStudent(Guid id, UpdateStudentDto updateDto)
        {
            if (id != updateDto.Id)
            {
                return BadRequest(new { message = "ID uyuşmazlığı" });
            }

            var student = await _context.Students.FindAsync(id);
            if (student == null)
            {
                return NotFound(new { message = "Öğrenci bulunamadı" });
            }

            // Sınıfın var olup olmadığını kontrol et
            var classExists = await _context.Classes.AnyAsync(c => c.Id == updateDto.ClassId);
            if (!classExists)
            {
                return BadRequest(new { message = "Belirtilen sınıf bulunamadı" });
            }

            // TC Kimlik No değişmişse, benzersizliğini kontrol et
            if (student.TCIDNumber != updateDto.TCIDNumber)
            {
                var tcExists = await _context.Students
                    .AnyAsync(s => s.TCIDNumber == updateDto.TCIDNumber && s.Id != id);
                if (tcExists)
                {
                    return BadRequest(new { message = "Bu TC Kimlik No zaten kayıtlı" });
                }
            }

            student.Name = updateDto.Name;
            student.Surname = updateDto.Surname;
            student.TCIDNumber = updateDto.TCIDNumber;
            student.DateOfBirth = updateDto.DateOfBirth;
            student.HealthNotes = updateDto.HealthNotes;
            student.ClassId = updateDto.ClassId;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!StudentExists(id))
                {
                    return NotFound(new { message = "Öğrenci bulunamadı" });
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // DELETE: api/Students/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStudent(Guid id)
        {
            var student = await _context.Students.FindAsync(id);
            if (student == null)
            {
                return NotFound(new { message = "Öğrenci bulunamadı" });
            }

            _context.Students.Remove(student);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool StudentExists(Guid id)
        {
            return _context.Students.Any(e => e.Id == id);
        }
    }
}