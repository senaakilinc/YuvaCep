using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using YuvaCep.Domain.Entities;
using YuvaCep.Domain.Enums;
using YuvaCep.Persistence.Contexts;

namespace YuvaCep.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProgramsController : ControllerBase
    {
        private readonly YuvaCepDbContext _context;
        private readonly IWebHostEnvironment _env;

        public ProgramsController(YuvaCepDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        // GET: api/Programs?classId=...
        // BURAYI DEĞİŞTİRDİK: Artık sınıf ID'sine göre filtreliyor!
        [HttpGet]
        public async Task<ActionResult<IEnumerable<SchoolProgram>>> GetPrograms([FromQuery] Guid? classId)
        {
            var query = _context.SchoolPrograms.AsQueryable();

            if (classId.HasValue)
            {
                // Veli bir sınıf ID'si gönderdiyse:
                // Sadece O Sınıfa ait olanları YA DA Tüm Okula (null) ait olanları getir.
                query = query.Where(p => p.ClassId == classId || p.ClassId == null);
            }

            return await query.OrderByDescending(p => p.Date).ToListAsync();
        }

        // POST: api/Programs (RESİM YÜKLEME - AYNI KALDI)
        [HttpPost]
        public async Task<ActionResult<SchoolProgram>> PostProgram(
            [FromForm] string title,
            [FromForm] DateTime date,
            [FromForm] ProgramType type,
            [FromForm] Guid? classId, // Hangi sınıfa ait olduğu buradan gelir
            IFormFile imageFile)
        {
            if (imageFile == null || imageFile.Length == 0)
                return BadRequest("Lütfen bir resim yükleyin.");

            // 1. Dosya ismini oluştur
            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(imageFile.FileName);

            // 2. Klasörü bul (yoksa yarat)
            var uploadsFolder = Path.Combine(_env.WebRootPath, "uploads");
            if (!Directory.Exists(uploadsFolder)) Directory.CreateDirectory(uploadsFolder);

            var filePath = Path.Combine(uploadsFolder, fileName);

            // 3. Kaydet
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await imageFile.CopyToAsync(stream);
            }

            // 4. Veritabanına Yaz
            var program = new SchoolProgram
            {
                Title = title,
                Date = date.ToUniversalTime(),
                Type = type,
                ClassId = classId, // <-- İşte burası sınıfı eşleştiriyor!
                ImagePath = "/uploads/" + fileName
            };

            _context.SchoolPrograms.Add(program);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetPrograms), new { id = program.Id }, program);
        }
    }
}