using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using YuvaCep.Domain.Entities;
using YuvaCep.Persistence.Contexts;

namespace YuvaCep.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClassesController : ControllerBase
    {
        private readonly YuvaCepDbContext _context;

        public ClassesController(YuvaCepDbContext context)
        {
            _context = context;
        }

        // GET: api/Classes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Class>>> GetClasses()
        {
            return await _context.Classes.ToListAsync();
        }

        // POST: api/Classes
        [HttpPost]
        public async Task<ActionResult<Class>> PostClass(Class newClass)
        {
            _context.Classes.Add(newClass);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetClasses), new { id = newClass.Id }, newClass);
        }
    }
}