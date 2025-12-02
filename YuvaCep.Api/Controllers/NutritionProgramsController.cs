using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using YuvaCep.Domain.Entities;
using YuvaCep.Persistence.Contexts;

namespace YuvaCep.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NutritionProgramsController : ControllerBase
    {
        private readonly YuvaCepDbContext _context;
        private readonly IWebHostEnvironment _env;

        public NutritionProgramsController(YuvaCepDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        // GET: api/NutritionPrograms?classId=...
        // VELİ: Sadece sınıfının ID'sini yollayacak.
        // SONUÇ: O sınıfa ait tüm listeler (Eski-Yeni hepsi) başlıklarıyla gelir.
        [HttpGet]
        public async Task<ActionResult<IEnumerable<NutritionProgram>>> GetPrograms([FromQuery] Guid classId)
        {
            return await _context.NutritionPrograms
                                 .Where(p => p.ClassId == classId)
                                 .OrderByDescending(p => p.CreatedAt) // En yeni yüklenen en üstte
                                 .ToListAsync();
        }

        // POST: api/NutritionPrograms
        // ÖĞRETMEN: Sadece Başlık ve Resim yükleyecek.
        [HttpPost]
        public async Task<ActionResult<NutritionProgram>> PostProgram(
            [FromForm] string title, // Örn: "Mart 2026 Menüsü"
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

            var program = new NutritionProgram
            {
                Title = title,
                ClassId = classId,
                ImagePath = "/uploads/" + fileName,
                CreatedAt = DateTime.UtcNow
            };

            _context.NutritionPrograms.Add(program);
            await _context.SaveChangesAsync();

            return Ok(program);
        }
    }
}