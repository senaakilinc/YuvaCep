using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using YuvaCep.Application.Dtos;
using YuvaCep.Domain.Entities;
using YuvaCep.Persistence.Contexts;

namespace YuvaCep.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ActivityChartsController : ControllerBase
    {
        private readonly YuvaCepDbContext _context;

        public ActivityChartsController(YuvaCepDbContext context)
        {
            _context = context;
        }

        [Authorize(Roles = "Teacher")]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateActivityChartDto request)
        {
            var userId = Guid.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value);
            var teacherClass = await _context.Classes.FirstOrDefaultAsync(c => c.TeacherId == userId);
            if (teacherClass == null) return BadRequest("Sınıf bulunamadı.");

            var chart = new ActivityChart
            {
                Id = Guid.NewGuid(),
                ClassId = teacherClass.Id,
                Title = request.Title,
                Theme = request.Theme,
                Month = request.Month,
                Year = request.Year,
                CreatedAt = DateTime.UtcNow
            };

            _context.ActivityCharts.Add(chart);
            await _context.SaveChangesAsync();
            return Ok(new { message = "Çizelge oluşturuldu." });
        }

        [Authorize(Roles = "Teacher")]
        [HttpGet("teacher")]
        public async Task<IActionResult> GetTeacherCharts([FromQuery] int month, [FromQuery] int year)
        {
            var userId = Guid.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value);

            var teacherClass = await _context.Classes
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.TeacherId == userId);

            if (teacherClass == null) return Ok(new List<ActivityChartDto>());

            var charts = await _context.ActivityCharts
                .AsNoTracking()
                .Where(c => c.ClassId == teacherClass.Id)
                .ToListAsync();

            var result = charts.Select(c => new ActivityChartDto
            {
                Id = c.Id,
                Title = c.Title,
                Theme = c.Theme,
                TotalCompletedCount = 0
            }).ToList();

            return Ok(result);
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] Guid studentId, [FromQuery] int month, [FromQuery] int year)
        {
            var student = await _context.Students.FindAsync(studentId);
            if (student == null) return NotFound("Öğrenci yok.");

            var charts = await _context.ActivityCharts
                .AsNoTracking()
                .Where(c => c.ClassId == student.ClassId)
                .ToListAsync();

            var result = new List<ActivityChartDto>();
            var today = DateTime.UtcNow.Date;

            foreach (var chart in charts)
            {
                var entries = await _context.StudentChartEntries
                    .AsNoTracking()
                    .Where(e => e.ActivityChartId == chart.Id && e.StudentId == studentId)
                    .ToListAsync();

                result.Add(new ActivityChartDto
                {
                    Id = chart.Id,
                    Title = chart.Title,
                    Theme = chart.Theme,
                    IsCompletedToday = entries.Any(e => e.Date.Date == today),
                    TotalCompletedCount = entries.Count
                });
            }

            return Ok(result);
        }

        [Authorize]
        [HttpGet("{id}/details")]
        public async Task<IActionResult> GetDetails(Guid id)
        {
            var chart = await _context.ActivityCharts.FindAsync(id);
            if (chart == null) return NotFound();

            var students = await _context.Students
                .AsNoTracking()
                .Where(s => s.ClassId == chart.ClassId)
                .ToListAsync();

            var allEntries = await _context.StudentChartEntries
                .AsNoTracking()
                .Where(e => e.ActivityChartId == id)
                .ToListAsync();

            var today = DateTime.Now.Date;

            var studentDtos = students.Select(s => {
                var studentEntries = allEntries.Where(e => e.StudentId == s.Id).ToList();

                return new ChartStudentDetailDto
                {
                    StudentId = s.Id,
                    StudentName = $"{s.Name} {s.Surname}",
                    PhotoUrl = s.PhotoUrl,
                    CompletedCount = studentEntries.Count(e => e.IsCompleted),
                    IsCompletedToday = studentEntries.Any(e => e.Date.Date == today && e.IsCompleted),
                    CompletedDates = studentEntries.Where(e => e.IsCompleted).Select(e => e.Date).ToList()
                };
            }).ToList();

            return Ok(new ActivityChartDetailDto
            {
                ChartId = chart.Id,
                Title = chart.Title,
                Month = chart.Month,
                Year = chart.Year,
                Students = studentDtos 
            });
        }

        [Authorize]
        [HttpPost("complete")]
        public async Task<IActionResult> Complete([FromBody] CompleteChartDto request)
        {

            if (request.ActivityChartId == Guid.Empty || request.StudentId == Guid.Empty)
                return BadRequest("Eksik veri.");
            var existingEntry = await _context.StudentChartEntries
                .FirstOrDefaultAsync(e => e.ActivityChartId == request.ActivityChartId
                                       && e.StudentId == request.StudentId
                                       && e.Date.Date == request.Date.Date);

            if (existingEntry != null)
                return Ok(new { message = "Zaten tamamlanmış." });

            var newEntry = new StudentChartEntry
            {
                Id = Guid.NewGuid(),
                ActivityChartId = request.ActivityChartId,
                StudentId = request.StudentId,
                Date = request.Date,
                IsCompleted = true
            };

            _context.StudentChartEntries.Add(newEntry);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Başarıyla kaydedildi." });
        }

        [Authorize]
        [HttpGet("{id}/student-details")]
        public async Task<IActionResult> GetStudentDetails(Guid id, [FromQuery] Guid studentId)
        {

            var chart = await _context.ActivityCharts.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
            if (chart == null) return NotFound("Çizelge bulunamadı.");

            var completedDates = await _context.StudentChartEntries
                .AsNoTracking()
                .Where(e => e.ActivityChartId == id && e.StudentId == studentId && e.IsCompleted)
                .Select(e => e.Date) 
                .ToListAsync();

            var result = new StudentActivityChartDetailDto
            {
                Id = chart.Id,
                StudentId = studentId,
                Title = chart.Title,
                Month = chart.Month,
                Year = chart.Year,
                CompletedDates = completedDates 
            };

            return Ok(result);
        }

        [Authorize(Roles = "Teacher")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var chart = await _context.ActivityCharts.FindAsync(id);
            if (chart == null) return NotFound("Çizelge bulunamadı.");

            var relatedEntries = await _context.StudentChartEntries
                .Where(e => e.ActivityChartId == id)
                .ToListAsync();

            if (relatedEntries.Any())
            {
                _context.StudentChartEntries.RemoveRange(relatedEntries);
            }

            _context.ActivityCharts.Remove(chart);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Çizelge ve bağlı kayıtlar silindi." });
        }
    }
}