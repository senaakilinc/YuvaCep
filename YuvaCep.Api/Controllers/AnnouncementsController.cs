using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using YuvaCep.Application.Dtos;
using YuvaCep.Domain.Entities;
using YuvaCep.Persistence.Contexts;
using System.Security.Claims;

namespace YuvaCep.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AnnouncementsController : ControllerBase
    {
        private readonly YuvaCepDbContext _context;

        public AnnouncementsController(YuvaCepDbContext context)
        {
            _context = context;
        }

        [Authorize(Roles = "Teacher")]
        [HttpGet("my-class")]
        public async Task<IActionResult> GetMyClassAnnouncements()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null) return Unauthorized();

            var teacherClass = await _context.Classes.FirstOrDefaultAsync(c => c.TeacherId == Guid.Parse(userId));
            if (teacherClass == null) return NotFound("Sınıfınız bulunamadı.");

            return await GetByClassId(teacherClass.Id);
        }

        // DUYURU OLUŞTUR (Sadece Öğretmen)
        [Authorize(Roles = "Teacher")]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateAnnouncementDto request)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null) return Unauthorized();

            if (request.ClassId == null)
            {
                var teacherClass = await _context.Classes.FirstOrDefaultAsync(c => c.TeacherId == Guid.Parse(userId));
                if (teacherClass == null) return BadRequest("Sınıfınız bulunamadı.");
                request.ClassId = teacherClass.Id;
            }

            var announcement = new Announcement
            {
                Id = Guid.NewGuid(),
                Title = request.Title,
                Content = request.Content,
                ClassId = request.ClassId.Value,
                CreatedDate = DateTime.UtcNow
            };

            _context.Announcements.Add(announcement);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Duyuru başarıyla oluşturuldu." });
        }

        [Authorize] 
        [HttpGet("class/{classId}")]
        public async Task<IActionResult> GetByClassId(Guid classId)
        {
            var announcements = await _context.Announcements
                .Where(a => a.ClassId == classId)
                .OrderByDescending(a => a.CreatedDate)
                .Select(a => new AnnouncementDto
                {
                    Id = a.Id,
                    Title = a.Title,
                    Content = a.Content,
                    CreatedDate = a.CreatedDate
                })
                .ToListAsync();

            return Ok(announcements);
        }

        [Authorize]
        [HttpGet("student/{studentId}")]
        public async Task<IActionResult> GetByStudentId(Guid studentId)
        {
            var student = await _context.Students.FindAsync(studentId);
            if (student == null) return NotFound("Öğrenci bulunamadı.");

            return await GetByClassId(student.ClassId);
        }
    }
}