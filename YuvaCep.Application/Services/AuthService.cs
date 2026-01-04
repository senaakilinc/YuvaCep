using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using YuvaCep.Application.Dtos;
using YuvaCep.Application.DTOs;
using YuvaCep.Application.Helpers;
using YuvaCep.Domain.Entities;
using YuvaCep.Domain.Enums;
using YuvaCep.Persistence.Contexts;

namespace YuvaCep.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly YuvaCepDbContext _context;
        private readonly IConfiguration _configuration;

        public AuthService(YuvaCepDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        // LOGIN
        public async Task<LoginResponse> LoginAsync(LoginRequest request)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.TCIDNumber == request.TCIDNumber);

            if (user == null)
            {
                return new LoginResponse { Token = string.Empty, UserRole = string.Empty, Message = "Kullanıcı bulunamadı." };
            }

            if (!HashingHelper.VerifyPasswordHash(request.Password, Convert.FromBase64String(user.PasswordHash), Convert.FromBase64String(user.PasswordSalt)))
            {
                return new LoginResponse { Token = string.Empty, UserRole = string.Empty, Message = "Şifre hatalı." };
            }

            var token = CreateToken(user);

            string? className = null;
            Guid? classId = null;

            // Eğer giriş yapan Öğretmense, sınıfını bulup gönderelim
            if (user.Role == UserRole.Teacher)
            {
                var teacherClass = await _context.TeacherClasses
                    .Include(tc => tc.Class) // Sınıf ismini almak için Join yap
                    .FirstOrDefaultAsync(tc => tc.TeacherId == user.Id);

                if (teacherClass != null)
                {
                    className = teacherClass.Class.Name;
                    classId = teacherClass.ClassId;
                }
            }

            return new LoginResponse
            {
                Token = token,
                UserRole = user.Role.ToString(),
                Message = "Giriş başarılı.",
                IsSuccess = true,
                Name = user.FirstName,
                Surname = user.LastName,
                ClassName = className,
                ClassId = classId
            };
        }

        // REGISTER PARENT 
        public async Task<LoginResponse> RegisterParentAsync(ParentRegisterRequest request)
        {
            var validation = ValidateRegisterRules(request.TCIDNumber, request.PhoneNumber, request.Password);
            if (!validation.IsValid)
            {
                return new LoginResponse { IsSuccess = false, Message = validation.Message };
            }

            // TC Kontrolü
            if (await _context.Users.AnyAsync(u => u.TCIDNumber == request.TCIDNumber))
            {
                return new LoginResponse { IsSuccess = false, Message = "Bu TC Kimlik numarası zaten kayıtlı." };
            }

            // Şifre Hashleme
            HashingHelper.CreatePasswordHash(request.Password, out string passwordHash, out string passwordSalt);

            // Veli Nesnesi Oluşturma
            var parent = new Parent
            {
                Id = Guid.NewGuid(),
                FirstName = request.Name,
                LastName = request.Surname,
                TCIDNumber = request.TCIDNumber,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt,
                Role = UserRole.Parent,
                IsActive = true,
                PhoneNumber = request.PhoneNumber
            };

            _context.Parents.Add(parent);
            await _context.SaveChangesAsync();

            // Kayıt sonrası otomatik token üret
            var token = CreateToken(parent);

            return new LoginResponse
            {
                IsSuccess = true,
                Token = token,
                UserRole = "Parent",
                Message = "Veli kaydı başarıyla oluşturuldu.",
                Name = parent.FirstName,
                Surname = parent.LastName
            };
        }

        // REGISTER TEACHER 
        public async Task<LoginResponse> RegisterTeacherAsync(TeacherRegisterRequest request)
        {
            var validationTeacher = ValidateRegisterRules(request.TCIDNumber, request.PhoneNumber, request.Password);
            if (!validationTeacher.IsValid)
            {
                return new LoginResponse { IsSuccess = false, Message = validationTeacher.Message };
            }
            // TC Kontrolü
            if (await _context.Users.AnyAsync(u => u.TCIDNumber == request.TCIDNumber))
            {
                return new LoginResponse { IsSuccess = false, Message = "Bu TC Kimlik numarası zaten kayıtlı." };
            }

            // Şifre Hashleme
            HashingHelper.CreatePasswordHash(request.Password, out string passwordHash, out string passwordSalt);

            // Öğretmen Nesnesi Oluşturma
            var teacher = new Teacher
            {
                Id = Guid.NewGuid(),
                FirstName = request.Name,
                LastName = request.Surname,
                TCIDNumber = request.TCIDNumber,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt,
                Role = UserRole.Teacher,
                IsActive = true,
                PhoneNumber = request.PhoneNumber
            };

            _context.Teachers.Add(teacher);
            await _context.SaveChangesAsync();

            // Token üret
            var token = CreateToken(teacher);

            return new LoginResponse
            {
                IsSuccess = true,
                Token = token,
                UserRole = "Teacher",
                Message = "Öğretmen kaydı başarıyla oluşturuldu.",
                Name = teacher.FirstName,
                Surname = teacher.LastName
            };
        }

        // GENEL REGISTER
        public async Task<RegisterResponse> RegisterAsync(RegisterRequest request)
        {
            var existingUserByTC = await _context.Users.AnyAsync(u => u.TCIDNumber == request.TCIDNumber);
            if (existingUserByTC) return new RegisterResponse { Success = false, Message = "Bu TC zaten kayıtlı." };

            return new RegisterResponse { Success = false, Message = "Lütfen özel kayıt endpointlerini kullanın." };
        }

        // LINK CHILD
        public async Task<LinkChildResponse> LinkChildAsync(Guid parentId, LinkStudentRequest request)
        {
            var parent = await _context.Parents.FindAsync(parentId);
            if (parent == null) return new LinkChildResponse { Success = false, Message = "Veli bulunamadı." };

            var student = await _context.Students
                .Include(s => s.Class)
                .FirstOrDefaultAsync(s => s.ReferenceCode == request.ReferenceCode);

            if (student == null) return new LinkChildResponse { Success = false, Message = "Geçersiz referans kodu." };

            var existingLink = await _context.ParentStudents
                .AnyAsync(ps => ps.ParentId == parentId && ps.StudentId == student.Id);

            if (existingLink) return new LinkChildResponse { Success = false, Message = "Bu çocuk zaten ekli." };

            var parentStudent = new ParentStudent { ParentId = parentId, StudentId = student.Id };
            _context.ParentStudents.Add(parentStudent);
            await _context.SaveChangesAsync();

            return new LinkChildResponse
            {
                Success = true,
                Message = "Çocuk başarıyla bağlandı!",
                StudentId = student.Id,
                StudentName = $"{student.Name} {student.Surname}"
            };
        }

        // TOKEN OLUŞTURUCU METODLAR
        private string CreateToken(User user)
        {
            return GenerateJwt(user.Id.ToString(), user.TCIDNumber, user.Role.ToString());
        }

        private string CreateToken(Parent parent)
        {
            return GenerateJwt(parent.Id.ToString(), parent.TCIDNumber, parent.Role.ToString());
        }

        private string CreateToken(Teacher teacher)
        {
            return GenerateJwt(teacher.Id.ToString(), teacher.TCIDNumber, teacher.Role.ToString());
        }

        private string GenerateJwt(string userId, string tcNumber, string role)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, userId),
                new Claim(ClaimTypes.Name, tcNumber),
                new Claim(ClaimTypes.Role, role)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetSection("Jwt:Key").Value));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = creds,
                Issuer = _configuration.GetSection("Jwt:Issuer").Value,
                Audience = _configuration.GetSection("Jwt:Audience").Value
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }

        private (bool IsValid, string Message) ValidateRegisterRules(string tc, string phone, string password)
        {
            if (string.IsNullOrWhiteSpace(tc) || tc.Length != 11 || !long.TryParse(tc, out _))
            {
                return (false, "TC Kimlik Numarası 11 haneli olmalı ve sadece rakamlardan oluşmalıdır.");
            }

            if (string.IsNullOrWhiteSpace(phone) || phone.Length != 11 || !phone.StartsWith("0") || !long.TryParse(phone, out _))
            {
                return (false, "Telefon numarası 11 haneli olmalı ve '0' ile başlamalıdır.");
            }

            if (string.IsNullOrWhiteSpace(password) || password.Length < 8)
            {
                return (false, "Şifre en az 8 karakter olmalıdır.");
            }
            if (!password.Any(char.IsUpper))
            {
                return (false, "Şifre en az 1 tane büyük harf içermelidir.");
            }
            if (!password.Any(ch => !char.IsLetterOrDigit(ch)))
            {
                return (false, "Şifre en az 1 tane özel karakter (nokta, ünlem vb.) içermelidir.");
            }

            return (true, string.Empty);
        }
    }
}