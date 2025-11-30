using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using YuvaCep.Domain.Entities;
using YuvaCep.Persistence.Contexts;

namespace YuvaCep.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DailyReportsController : ControllerBase
    {
        private readonly YuvaCepDbContext _context;

        public DailyReportsController(YuvaCepDbContext context)
        {
            _context = context;
        }

        // GET: api/DailyReports
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DailyReport>>> GetReports()
        {
            return await _context.DailyReports.ToListAsync();
        }

        // POST: api/DailyReports
        [HttpPost]
        public async Task<ActionResult<DailyReport>> PostReport(DailyReport report)
        {
            // 1. KURAL: Tarih o anın tarihi olsun (Elle girilmesin)
            report.Date = DateTime.UtcNow;

            // 2. KURAL: ChildId ve TeacherId dolu gelmek ZORUNDA.
            // (Zaten Entity yapımızda Guid olduğu için boş gelemez, 
            // boş gelirse "0000..." gelir ve veritabanı "Böyle kayıt yok" der).

            _context.DailyReports.Add(report);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetReports), new { id = report.Id }, report);
        }
    }
}