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
    public class DailyReportsController : ControllerBase
    {
        private readonly IDailyReportService _dailyReportService;

        public DailyReportsController(IDailyReportService dailyReportService)
        {
            _dailyReportService = dailyReportService;
        }

        // GET: api/DailyReports
        [HttpGet]
        [Authorize(Roles = "Teacher")]
        public async Task<IActionResult> GetAllReports(
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10)
        {
            var reports = await _dailyReportService.GetAllAsync(pageNumber, pageSize);
            return Ok(reports);
        }

        // GET: api/DailyReports/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetReport(Guid id)
        {
            var report = await _dailyReportService.GetByIdAsync(id);

            if (report == null)
            {
                return NotFound(new { message = "Rapor bulunamadı." });
            }

            return Ok(report);
        }

        // GET: api/DailyReports/student/{studentId}
        [HttpGet("student/{studentId}")]
        public async Task<IActionResult> GetReportsByStudent(
            Guid studentId,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10)
        {
            var reports = await _dailyReportService.GetByStudentIdAsync(studentId, pageNumber, pageSize);
            return Ok(reports);
        }

        // GET: api/DailyReports/student/{studentId}/date-range
        [HttpGet("student/{studentId}/date-range")]
        public async Task<IActionResult> GetReportsByDateRange(
            Guid studentId,
            [FromQuery] DateTime startDate,
            [FromQuery] DateTime endDate)
        {
            var reports = await _dailyReportService.GetByDateRangeAsync(studentId, startDate, endDate);
            return Ok(reports);
        }

        // GET: api/DailyReports/today
        [HttpGet("today")]
        [Authorize(Roles = "Teacher")]
        public async Task<IActionResult> GetTodayReports()
        {
            var reports = await _dailyReportService.GetTodayReportsAsync();
            return Ok(reports);
        }

        // POST: api/DailyReports
        [HttpPost]
        [Authorize(Roles = "Teacher")]
        public async Task<IActionResult> CreateReport([FromBody] CreateDailyReportDto dto)
        {
            // 1. KURAL: Tarih o anın tarihi olsun (Elle girilmesin)
            
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var teacherId = GetCurrentUserId();

            try
            {
                var response = await _dailyReportService.CreateAsync(dto, teacherId);
                return CreatedAtAction(nameof(GetReport), new { id = response.Id }, response);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // PUT: api/DailyReports/{id}
        [HttpPut("{id}")]
        [Authorize(Roles = "Teacher")]
        public async Task<IActionResult> UpdateReport(Guid id, [FromBody] UpdateDailyReportDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var response = await _dailyReportService.UpdateAsync(id, dto);

            if (response == null)
            {
                return NotFound(new { message = "Rapor bulunamadı." });
            }

            return Ok(response);
        }

        // DELETE: api/DailyReports/{id}
        [HttpDelete("{id}")]
        [Authorize(Roles = "Teacher")]
        public async Task<IActionResult> DeleteReport(Guid id)
        {
            var result = await _dailyReportService.DeleteAsync(id);

            if (!result)
            {
                return NotFound(new { message = "Rapor bulunamadı." });
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