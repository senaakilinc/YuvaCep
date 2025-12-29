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
    public class DailyReportsController : ControllerBase
    {
        private readonly YuvaCepDbContext _context;

        public DailyReportsController(YuvaCepDbContext context)
        {
            _context = context;
        }

        // 1. RAPOR OLUŞTUR (Öğretmen)
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateDailyReportDto request)
        {
            var report = new DailyReport
            {
                Id = Guid.NewGuid(),
                Date = DateTime.UtcNow,
                StudentId = request.StudentId,

                // Mod
                Mood = request.Mood,
                MoodNote = request.MoodNote ?? "",

                // Yemek
                Breakfast = request.Breakfast,
                Lunch = request.Lunch,
                FoodNote = request.FoodNote ?? "",

                // Uyku
                Sleep = request.Sleep,

                // Etkinlik
                Activity = request.Activity,
                ActivityNote = request.ActivityNote ?? "",

                // Genel
                TeacherNote = request.TeacherNote ?? ""
            };

            _context.DailyReports.Add(report);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Günlük rapor kaydedildi." });
        }

        // 2. ÖĞRENCİNİN RAPORLARINI GETİR (Veli)
        [HttpGet("student/{studentId}")]
        public async Task<IActionResult> GetByStudentId(Guid studentId)
        {
            var reports = await _context.DailyReports
                .Include(r => r.Student)
                .Where(r => r.StudentId == studentId)
                .OrderByDescending(r => r.Date)
                .Select(r => new DailyReportDto
                {
                    Id = r.Id,
                    Date = r.Date,
                    StudentName = r.Student.Name,

                    Mood = r.Mood,
                    MoodNote = r.MoodNote,

                    Breakfast = r.Breakfast,
                    Lunch = r.Lunch,
                    FoodNote = r.FoodNote,

                    Sleep = r.Sleep,

                    Activity = r.Activity,
                    ActivityNote = r.ActivityNote,

                    TeacherNote = r.TeacherNote
                })
                .ToListAsync();

            return Ok(reports);
        }

        [HttpGet("reported-today-ids")]
        public async Task<IActionResult> GetReportedStudentIds()
        {
            // Bugünün başlangıcı (Saat 00:00)
            var today = DateTime.UtcNow.Date;

            // Veritabanında tarihi 'bugün' olan raporların Öğrenci ID'lerini çek
            var reportedStudentIds = await _context.DailyReports
                .Where(r => r.Date >= today) // Bugün ve sonrası
                .Select(r => r.StudentId)
                .Distinct() // Aynı öğrenci 2 kere geldiyse 1 kere say
                .ToListAsync();

            return Ok(reportedStudentIds);
        }
    }
}