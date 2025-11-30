using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using YuvaCep.Domain.Entities;
using YuvaCep.Persistence.Contexts;

namespace YuvaCep.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChildrenController : ControllerBase
    {
        private readonly YuvaCepDbContext _context;

        public ChildrenController(YuvaCepDbContext context)
        {
            _context = context;
        }

        // GET: api/Children
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Child>>> GetChildren()
        {
            // Sınıf bilgisiyle beraber getir (Include)
            return await _context.Children
                                 .Include(c => c.Class)
                                 .ToListAsync();
        }

        // POST: api/Children
        [HttpPost]
        public async Task<ActionResult<Child>> PostChild(Child child)
        {
            // Burada Child nesnesi gelir.
            // ClassId ZORUNLUDUR (Veritabanı kuralı).
            // Class nesnesi ise [JsonIgnore] olduğu için API istemez.

            _context.Children.Add(child);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetChildren), new { id = child.Id }, child);
        }
    }
}