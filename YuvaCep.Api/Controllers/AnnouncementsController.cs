using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using YuvaCep.Domain.Entities;
using YuvaCep.Persistence.Contexts;

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

        // GET: api/Announcements
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Announcement>>> GetAnnouncements()
        {
            return await _context.Announcements.ToListAsync();
        }

        // POST: api/Announcements
        [HttpPost]
        public async Task<ActionResult<Announcement>> PostAnnouncement(Announcement announcement)
        {
            // Tarihi otomatik ver
            announcement.CreatedAt = DateTime.UtcNow;

            // METİNDEKİ KURAL:
            // Eğer TargetClassId NULL gelirse -> Tüm Okula
            // Eğer TargetClassId DOLU gelirse -> O Sınıfa
            // Bu mantık veritabanında zaten kurulu, direkt kaydediyoruz.

            _context.Announcements.Add(announcement);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetAnnouncements), new { id = announcement.Id }, announcement);
        }
    }
}