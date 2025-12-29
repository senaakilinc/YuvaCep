using Microsoft.AspNetCore.Mvc;
using YuvaCep.Application.Dtos;
using YuvaCep.Application.Services;
using System.Threading.Tasks;
using YuvaCep.Application.DTOs;

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

            if (response != null && !string.IsNullOrEmpty(response.Token))
            {
                return Ok(response);
            }

            return Unauthorized(new { message = "Giriş Başarısız" });
        }

        [HttpPost("register/parent")]
        public async Task<IActionResult> RegisterParent([FromBody] ParentRegisterRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var response = await _authService.RegisterParentAsync(request);

            if (!string.IsNullOrEmpty(response.Token))
            {
                return Ok(response);
            }

            return BadRequest(new { message = response.Message });
        }

        [HttpPost("register/teacher")]
        public async Task<IActionResult> RegisterTeacher([FromBody] TeacherRegisterRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var response = await _authService.RegisterTeacherAsync(request);

            if (!string.IsNullOrEmpty(response.Token))
            {
                return Ok(response);
            }

            return BadRequest(new { message = response.Message });
        }
    }
}