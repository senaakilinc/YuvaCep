using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using YuvaCep.Application.Dtos;
using YuvaCep.Application.Services;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace YuvaCep.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AnnouncementsController : ControllerBase
    {
        private readonly IAnnouncementService _announcementService;

        public AnnouncementsController(IAnnouncementService announcementService)
        {
            _announcementService = announcementService;
        }

        // GET: api/Announcements
        // Tüm duyuruları getir (Sadece öğretmen)
        [HttpGet]
        [Authorize(Roles = "Teacher,Admin")]
        public async Task<IActionResult> GetAllAnnouncements(
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10)
        {
            var announcements = await _announcementService.GetAllAsync(pageNumber, pageSize);
            return Ok(announcements);
        }

        // GET: api/Announcements/{id}
        // Tek duyuru detayı
        [HttpGet("{id}")]
        public async Task<IActionResult> GetAnnouncement(Guid id)
        {
            var announcement = await _announcementService.GetByIdAsync(id);

            if (announcement == null)
            {
                return NotFound(new { message = "Duyuru bulunamadı." });
            }

            return Ok(announcement);
        }

        // GET: api/Announcements/my-announcements
        // Velinin kendi duyuruları
        [HttpGet("my-announcements")]
        [Authorize(Roles = "Parent")]
        public async Task<IActionResult> GetMyAnnouncements(
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10)
        {
            var parentId = GetCurrentUserId();
            var announcements = await _announcementService.GetForParentAsync(parentId, pageNumber, pageSize);
            return Ok(announcements);
        }

        // GET: api/Announcements/teacher/{teacherId}
        // Öğretmenin duyuruları
        [HttpGet("teacher/{teacherId}")]
        [Authorize(Roles = "Teacher,Admin")]
        public async Task<IActionResult> GetTeacherAnnouncements(
            Guid teacherId,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10)
        {
            var announcements = await _announcementService.GetByTeacherIdAsync(teacherId, pageNumber, pageSize);
            return Ok(announcements);
        }

        // GET: api/Announcements/my-teacher-announcements
        // Giriş yapan öğretmenin kendi duyuruları
        [HttpGet("my-teacher-announcements")]
        [Authorize(Roles = "Teacher")]
        public async Task<IActionResult> GetMyTeacherAnnouncements(
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10)
        {
            var teacherId = GetCurrentUserId();
            var announcements = await _announcementService.GetByTeacherIdAsync(teacherId, pageNumber, pageSize);
            return Ok(announcements);
        }

        // GET: api/Announcements/class/{classId}
        // Sınıfa ait duyurular
        [HttpGet("class/{classId}")]
        public async Task<IActionResult> GetClassAnnouncements(
            Guid classId,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10)
        {
            var announcements = await _announcementService.GetByClassIdAsync(classId, pageNumber, pageSize);
            return Ok(announcements);
        }

        // GET: api/Announcements/student/{studentId}
        // Öğrenciye ait duyurular
        [HttpGet("student/{studentId}")]
        public async Task<IActionResult> GetStudentAnnouncements(
            Guid studentId,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10)
        {
            try
            {
                var announcements = await _announcementService.GetByStudentIdAsync(studentId, pageNumber, pageSize);
                return Ok(announcements);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // POST: api/Announcements
        // Duyuru oluştur VE bildirim gönder (Sadece öğretmen)
        [HttpPost]
        [Authorize(Roles = "Teacher,Admin")]
        public async Task<IActionResult> CreateAnnouncement([FromBody] CreateAnnouncementDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var teacherId = GetCurrentUserId();

            try
            {
                var (announcement, notificationResult) = await _announcementService.CreateAndNotifyAsync(dto, teacherId);

                return CreatedAtAction(
                    nameof(GetAnnouncement),
                    new { id = announcement.Id },
                    new
                    {
                        announcement,
                        notification = notificationResult
                    });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // POST: api/Announcements/draft
        // Duyuru oluştur (bildirim göndermeden - taslak)
        [HttpPost("draft")]
        [Authorize(Roles = "Teacher,Admin")]
        public async Task<IActionResult> CreateDraftAnnouncement([FromBody] CreateAnnouncementDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var teacherId = GetCurrentUserId();

            try
            {
                var announcement = await _announcementService.CreateAsync(dto, teacherId);
                return CreatedAtAction(nameof(GetAnnouncement), new { id = announcement.Id }, announcement);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // DELETE: api/Announcements/{id}
        // Duyuruyu sil (Sadece öğretmen)
        [HttpDelete("{id}")]
        [Authorize(Roles = "Teacher,Admin")]
        public async Task<IActionResult> DeleteAnnouncement(Guid id)
        {
            var result = await _announcementService.DeleteAsync(id);

            if (!result)
            {
                return NotFound(new { message = "Duyuru bulunamadı." });
            }

            return NoContent();
        }

        private Guid GetCurrentUserId()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out var userId))
            {
                throw new UnauthorizedAccessException("Geçersiz kullanıcı.");
            }
            return userId;
        }
    }
}