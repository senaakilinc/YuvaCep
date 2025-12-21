using Microsoft.AspNetCore.Mvc;
using YuvaCep.Application.Dtos;
using YuvaCep.Application.Services;
using System.Threading.Tasks;
using YuvaCep.Application.DTOs;
//using Microsoft.AspNetCore.Identity.Data;

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
                return Ok(response);
            }
            return Unauthorized(new { message = response.Message });
        }

        [HttpPost("first-register")]
        public async Task<IActionResult> FirstRegister([FromBody] FirstRegisterRequest request)
        {
            if (ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var response = await _authService.FirstRegisterAsync(request);

           if (!string.IsNullOrEmpty(response.Token))
            {
                return Ok(response);
            }


            return BadRequest(new { message = response.Message });
        }
    }
}