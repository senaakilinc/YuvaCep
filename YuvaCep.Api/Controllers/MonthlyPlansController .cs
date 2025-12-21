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
    public class MonthlyPlansController : ControllerBase
    {
        private readonly IMonthlyPlanService _monthlyPlanService;
        // TODO: IFileService eklenecek (fotoğraf yükleme için)

        public MonthlyPlansController(IMonthlyPlanService monthlyPlanService)
        {
            _monthlyPlanService = monthlyPlanService;
        }

        // GET: api/MonthlyPlans
        // Tüm planları getir (Öğretmen için)
        [HttpGet]
        [Authorize(Roles = "Teacher")]
        public async Task<IActionResult> GetAllPlans(
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10)
        {
            var plans = await _monthlyPlanService.GetAllAsync(pageNumber, pageSize);
            return Ok(plans);
        }

        // GET: api/MonthlyPlans/{id}
        // Tek plan detayı
        [HttpGet("{id}")]
        public async Task<IActionResult> GetPlan(Guid id)
        {
            var plan = await _monthlyPlanService.GetByIdAsync(id);

            if (plan == null)
            {
                return NotFound(new { message = "Plan bulunamadı." });
            }

            return Ok(plan);
        }

        // GET: api/MonthlyPlans/class/{classId}
        // Sınıfın belirli ay/yıl planları
        [HttpGet("class/{classId}")]
        public async Task<IActionResult> GetPlansByClass(
            Guid classId,
            [FromQuery] int year,
            [FromQuery] int month)
        {
            var plans = await _monthlyPlanService.GetByClassIdAsync(classId, year, month);
            return Ok(plans);
        }

        // GET: api/MonthlyPlans/class/{classId}/current
        // Sınıfın bu ayın planları
        [HttpGet("class/{classId}/current")]
        public async Task<IActionResult> GetCurrentMonthPlans(Guid classId)
        {
            var plans = await _monthlyPlanService.GetCurrentMonthByClassIdAsync(classId);
            return Ok(plans);
        }

        // GET: api/MonthlyPlans/class/{classId}/type/{planType}
        // Sınıfın belirli tip planları (MenuPlan veya LessonPlan)
        [HttpGet("class/{classId}/type/{planType}")]
        public async Task<IActionResult> GetPlansByType(Guid classId, string planType)
        {
            var plans = await _monthlyPlanService.GetByPlanTypeAsync(classId, planType);
            return Ok(plans);
        }

        // POST: api/MonthlyPlans
        // Yeni plan oluştur (Sadece öğretmen)
        [HttpPost]
        [Authorize(Roles = "Teacher")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> CreatePlan([FromForm] CreateMonthlyPlanDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Fotoğraf kontrolü
            if (dto.ImageFile == null || dto.ImageFile.Length == 0)
            {
                return BadRequest(new { message = "Fotoğraf yüklenmedi." });
            }

            // Dosya boyutu kontrolü (max 10MB)
            if (dto.ImageFile.Length > 10 * 1024 * 1024)
            {
                return BadRequest(new { message = "Fotoğraf boyutu 10MB'dan küçük olmalıdır." });
            }

            // Dosya tipi kontrolü
            var allowedTypes = new[] { "image/jpeg", "image/jpg", "image/png" };
            if (!Array.Exists(allowedTypes, t => t == dto.ImageFile.ContentType))
            {
                return BadRequest(new { message = "Sadece JPG, JPEG ve PNG formatları desteklenmektedir." });
            }

            var teacherId = GetCurrentUserId();

            try
            {
                var response = await _monthlyPlanService.CreateAsync(dto, teacherId);
                return CreatedAtAction(nameof(GetPlan), new { id = response.Id }, response);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // PUT: api/MonthlyPlans/{id}
        // Plan güncelle (Sadece öğretmen)
        [HttpPut("{id}")]
        [Authorize(Roles = "Teacher")]
        public async Task<IActionResult> UpdatePlan(Guid id, [FromBody] UpdateMonthlyPlanDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var response = await _monthlyPlanService.UpdateAsync(id, dto);

            if (response == null)
            {
                return NotFound(new { message = "Plan bulunamadı." });
            }

            return Ok(response);
        }

        // PUT: api/MonthlyPlans/{id}/image
        // Planın fotoğrafını güncelle (Sadece öğretmen)
        [HttpPut("{id}/image")]
        [Authorize(Roles = "Teacher")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> UpdatePlanImage(Guid id, [FromForm] UploadImageDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (dto.ImageFile == null || dto.ImageFile.Length == 0)
            {
                return BadRequest(new { message = "Fotoğraf yüklenmedi." });
            }

            // TODO: Fotoğraf yükleme servisi
            // var imageUrl = await _fileService.UploadImageAsync(dto.ImageFile);
            var imageUrl = "new-temp-image-url.jpg"; // Geçici

            var response = await _monthlyPlanService.UpdateImageAsync(id, imageUrl);

            if (response == null)
            {
                return NotFound(new { message = "Plan bulunamadı." });
            }

            return Ok(response);
        }

        // PATCH: api/MonthlyPlans/{id}/active
        // Planı aktif/pasif yap (Sadece öğretmen)
        [HttpPatch("{id}/active")]
        [Authorize(Roles = "Teacher")]
        public async Task<IActionResult> SetActiveStatus(Guid id, [FromBody] SetActiveStatusDto dto)
        {
            var result = await _monthlyPlanService.SetActiveStatusAsync(id, dto.IsActive);

            if (!result)
            {
                return NotFound(new { message = "Plan bulunamadı." });
            }

            return Ok(new { message = $"Plan {(dto.IsActive ? "aktif" : "pasif")} yapıldı." });
        }

        // DELETE: api/MonthlyPlans/{id}
        // Planı sil (Sadece öğretmen)
        [HttpDelete("{id}")]
        [Authorize(Roles = "Teacher")]
        public async Task<IActionResult> DeletePlan(Guid id)
        {
            var result = await _monthlyPlanService.DeleteAsync(id);

            if (!result)
            {
                return NotFound(new { message = "Plan bulunamadı." });
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

    // Helper DTO
    public class SetActiveStatusDto
    {
        public bool IsActive { get; set; }
    }
}