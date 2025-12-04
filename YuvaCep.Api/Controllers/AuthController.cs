using Microsoft.AspNetCore.Mvc;
using YuvaCep.Application.Dtos;
using YuvaCep.Application.Services;
using System.Threading.Tasks;

namespace YuvaCep.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var response = await _authService.LoginAsync(request);

            if (response.Token != null)
            {
                // Başarılı giriş: Token ve rolü döndür
                return Ok(response);
            }
            // Başarısız giriş (Hata mesajını döndür)
            return Unauthorized(new { message = response.Message });
        }

        // TODO: FirstRegister (İlk Kayıt) Endpoint'i de buraya eklenecek
    }
}