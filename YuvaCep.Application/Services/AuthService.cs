using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using YuvaCep.Persistence.Contexts;
using YuvaCep.Application.Dtos;
using YuvaCep.Application.Helpers;
using YuvaCep.Domain.Entities;

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
                return new LoginResponse { Message = "Kullanıcı adı ve şifre hatalı." };
            }

            if (!HashingHelper.VerifyPasswordHash(
                request.Password,
                Convert.FromBase64String(user.PasswordHash),
                Convert.FromBase64String(user.PasswordSalt)))
            {
                return new LoginResponse { Message = "Kullanıcı adı ve şifre hatalı." };
            }

            var token = CreateToken(user);

            return new LoginResponse
            {
                Token = token,
                UserRole = user.Role.ToString(),
                Message = "Giriş başarılı."
            };
        }

        public async Task<LoginResponse> FirstRegisterAsync(RegisterRequest request)
        {
            throw new NotImplementedException();
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