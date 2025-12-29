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
    public class AnnouncementsController : ControllerBase
    {
        private readonly YuvaCepDbContext _context;

        public AnnouncementsController(YuvaCepDbContext context)
        {
            _context = context;
        }

        // 1. DUYURU OLUŞTUR (Sadece Öğretmenler - İleride [Authorize(Roles = "Teacher")] yapacağız)
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateAnnouncementDto request)
        {
            var announcement = new Announcement
            {
                Id = Guid.NewGuid(),
                Title = request.Title,
                Content = request.Content,
                ClassId = request.ClassId,
                CreatedDate = DateTime.UtcNow
            };

            _context.Announcements.Add(announcement);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Duyuru başarıyla oluşturuldu." });
        }

        // 2. SINIFIN DUYURULARINI GETİR
        [HttpGet("class/{classId}")]
        public async Task<IActionResult> GetByClassId(Guid classId)
        {
            var announcements = await _context.Announcements
                .Where(a => a.ClassId == classId)
                .OrderByDescending(a => a.CreatedDate) // En yeni en üstte
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
    }
}