using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using YuvaCep.Application.Dtos;
using YuvaCep.Domain.Entities; // Class entity'sinin olduğu yer
using YuvaCep.Persistence.Contexts;

namespace YuvaCep.Api.Controllers
{
    [Authorize(Roles = "Teacher")] // Sadece öğretmenler sınıf oluşturabilir
    [Route("api/[controller]")]
    [ApiController]
    public class ClassesController : ControllerBase
    {
        private readonly YuvaCepDbContext _context;

        public ClassesController(YuvaCepDbContext context)
        {
            _context = context;
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateClass([FromBody] CreateClassRequest request)
        {
            // Öğretmenin ID'sini bul
            var teacherIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(teacherIdString)) return Unauthorized();
            var teacherId = Guid.Parse(teacherIdString);

            // Yeni sınıfı oluştur (Classes Tablosu için)
            var newClass = new Class
            {
                Id = Guid.NewGuid(),
                Name = request.ClassName,
                AgeGroup = request.AgeGroup,
                TeacherId = teacherId
            };

            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                // Sınıfı Ekle
                _context.Classes.Add(newClass);
                await _context.SaveChangesAsync();

                // Öğretmen ile Sınıfı Bağla
                var teacherClass = new TeacherClass
                {
                    TeacherId = teacherId,
                    ClassId = newClass.Id
                };
                _context.TeacherClasses.Add(teacherClass);

                await _context.SaveChangesAsync(); // Bağlantıyı kaydet
                await transaction.CommitAsync();   // Onayla

                return Ok(new { message = "Sınıf başarıyla oluşturuldu!", classId = newClass.Id });
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync(); // Hata olursa geri al
                return StatusCode(500, "Sınıf oluşturulurken hata: " + ex.Message);
            }
        }
    }
}