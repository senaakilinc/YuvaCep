using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using YuvaCep.Application.Dtos;
using YuvaCep.Application.DTOs;
using YuvaCep.Domain.Entities;
using YuvaCep.Persistence.Contexts;

namespace YuvaCep.Api.Controllers
{
    public class CreateStudentRequest
    {
        public string Name { get; set; }
        public string Surname { get; set; }
    }
    public class LinkStudentRequest
    {
        public string ReferenceCode { get; set; }
    }

    [Route("api/[controller]")]
    [ApiController]
    public class StudentsController : ControllerBase
    {
        private readonly YuvaCepDbContext _context;

        public StudentsController(YuvaCepDbContext context)
        {
            _context = context;
        }

        // SADECE ÖĞRETMENLER GİREBİLİR
        [Authorize(Roles = "Teacher")]
        [HttpGet("my-class-students")]
        public async Task<IActionResult> GetMyStudents()
        {
            try
            {
                var userIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userIdString)) return Unauthorized();

                var teacherId = Guid.Parse(userIdString);

                var classId = await _context.Classes
                    .Where(c => c.TeacherId == teacherId)
                    .Select(c => c.Id)
                    .FirstOrDefaultAsync();

                if (classId == Guid.Empty) return NotFound("Henüz bir sınıfınız yok.");

                var students = await _context.Students
                    .Where(s => s.ClassId == classId)
                    .Select(s => new
                    {
                        s.Id,
                        s.Name,
                        s.Surname,
                        s.ReferenceCode
                    })
                    .ToListAsync();

                if (!students.Any()) return Ok(new List<object>());

                var studentIds = students.Select(s => s.Id).ToList();

                var links = await _context.ParentStudents
                    .Where(ps => studentIds.Contains(ps.StudentId))
                    .Select(ps => new
                    {
                        ps.StudentId,
                        ps.ParentId
                    })
                    .ToListAsync();

                var parentIds = links.Select(l => l.ParentId).Distinct().ToList();

                var parentNames = await _context.Users
                    .Where(u => parentIds.Contains(u.Id))
                    .Select(u => new
                    {
                        u.Id,
                        FullName = u.FirstName + " " + u.LastName
                    })
                    .ToDictionaryAsync(k => k.Id, v => v.FullName);

                var result = students.Select(s =>
                {
                    var link = links.FirstOrDefault(l => l.StudentId == s.Id);
                    string parentNameStr = "Henüz Eşleşmedi";

                    if (link != null && parentNames.ContainsKey(link.ParentId))
                    {
                        parentNameStr = parentNames[link.ParentId];
                    }

                    return new
                    {
                        Id = s.Id,
                        Name = s.Name + " " + (s.Surname ?? ""),
                        ReferenceCode = s.ReferenceCode,
                        ParentName = parentNameStr
                    };
                }).ToList();

