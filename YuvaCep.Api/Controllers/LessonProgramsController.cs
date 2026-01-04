using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using YuvaCep.Application.Dtos;
using YuvaCep.Domain.Entities;
using YuvaCep.Persistence.Contexts;

namespace YuvaCep.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LessonProgramsController : ControllerBase
    {
        private readonly YuvaCepDbContext _context;

        public LessonProgramsController(YuvaCepDbContext context)
        {
            _context = context;
        }

        [Authorize(Roles = "Teacher")]
        [HttpPost]
        [DisableRequestSizeLimit]
        public async Task<IActionResult> Upload([FromBody] CreateLessonProgramDto request)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null) return Unauthorized();

            var teacherClass = await _context.Classes.FirstOrDefaultAsync(c => c.TeacherId == Guid.Parse(userId));
            if (teacherClass == null) return BadRequest("Sınıfınız bulunamadı.");

            var existing = await _context.LessonPrograms
                .Include(lp => lp.Images)
                .FirstOrDefaultAsync(l => l.ClassId == teacherClass.Id && l.Month == request.Date.Month && l.Year == request.Date.Year);

            if (existing != null)
            {
                foreach (var img in request.Images)
                {
                    _context.LessonProgramImages.Add(new LessonProgramImage
                    {
                        Id = Guid.NewGuid(),
                        ImageBase64 = img,
                        LessonProgramId = existing.Id
                    });
                }
                existing.CreatedAt = DateTime.UtcNow;
            }
            else
            {
                var newProgram = new LessonProgram
                {
                    Id = Guid.NewGuid(),
                    ClassId = teacherClass.Id,
                    Month = request.Date.Month,
                    Year = request.Date.Year,
                    CreatedAt = DateTime.UtcNow
                };

                _context.LessonPrograms.Add(newProgram);

                foreach (var img in request.Images)
                {
                    newProgram.Images.Add(new LessonProgramImage
                    {
                        Id = Guid.NewGuid(),
                        ImageBase64 = img
                    });
                }
            }

            try
            {
                await _context.SaveChangesAsync();
                return Ok(new { message = "Resimler başarıyla eklendi." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Veritabanı hatası: {ex.Message}");
            }
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] int month, [FromQuery] int year, [FromQuery] Guid? studentId)
        {
            Guid classId = Guid.Empty;

            if (User.IsInRole("Teacher"))
            {
                var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
                var cls = await _context.Classes.FirstOrDefaultAsync(c => c.TeacherId == userId);
                if (cls != null) classId = cls.Id;
            }
            else if (studentId.HasValue)
            {
                var student = await _context.Students.FindAsync(studentId.Value);
                if (student != null) classId = student.ClassId;
            }

            if (classId == Guid.Empty) return NotFound("Sınıf bilgisi bulunamadı.");

            var program = await _context.LessonPrograms
                .Include(lp => lp.Images)
                .FirstOrDefaultAsync(l => l.ClassId == classId && l.Month == month && l.Year == year);

            if (program == null || !program.Images.Any()) return NoContent();

            return Ok(new LessonProgramDto
            {
                Id = program.Id,
                Month = program.Month,
                Year = program.Year,
                Images = program.Images.Select(i => new LessonProgramImageDto
                {
                    Id = i.Id,
                    ImageBase64 = i.ImageBase64
                }).ToList()
            });
        }

        [Authorize(Roles = "Teacher")]
        [HttpDelete("image/{imageId}")]
        public async Task<IActionResult> DeleteImage(Guid imageId)
        {
            var image = await _context.LessonProgramImages.FindAsync(imageId);
            if (image == null) return NotFound("Resim bulunamadı.");

            _context.LessonProgramImages.Remove(image);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Resim silindi." });
        }
    }
}