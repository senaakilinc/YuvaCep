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

        // VELİ: Çocuğun durumunu (Emoji) girer
        // Sistem o an rozet kontrolü yapar.
        [HttpPost("submit-mood")]
        public async Task<IActionResult> SubmitMood([FromForm] Guid studentId, [FromForm] string emoji)
        {
            // 1. Emojiyi Kaydet
            var mood = new StudentMood
            {
                StudentId = studentId,
                Emoji = emoji,
                CreatedAt = DateTime.UtcNow
            };
            _context.StudentMoods.Add(mood);
            await _context.SaveChangesAsync();

            // 2. ARKA PLAN İŞLEMİ: Rozet Kontrolü Yap (Trigger)
            await CheckAndAwardBadges(studentId);

            return Ok(new { Message = "Ruh hali kaydedildi, rozetler kontrol edildi.", Emoji = emoji });
        }

        // --- ÖZEL METOT: ROZET HESAPLAMA MOTORU ---
        private async Task CheckAndAwardBadges(Guid studentId)
        {
            var now = DateTime.UtcNow;

            // A. BU HAFTANIN VERİLERİ (Son 7 gün)
            var last7Days = now.AddDays(-7);
            var weeklyCount = await _context.StudentMoods
                .Where(m => m.StudentId == studentId && m.CreatedAt >= last7Days)
                .Select(m => m.CreatedAt.Date) // Sadece tarih kısmını al (Saat/Dakika at)
                .Distinct() // Tekrar eden günleri ele (Aynı gün 5 kere girdiyse 1 say)
                .CountAsync();

            // B. BU AYIN VERİLERİ
            var startOfMonth = new DateTime(now.Year, now.Month, 1).ToUniversalTime();
            var monthlyCount = await _context.StudentMoods
                .Where(m => m.StudentId == studentId && m.CreatedAt >= startOfMonth)
                .Select(m => m.CreatedAt.Date)
                .Distinct()
                .CountAsync();

            // --- KURAL 1: HAFTALIK KATILIM (Son 7 günde 3 giriş) ---
            if (weeklyCount >= 3)
            {
                await GiveBadgeIfNotExists(studentId, "WEEKLY", "Haftalık Katılım Rozeti");
            }

            // --- KURAL 2: GÜMÜŞ ROZET (Ayda 15 giriş) ---
            if (monthlyCount >= 15)
            {
                await GiveBadgeIfNotExists(studentId, "SILVER", "Aylık Gümüş Rozet");
            }

            // --- KURAL 3: ALTIN ROZET (Ayda 20 giriş) ---
            if (monthlyCount >= 20)
            {
                await GiveBadgeIfNotExists(studentId, "GOLD", "Aylık Altın Rozet");
            }
        }

        // Rozeti verirken "Daha önce almış mı?" diye bakar, spam'i önler.
        private async Task GiveBadgeIfNotExists(Guid studentId, string badgeCode, string badgeName)
        {
            // 1. Bu kodlu rozeti veritabanından bul
            var badgeDefinition = await _context.Badges.FirstOrDefaultAsync(b => b.Code == badgeCode);

            // Eğer rozet sistemde tanımlı değilse, otomatik oluştur (Seed Data yoksa patlamasın diye)
            if (badgeDefinition == null)
            {
                badgeDefinition = new Badge
                {
                    Name = badgeName,
                    Code = badgeCode,
                    ImagePath = $"/badges/{badgeCode.ToLower()}.png"
                };
                _context.Badges.Add(badgeDefinition);
                await _context.SaveChangesAsync();
            }

            // 2. Öğrenci bu rozeti bu ay/hafta almış mı?
            // (Basitlik için: Öğrencinin bu rozeti hiç var mı diye bakıyoruz)
            bool alreadyHas = await _context.StudentBadges
                .AnyAsync(sb => sb.StudentId == studentId && sb.BadgeId == badgeDefinition.Id);

            if (!alreadyHas)
            {
                var newBadge = new StudentBadge
                {
                    StudentId = studentId,
                    BadgeId = badgeDefinition.Id,
                    EarnedAt = DateTime.UtcNow
                };
                _context.StudentBadges.Add(newBadge);
                await _context.SaveChangesAsync();
            }
        }

        // VELİ: Rozetlerimi Göster
        [HttpGet("my-badges/{studentId}")]
        public async Task<IActionResult> GetBadges(Guid studentId)
        {
            var list = await _context.StudentBadges
                .Where(sb => sb.StudentId == studentId)
                .Include(sb => sb.Badge)
                .Select(sb => new
                {
                    BadgeName = sb.Badge.Name,
                    Image = sb.Badge.ImagePath,
                    EarnedDate = sb.EarnedAt
                })
                .ToListAsync();

            return Ok(list);
        }
    }
}