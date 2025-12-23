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
    public class FeedbacksController : ControllerBase
    {
        private readonly IFeedbackService _feedbackService;

        public FeedbacksController(IFeedbackService feedbackService)
        {
            _feedbackService = feedbackService;
        }

        // GET: api/Feedbacks
        // Tüm geri bildirimleri getir (Öğretmen için)
        [HttpGet]
        [Authorize(Roles = "Teacher")]
        public async Task<IActionResult> GetAllFeedbacks(
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10)
        {
            var feedbacks = await _feedbackService.GetAllAsync(pageNumber, pageSize);
            return Ok(feedbacks);
        }

        // GET: api/Feedbacks/{id}
        // Tek geri bildirim detayı
        [HttpGet("{id}")]
        public async Task<IActionResult> GetFeedback(Guid id)
        {
            var feedback = await _feedbackService.GetByIdAsync(id);

            if (feedback == null)
            {
                return NotFound(new { message = "Geri bildirim bulunamadı." });
            }

            return Ok(feedback);
        }

        // GET: api/Feedbacks/my-feedbacks
        // Velinin kendi geri bildirimleri
        [HttpGet("my-feedbacks")]
        [Authorize(Roles = "Parent")]
        public async Task<IActionResult> GetMyFeedbacks(
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10)
        {
            var parentId = GetCurrentUserId();
            var feedbacks = await _feedbackService.GetByParentIdAsync(parentId, pageNumber, pageSize);
            return Ok(feedbacks);
        }

        // GET: api/Feedbacks/student/{studentId}
        // Öğrenciye ait geri bildirimler (Öğretmen)
        [HttpGet("student/{studentId}")]
        [Authorize(Roles = "Teacher")]
        public async Task<IActionResult> GetFeedbacksByStudent(
            Guid studentId,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10)
        {
            var feedbacks = await _feedbackService.GetByStudentIdAsync(studentId, pageNumber, pageSize);
            return Ok(feedbacks);
        }

        // GET: api/Feedbacks/priority/{priority}
        // Önceliğe göre geri bildirimler (Öğretmen)
        [HttpGet("priority/{priority}")]
        [Authorize(Roles = "Teacher")]
        public async Task<IActionResult> GetFeedbacksByPriority(
            string priority,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10)
        {
            var feedbacks = await _feedbackService.GetByPriorityAsync(priority, pageNumber, pageSize);
            return Ok(feedbacks);
        }

        // GET: api/Feedbacks/unanswered
        // Yanıtlanmamış geri bildirimler (Öğretmen)
        [HttpGet("unanswered")]
        [Authorize(Roles = "Teacher")]
        public async Task<IActionResult> GetUnansweredFeedbacks(
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10)
        {
            var feedbacks = await _feedbackService.GetUnansweredAsync(pageNumber, pageSize);
            return Ok(feedbacks);
        }

        // POST: api/Feedbacks
        // Yeni geri bildirim oluştur (Sadece veli)
        [HttpPost]
        [Authorize(Roles = "Parent")]
        public async Task<IActionResult> CreateFeedback([FromBody] CreateFeedbackDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var parentId = GetCurrentUserId();

            try
            {
                var response = await _feedbackService.CreateAsync(dto, parentId);
                return CreatedAtAction(nameof(GetFeedback), new { id = response.Id }, response);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // POST: api/Feedbacks/{id}/respond
        // Geri bildirime yanıt ver (Sadece öğretmen)
        [HttpPost("{id}/respond")]
        [Authorize(Roles = "Teacher")]
        public async Task<IActionResult> RespondToFeedback(Guid id, [FromBody] RespondFeedbackDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var teacherId = GetCurrentUserId();

            try
            {
                var response = await _feedbackService.RespondAsync(id, dto, teacherId);

                if (response == null)
                {
                    return NotFound(new { message = "Geri bildirim bulunamadı." });
                }

                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // DELETE: api/Feedbacks/{id}
        // Geri bildirimi sil
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFeedback(Guid id)
        {
            var result = await _feedbackService.DeleteAsync(id);

            if (!result)
            {
                return NotFound(new { message = "Geri bildirim bulunamadı." });
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