                return Ok(result);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"KRİTİK HATA: {ex.Message}");
                return StatusCode(500, $"Sunucu Hatası: {ex.Message}");
            }
        }

        // SADECE ÖĞRETMENLER GİREBİLİR 
        [Authorize(Roles = "Teacher")]
        [HttpPost("add")]
        public async Task<IActionResult> AddStudent([FromBody] CreateStudentRequest request)
        {
            if (request == null || string.IsNullOrWhiteSpace(request.Name))
                return BadRequest("Öğrenci adı boş olamaz.");

            var userIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdString)) return Unauthorized();

            var teacherId = Guid.Parse(userIdString);

            var teacherClass = await _context.Classes.FirstOrDefaultAsync(c => c.TeacherId == teacherId);

            if (teacherClass == null) return NotFound("Önce bir sınıf oluşturmalısınız.");

            string refCode = GenerateReferenceCode();
            while (await _context.Students.AnyAsync(s => s.ReferenceCode == refCode))
            {
                refCode = GenerateReferenceCode();
            }

            var newStudent = new Student
            {
                Id = Guid.NewGuid(),
                Name = request.Name.Trim(),
                Surname = request.Surname.Trim(),
                ClassId = teacherClass.Id,
                ReferenceCode = refCode
            };

            _context.Students.Add(newStudent);
            await _context.SaveChangesAsync();
            return Ok(new { message = "Öğrenci eklendi.", data = newStudent });
        }

        private string GenerateReferenceCode()
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var random = new Random();
            return new string(Enumerable.Repeat(chars, 6)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }


        // SADECE VELİLER GİREBİLİR 
        [Authorize(Roles = "Parent")]
        [HttpPost("link-student")]
        public async Task<IActionResult> LinkStudent([FromBody] LinkStudentRequest request)
        {

            if (string.IsNullOrWhiteSpace(request.ReferenceCode))
                return BadRequest("Kod boş olamaz.");

            var cleanCode = request.ReferenceCode.Trim().ToUpper();

            // Öğrenciyi bul
            var student = await _context.Students
                .FirstOrDefaultAsync(s => s.ReferenceCode == cleanCode);

            if (student == null)
                return NotFound($"'{cleanCode}' koduna sahip öğrenci bulunamadı.");

            // Veli ID'sini al
            var parentIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(parentIdString)) return Unauthorized();
            var parentId = Guid.Parse(parentIdString);

            // Zaten ekli mi?
            var alreadyLinked = await _context.ParentStudents
                .AnyAsync(ps => ps.ParentId == parentId && ps.StudentId == student.Id);

            if (alreadyLinked)
                return BadRequest("Bu öğrenci zaten listenizde ekli.");

            // Eşleştir
            var parentStudent = new ParentStudent
            {
                ParentId = parentId,
                StudentId = student.Id
            };

            _context.ParentStudents.Add(parentStudent);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Öğrenci başarıyla eklendi!", studentName = student.Name });
        }

        // SADECE VELİLER GİREBİLİR
        [Authorize(Roles = "Parent")]
        [HttpGet("my-children")]
        public async Task<IActionResult> GetMyChildren()
        {
            var parentId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

            var children = await _context.ParentStudents
                .Where(ps => ps.ParentId == parentId)
                .Select(ps => new
                {
                    Id = ps.Student.Id,
                    Name = ps.Student.Name + " " + (ps.Student.Surname ?? ""),
                    ClassName = ps.Student.Class.Name,
                    ReferenceCode = ps.Student.ReferenceCode
                })
                .ToListAsync();

            return Ok(children);
        }

        [Authorize(Roles = "Teacher,Parent")] 
        [HttpGet("{id}/detail")]
        public async Task<IActionResult> GetStudentDetail(Guid id)
        {
            var student = await _context.Students
                .Include(s => s.Class)
                .FirstOrDefaultAsync(s => s.Id == id);

            if (student == null) return NotFound("Öğrenci bulunamadı.");

            var parentLink = await _context.ParentStudents
                .Include(ps => ps.Parent)
                .FirstOrDefaultAsync(ps => ps.StudentId == id);

            string parentFullName = parentLink?.Parent != null
                ? $"{parentLink.Parent.FirstName} {parentLink.Parent.LastName}"
                : "Atanmış Veli Yok";

            var dto = new StudentDetailDto
            {
                Id = student.Id,
                FirstName = student.Name,
                LastName = student.Surname,
                Name = $"{student.Name} {student.Surname}",
                PhotoUrl = null,
                ParentName = parentFullName,
                ClassName = student.Class?.Name ?? "Sınıf Bilgisi Yok",
                Gender = student.Gender,
                TCIDNumber = student.TCIDNumber,
                DateOfBirth = student.DateOfBirth,
                HealthNotes = student.HealthNotes
            };

            return Ok(dto);
        }

        [Authorize(Roles = "Teacher")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateStudent(Guid id, [FromBody] StudentUpdateDto model)
        {
            var student = await _context.Students.FindAsync(id);
            if (student == null) return NotFound();

            student.Name = model.FirstName;
            student.Surname = model.LastName;
            student.Gender = model.Gender;
            student.TCIDNumber = model.TCIDNumber;
            student.DateOfBirth = model.DateOfBirth;

            if (!string.IsNullOrEmpty(model.PhotoBase64))
            {
                // Burada Base64'ü dosyaya çevirip kaydetme kodu olmalı
                // Şimdilik DB'ye string olarak veya URL olarak bastığını varsayıyorum
                // student.PhotoUrl = SaveImage(model.PhotoBase64); 
            }

            await _context.SaveChangesAsync();
            return Ok(new { message = "Bilgiler güncellendi." });
        }
    }
}