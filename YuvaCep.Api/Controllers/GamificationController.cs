using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using YuvaCep.Domain.Entities;
using YuvaCep.Persistence.Contexts;

namespace YuvaCep.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GamificationController : ControllerBase
    {
        private readonly YuvaCepDbContext _context;

        public GamificationController(YuvaCepDbContext context)
        {
            _context = context;
        }

        [HttpPost("seed-badges")]
        public async Task<IActionResult> SeedBadges()
        {
            if (_context.BadgeDefinitions.Any()) return Ok("Zaten rozetler var.");

            var badges = new List<BadgeDefinition>
            {
                new BadgeDefinition
                {
                    Id = Guid.NewGuid(),
                    Name = "Başlangıç Rozeti",
                    Description = "İlk aktiviteni tamamladın!",
                    ImageUrl = "badge_start.png",
                    TargetCount = 1
                },
                new BadgeDefinition
                {
                    Id = Guid.NewGuid(),
                    Name = "Bronz Kupa",
                    Description = "5 farklı gün aktivite yaptın!",
                    ImageUrl = "badge_bronze.png",
                    TargetCount = 5
                },
                new BadgeDefinition
                {
                    Id = Guid.NewGuid(),
                    Name = "Gümüş Madalya",
                    Description = "10 farklı gün aktivite yaptın!",
                    ImageUrl = "badge_silver.png",
                    TargetCount = 10
                },
                new BadgeDefinition
                {
                    Id = Guid.NewGuid(),
                    Name = "Altın Yıldız",
                    Description = "20 farklı gün aktivite yaptın!",
                    ImageUrl = "badge_gold.png",
                    TargetCount = 20
                }
            };

            _context.BadgeDefinitions.AddRange(badges);
            await _context.SaveChangesAsync();

            return Ok("Rozetler başarıyla oluşturuldu.");
        }

        [Authorize]
        [HttpGet("student-badges")]
        public async Task<IActionResult> GetStudentBadges([FromQuery] Guid studentId)
        {
            var allBadges = await _context.BadgeDefinitions.AsNoTracking().OrderBy(b => b.TargetCount).ToListAsync();
            var earnedBadges = await _context.StudentBadges
                .AsNoTracking()
                .Where(sb => sb.StudentId == studentId)
                .ToListAsync();

            var result = allBadges.Select(badgeDef =>
            {
                var earned = earnedBadges.FirstOrDefault(eb => eb.BadgeDefinitionId == badgeDef.Id);
                return new
                {
                    Name = badgeDef.Name,
                    Description = badgeDef.Description,
                    ImageUrl = badgeDef.ImageUrl,
                    TargetCount = badgeDef.TargetCount,
                    IsEarned = earned != null,
                    EarnedDate = earned?.EarnedDate
                };
            }).ToList();

            return Ok(result);
        }

        [Authorize(Roles = "Teacher")]
        [HttpGet("class-status")]
        public async Task<IActionResult> GetClassBadgeStatus()
        {
            var userId = Guid.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value);

            // Öğretmenin sınıfını bul
            var teacherClass = await _context.Classes.FirstOrDefaultAsync(c => c.TeacherId == userId);
            if (teacherClass == null) return BadRequest("Sınıf bulunamadı.");

            // Sınıftaki öğrencileri çek
            var students = await _context.Students
                .AsNoTracking()
                .Where(s => s.ClassId == teacherClass.Id)
                .ToListAsync();

            var resultList = new List<object>();

            foreach (var student in students)
            {
                // Öğrencinin toplam aktif gün sayısını hesapla
                var activeDays = await _context.StudentChartEntries
                    .AsNoTracking()
                    .Where(e => e.StudentId == student.Id && e.IsCompleted)
                    .Select(e => e.Date.Date)
                    .Distinct()
                    .CountAsync();

                string badgeIcon = "badge_lock.png"; // Varsayılan/Kilitli
                if (activeDays >= 20) badgeIcon = "badge_gold.png";
                else if (activeDays >= 10) badgeIcon = "badge_silver.png";
                else if (activeDays >= 5) badgeIcon = "badge_bronze.png";
                else if (activeDays >= 1) badgeIcon = "badge_start.png";

                resultList.Add(new
                {
                    StudentId = student.Id,
                    Name = $"{student.Name} {student.Surname}",
                    CompletedDays = activeDays,
                    BadgeIcon = badgeIcon,
                    HasBadge = activeDays >= 1 // En az 1 gün yaptıysa rozeti var sayalım
                });
            }

            return Ok(resultList.OrderByDescending(x => ((dynamic)x).CompletedDays));
        }
    }
}