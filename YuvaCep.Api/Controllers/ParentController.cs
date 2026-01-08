using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using YuvaCep.Api.DTOs;
using YuvaCep.Application.Dtos;
using YuvaCep.Domain.Entities;
using YuvaCep.Persistence.Contexts;

namespace YuvaCep.Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ParentController : ControllerBase
    {
        private readonly YuvaCepDbContext _context;

        public ParentController(YuvaCepDbContext context)
        {
            _context = context;
        }


        private string GetUserRole()
        {

            var role = User.FindFirst(ClaimTypes.Role)?.Value;

            if (string.IsNullOrEmpty(role))
            {
                role = User.FindFirst("role")?.Value;
            }

            return role ?? "Parent";
        }

        [HttpGet("my-profile")]
        public async Task<IActionResult> GetMyProfile()
        {
            var userIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdString)) return Unauthorized();
            var userId = Guid.Parse(userIdString);

            var role = GetUserRole();

            string name = "";
            string surname = "";
            string phoneNumber = "";

            if (role == "Teacher")
            {
                var teacher = await _context.Teachers.FirstOrDefaultAsync(t => t.Id == userId);
                if (teacher == null) return NotFound("Öğretmen profili bulunamadı.");

                name = teacher.FirstName;
                surname = teacher.LastName;
                phoneNumber = teacher.PhoneNumber;
            }
            else
            {
                var parent = await _context.Parents.FirstOrDefaultAsync(p => p.Id == userId);
                if (parent == null) return NotFound("Veli profili bulunamadı.");

                name = parent.FirstName;
                surname = parent.LastName;
                phoneNumber = parent.PhoneNumber;
            }

            var profileData = new
            {
                Name = name,
                Surname = surname,
                PhoneNumber = phoneNumber
            };

            return Ok(profileData);
        }

        [HttpPost("update-contact")]
        public async Task<IActionResult> UpdateContact([FromBody] UpdateParentContactDto model)
        {
            var userIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdString)) return Unauthorized();
            var userId = Guid.Parse(userIdString);

            var role = GetUserRole();

            if (role == "Teacher")
            {
                var teacher = await _context.Teachers.FirstOrDefaultAsync(t => t.Id == userId);
                if (teacher == null) return NotFound("Öğretmen bulunamadı.");

                teacher.PhoneNumber = model.PhoneNumber;
                _context.Teachers.Update(teacher);
            }
            else
            {
                var parent = await _context.Parents.FirstOrDefaultAsync(p => p.Id == userId);
                if (parent == null) return NotFound("Veli bulunamadı.");

                parent.PhoneNumber = model.PhoneNumber;
                _context.Parents.Update(parent);
            }

            await _context.SaveChangesAsync();

            return Ok(new { message = "İletişim bilgileri başarıyla güncellendi." });
        }

        [HttpPost("change-password")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDto model)
        {
            var userIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdString)) return Unauthorized();
            var userId = Guid.Parse(userIdString);

            var role = GetUserRole();

            if (role == "Teacher")
            {
                var teacher = await _context.Teachers.FirstOrDefaultAsync(t => t.Id == userId);
                if (teacher == null) return NotFound();

                if (!VerifyPasswordHash(model.CurrentPassword, teacher.PasswordHash, teacher.PasswordSalt))
                    return BadRequest(new { message = "Mevcut şifreniz hatalı." });

                CreatePasswordHash(model.NewPassword, out string newHash, out string newSalt);
                teacher.PasswordHash = newHash;
                teacher.PasswordSalt = newSalt;
                _context.Teachers.Update(teacher);
            }
            else
            {
                var parent = await _context.Parents.FirstOrDefaultAsync(p => p.Id == userId);
                if (parent == null) return NotFound();

                if (!VerifyPasswordHash(model.CurrentPassword, parent.PasswordHash, parent.PasswordSalt))
                    return BadRequest(new { message = "Mevcut şifreniz hatalı." });

                CreatePasswordHash(model.NewPassword, out string newHash, out string newSalt);
                parent.PasswordHash = newHash;
                parent.PasswordSalt = newSalt;
                _context.Parents.Update(parent);
            }

            await _context.SaveChangesAsync();
            return Ok(new { message = "Şifreniz başarıyla değiştirildi." });
        }

        [HttpGet("my-student")]
        public async Task<IActionResult> GetMyStudent()
        {
            var userIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdString)) return Unauthorized();

            var userId = Guid.Parse(userIdString);

            var parent = await _context.Parents
                .Include(p => p.ParentStudents)
                .ThenInclude(ps => ps.Student)
                .ThenInclude(s => s.Class)
                .FirstOrDefaultAsync(p => p.Id == userId);

            if (parent == null) return NotFound("Veli bulunamadı.");

            var studentEntity = parent.ParentStudents.Select(ps => ps.Student).FirstOrDefault();

            if (studentEntity == null) return Ok(null);

            var studentDto = new StudentDto
            {
                Id = studentEntity.Id,
                Name = studentEntity.Name,
                Surname = studentEntity.Surname,
                ClassId = studentEntity.ClassId,
                ClassName = studentEntity.Class != null ? studentEntity.Class.Name : "Sınıfı Yok",
            };

            return Ok(studentDto);
        }

        [HttpPost("link-student")]
        public async Task<IActionResult> LinkStudent([FromBody] LinkStudentRequest request)
        {
            var userIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdString)) return Unauthorized();
            var parentId = Guid.Parse(userIdString);

            var student = await _context.Students.FirstOrDefaultAsync(s => s.ReferenceCode == request.ReferenceCode);
            if (student == null) return NotFound(new { message = "Geçersiz kod!" });

            var exists = await _context.ParentStudents.AnyAsync(ps => ps.ParentId == parentId && ps.StudentId == student.Id);
            if (exists) return BadRequest(new { message = "Bu öğrenci zaten ekli." });

            var link = new ParentStudent { ParentId = parentId, StudentId = student.Id };

            _context.ParentStudents.Add(link);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Eşleşme başarılı!" });
        }

        private bool VerifyPasswordHash(string password, string storedHash, string storedSalt)
        {
            try
            {
                var saltBytes = Convert.FromBase64String(storedSalt);
                var hashBytes = Convert.FromBase64String(storedHash);

                using (var hmac = new HMACSHA512(saltBytes))
                {
                    var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
                    for (int i = 0; i < computedHash.Length; i++)
                    {
                        if (computedHash[i] != hashBytes[i]) return false;
                    }
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        private void CreatePasswordHash(string password, out string passwordHash, out string passwordSalt)
        {
            using (var hmac = new HMACSHA512())
            {
                passwordSalt = Convert.ToBase64String(hmac.Key);
                passwordHash = Convert.ToBase64String(hmac.ComputeHash(Encoding.UTF8.GetBytes(password)));
            }
        }
    }
}