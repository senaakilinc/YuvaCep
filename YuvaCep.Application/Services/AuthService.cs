using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using YuvaCep.Persistence.Contexts;
using YuvaCep.Application.Dtos;
using YuvaCep.Application.Helpers;
using YuvaCep.Domain.Entities;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;

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

        public async Task<LoginResponse> LoginAsync(LoginRequest request)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.TCIDNumber == request.TCIDNumber);

            if (user == null)
            {
                return new LoginResponse {
                    Token = string.Empty,
                    UserRole = string.Empty,
                    Message = "Kullanıcı adı ve şifre hatalı."
                };
            }

            if (!HashingHelper.VerifyPasswordHash(
                request.Password,
                Convert.FromBase64String(user.PasswordHash),
                Convert.FromBase64String(user.PasswordSalt)))
            {
                return new LoginResponse {
                    Token = string.Empty,
                    UserRole = string.Empty,
                    Message = "Kullanıcı adı ve şifre hatalı." 
                };
            }

            var token = CreateToken(user);

            return new LoginResponse
            {
                Token = token,
                UserRole = user.Role.ToString(),
                Message = "Giriş başarılı."
            };
        }

        public async Task<LoginResponse> FirstRegisterAsync(FirstRegisterRequest request)
        {
            // 1. Token (ReferenceCode) ile kullanıcıyı bul
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.ReferenceCode == request.ReferenceCode);

            if (user == null)
            {
                return new LoginResponse
                {
                    Token = string.Empty,
                    UserRole = string.Empty,
                    Message = "Geçersiz kayıt kodu."
                };
            }

            // 2. Hesap zaten aktif mi kontrol et
            if (!string.IsNullOrEmpty(user.PasswordHash))
            {
                return new LoginResponse
                {
                    Token = string.Empty,
                    UserRole = string.Empty,
                    Message = "Bu hesap zaten aktif. Lütfen giriş yapın."
                };
            }

            // 3. TC Kimlik No kontrolü (Email yerine)
            if (user.TCIDNumber != request.TCIDNumber)
            {
                return new LoginResponse
                {
                    Token = string.Empty,
                    UserRole = string.Empty,
                    Message = "TC Kimlik numarası kayıtlı bilgilerle eşleşmiyor."
                };
            }

            // 4. Şifre hash'le ve kullanıcıyı güncelle
            string passwordHash, passwordSalt;
            HashingHelper.CreatePasswordHash(request.Password, out passwordHash, out passwordSalt);

            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;
            user.IsActive = true;

            // 5. Token'ı kullanılmış yap (opsiyonel güvenlik için)
            user.ReferenceCode = null; // veya IsReferenceCodeUsed = true

            await _context.SaveChangesAsync();

            // 6. JWT Token oluştur ve döndür
            var token = CreateToken(user);

            return new LoginResponse
            {
                Token = token,
                UserRole = user.Role.ToString(),
                Message = "Kayıt başarılı. Hoş geldiniz!"
            };
        }

        private string CreateToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var secretKey = _configuration["JwtSettings:Secret"]
                ?? throw new Exception("JWT Secret not configured.");
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Role, user.Role.ToString())
            };

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(
                    Convert.ToInt32(_configuration["JwtSettings:ExpiryMinutes"])),
                SigningCredentials = new SigningCredentials(
                    key, SecurityAlgorithms.HmacSha256Signature),
                Issuer = _configuration["JwtSettings:Issuer"],
                Audience = _configuration["JwtSettings:Audience"]
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}