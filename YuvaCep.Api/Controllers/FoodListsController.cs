using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using YuvaCep.Application.Dtos;
using YuvaCep.Domain.Entities;
using YuvaCep.Persistence.Contexts;

namespace YuvaCep.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FoodListsController : ControllerBase
    {
        private readonly YuvaCepDbContext _context;

        public FoodListsController(YuvaCepDbContext context)
        {
            _context = context;
        }

        [Authorize(Roles = "Teacher")]
        [HttpPost]
        public async Task<IActionResult> Upload([FromBody] CreateFoodListDto request)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null) return Unauthorized();

            var teacherClass = await _context.Classes.FirstOrDefaultAsync(c => c.TeacherId == Guid.Parse(userId));
            if (teacherClass == null) return BadRequest("Sınıfınız bulunamadı.");

            var existing = await _context.FoodLists
                .FirstOrDefaultAsync(f => f.ClassId == teacherClass.Id && f.Month == request.Date.Month && f.Year == request.Date.Year);

            if (existing != null)
            {
                existing.ImageBase64 = request.ImageBase64;
                existing.CreatedAt = DateTime.UtcNow;
            }
            else
            {
                var newList = new FoodList
                {
                    Id = Guid.NewGuid(),
                    ClassId = teacherClass.Id,
                    Month = request.Date.Month,
                    Year = request.Date.Year,
                    ImageBase64 = request.ImageBase64,
                    CreatedAt = DateTime.UtcNow
                };
                _context.FoodLists.Add(newList);
            }

            await _context.SaveChangesAsync();
            return Ok(new { message = "Yemek listesi güncellendi." });
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetByDate([FromQuery] int month, [FromQuery] int year, [FromQuery] Guid? studentId)
        {
            Guid classId = Guid.Empty;

            if (User.IsInRole("Teacher"))
            {
                var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
                var cls = await _context.Classes.FirstOrDefaultAsync(c => c.TeacherId == userId);
                if (cls != null) classId = cls.Id;
            }

            else if (studentId.HasValue)
            {
                var student = await _context.Students.FindAsync(studentId.Value);
                if (student != null) classId = student.ClassId;
            }

            if (classId == Guid.Empty) return NotFound("Sınıf bilgisi bulunamadı.");

            var foodList = await _context.FoodLists
                .FirstOrDefaultAsync(f => f.ClassId == classId && f.Month == month && f.Year == year);

            if (foodList == null) return Ok(null); 

            return Ok(new FoodListDto
            {
                Id = foodList.Id,
                Month = foodList.Month,
                Year = foodList.Year,
                ImageBase64 = foodList.ImageBase64
            });
        }
    }
}