using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using YuvaCep.Api.DTOs;
using YuvaCep.Application.Dtos;
using YuvaCep.Domain.Entities;
using YuvaCep.Persistence.Contexts;

namespace YuvaCep.Api.Controllers
{
    [Authorize] // Sadece giriş yapmış kişiler erişebilir
    [Route("api/[controller]")]
    [ApiController]
    public class ParentController : ControllerBase
    {
        private readonly YuvaCepDbContext _context;

        public ParentController(YuvaCepDbContext context)
        {
            _context = context;
        }

        [HttpGet("my-student")]
        public async Task<IActionResult> GetMyStudent()
        {
            // 1. Token'dan "Ben Kimim?" sorusunun cevabını (User ID) alıyoruz
            var userIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdString)) return Unauthorized();

            var userId = Guid.Parse(userIdString);

            // 2. Veritabanından bu veliyi ve bağlı olduğu öğrenciyi buluyoruz
            var parent = await _context.Parents
                .Include(p => p.ParentStudents)
                .ThenInclude(ps => ps.Student)
                .ThenInclude(s => s.Class) // Sınıf adını da alalım
                .FirstOrDefaultAsync(p => p.Id == userId);

            if (parent == null) return NotFound("Veli bulunamadı.");

            // 3. Veliye bağlı öğrenciyi alıyor 
            var studentEntity = parent.ParentStudents.Select(ps => ps.Student).FirstOrDefault();

            if (studentEntity == null)
            {
                // Öğrenci yoksa boş döneriz, mobil taraf bunu "Henüz eklemediniz" diye anlar.
                return Ok(null);
            }

            // 4. Öğrenci bilgilerini DTO'ya çevirip yolluyoruz
            var studentDto = new StudentDto
            {
                Id = studentEntity.Id,
                Name = studentEntity.Name,
                ClassName = studentEntity.Class != null ? studentEntity.Class.Name : "Sınıfı Yok",
                // ImagePath = studentEntity.ImagePath // Fotoğraf alanı varsa
            };

            return Ok(studentDto);
        }

        [HttpPost("link-student")]
        public async Task<IActionResult> LinkStudent([FromBody] LinkStudentRequest request)
        {
            // 1. Veli kim?
            var userIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdString)) return Unauthorized();
            var parentId = Guid.Parse(userIdString);

            // 2. Bu koda sahip öğrenci var mı?
            var student = await _context.Students.FirstOrDefaultAsync(s => s.ReferenceCode == request.ReferenceCode);
            if (student == null)
                return NotFound(new { message = "Geçersiz kod! Lütfen kodu kontrol ediniz." });

            // 3. Zaten eşleşmiş mi?
            var exists = await _context.ParentStudent.AnyAsync(ps => ps.ParentId == parentId && ps.StudentId == student.Id);
            if (exists)
                return BadRequest(new { message = "Bu öğrenci zaten ekli." });

            // 4. Eşleştirmeyi yap
            var link = new ParentStudent
            {
                ParentId = parentId,
                StudentId = student.Id
            };

            _context.ParentStudent.Add(link);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Eşleşme başarılı!" });
        }
    }
}