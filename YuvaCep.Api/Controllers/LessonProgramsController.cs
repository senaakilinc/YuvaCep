using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using YuvaCep.Domain.Entities;
using YuvaCep.Persistence.Contexts;

namespace YuvaCep.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LessonProgramsController : ControllerBase
    {
        private readonly YuvaCepDbContext _context;
        private readonly IWebHostEnvironment _env;

        public LessonProgramsController(YuvaCepDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        // GET: api/LessonPrograms?classId=...
        [HttpGet]
        public async Task<ActionResult<IEnumerable<LessonProgram>>> GetPrograms([FromQuery] Guid classId)
        {
            return await _context.LessonPrograms
                                 .Where(p => p.ClassId == classId)
                                 .OrderByDescending(p => p.CreatedAt)
                                 .ToListAsync();
        }

        // POST: api/LessonPrograms
        [HttpPost]
        public async Task<ActionResult<LessonProgram>> PostProgram(
            [FromForm] string title,
            [FromForm] Guid classId,
            IFormFile imageFile)
        {
            if (imageFile == null || imageFile.Length == 0) return BadRequest("Resim yok.");

            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(imageFile.FileName);
            var uploadsFolder = Path.Combine(_env.WebRootPath, "uploads");
            if (!Directory.Exists(uploadsFolder)) Directory.CreateDirectory(uploadsFolder);

            using (var stream = new FileStream(Path.Combine(uploadsFolder, fileName), FileMode.Create))
            {
                await imageFile.CopyToAsync(stream);
            }

            var program = new LessonProgram
            {
                Title = title,
                ClassId = classId,
                ImagePath = "/uploads/" + fileName,
                CreatedAt = DateTime.UtcNow
            };

            _context.LessonPrograms.Add(program);
            await _context.SaveChangesAsync();

            return Ok(program);
        }
    }
}