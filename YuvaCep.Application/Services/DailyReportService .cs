using Microsoft.EntityFrameworkCore;
using YuvaCep.Application.Dtos;
using YuvaCep.Domain.Entities;
using YuvaCep.Persistence.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YuvaCep.Application.Services
{
    public class DailyReportService : IDailyReportService
    {
        private readonly YuvaCepDbContext _context;

        public DailyReportService(YuvaCepDbContext context)
        {
            _context = context;
        }

        public async Task<DailyReportResponseDto> CreateAsync(CreateDailyReportDto request, Guid teacherId)
        {
            // Öğrenci kontrolü
            var studentExists = await _context.Students.AnyAsync(s => s.Id == request.StudentId);
            if (!studentExists)
            {
                throw new Exception("Öğrenci bulunamadı.");
            }

            // Entity oluştur
            var report = new DailyReport
            {
                Id = Guid.NewGuid(),
                StudentId = request.StudentId,
                TeacherId = teacherId,
                CreatedAt = DateTime.UtcNow,

                // Beslenme
                BreakfastNotes = request.BreakfastNotes ?? string.Empty,
                LunchNotes = request.LunchNotes ?? string.Empty,
                SnackNotes = request.SnackNotes ?? string.Empty,
                AteWell = request.AteWell,
                NutritionNotes = request.NutritionNotes ?? string.Empty,

                // Aktivite
                ActivityNotes = request.ActivityNotes ?? string.Empty,
                ActivityType = request.ActivityType ?? string.Empty,

                // Duygu ve Davranış
                MoodStatus = request.MoodStatus ?? string.Empty,
                BehaviorNotes = request.BehaviorNotes ?? string.Empty,
                BehaviorScore = request.BehaviorScore,

                // Uyku ve Hijyen
                NapTaken = request.NapTaken,
                NapDurationMinutes = request.NapDurationMinutes,
                ToiletUsed = request.ToiletUsed,

                // Genel
                GeneralNotes = request.GeneralNotes ?? string.Empty
            };

            _context.DailyReports.Add(report);
            await _context.SaveChangesAsync();

            // Response DTO'ya dönüştür
            return await GetByIdAsync(report.Id);
        }

        public async Task<DailyReportResponseDto> GetByIdAsync(Guid id)
        {
            var report = await _context.DailyReports
                .Include(r => r.Student)
                .Include(r => r.Teacher)
                .FirstOrDefaultAsync(r => r.Id == id);

            if (report == null)
            {
                return null;
            }

            return MapToResponseDto(report);
        }

        public async Task<List<DailyReportResponseDto>> GetAllAsync(int pageNumber = 1, int pageSize = 10)
        {
            var reports = await _context.DailyReports
                .Include(r => r.Student)
                .Include(r => r.Teacher)
                .OrderByDescending(r => r.CreatedAt)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return reports.Select(MapToResponseDto).ToList();
        }

        public async Task<List<DailyReportResponseDto>> GetByStudentIdAsync(Guid studentId, int pageNumber = 1, int pageSize = 10)
        {
            var reports = await _context.DailyReports
                .Include(r => r.Student)
                .Include(r => r.Teacher)
                .Where(r => r.StudentId == studentId)
                .OrderByDescending(r => r.CreatedAt)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return reports.Select(MapToResponseDto).ToList();
        }

        public async Task<List<DailyReportResponseDto>> GetByDateRangeAsync(Guid studentId, DateTime startDate, DateTime endDate)
        {
            var reports = await _context.DailyReports
                .Include(r => r.Student)
                .Include(r => r.Teacher)
                .Where(r => r.StudentId == studentId
                    && r.CreatedAt >= startDate
                    && r.CreatedAt <= endDate)
                .OrderByDescending(r => r.CreatedAt)
                .ToListAsync();

            return reports.Select(MapToResponseDto).ToList();
        }

        public async Task<List<DailyReportResponseDto>> GetTodayReportsAsync()
        {
            var today = DateTime.UtcNow.Date;
            var tomorrow = today.AddDays(1);

            var reports = await _context.DailyReports
                .Include(r => r.Student)
                .Include(r => r.Teacher)
                .Where(r => r.CreatedAt >= today && r.CreatedAt < tomorrow)
                .OrderByDescending(r => r.CreatedAt)
                .ToListAsync();

            return reports.Select(MapToResponseDto).ToList();
        }

        public async Task<DailyReportResponseDto> UpdateAsync(Guid id, UpdateDailyReportDto request)
        {
            var report = await _context.DailyReports.FindAsync(id);
            if (report == null)
            {
                return null;
            }

            // Güncelleme
            report.BreakfastNotes = request.BreakfastNotes ?? report.BreakfastNotes;
            report.LunchNotes = request.LunchNotes ?? report.LunchNotes;
            report.SnackNotes = request.SnackNotes ?? report.SnackNotes;
            report.AteWell = request.AteWell;
            report.ActivityNotes = request.ActivityNotes ?? report.ActivityNotes;
            report.ActivityType = request.ActivityType ?? report.ActivityType;
            report.MoodStatus = request.MoodStatus ?? report.MoodStatus;
            report.BehaviorNotes = request.BehaviorNotes ?? report.BehaviorNotes;
            report.BehaviorScore = request.BehaviorScore;
            report.NapTaken = request.NapTaken;
            report.NapDurationMinutes = request.NapDurationMinutes;
            report.ToiletUsed = request.ToiletUsed;
            report.GeneralNotes = request.GeneralNotes ?? report.GeneralNotes;
            report.NutritionNotes = request.NutritionNotes ?? report.NutritionNotes;
            report.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return await GetByIdAsync(id);
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var report = await _context.DailyReports.FindAsync(id);
            if (report == null)
            {
                return false;
            }

            _context.DailyReports.Remove(report);
            await _context.SaveChangesAsync();

            return true;
        }

        // Helper: Entity → Response DTO dönüşümü
        private DailyReportResponseDto MapToResponseDto(DailyReport report)
        {
            return new DailyReportResponseDto
            {
                Id = report.Id,
                StudentId = report.StudentId,
                StudentName = report.Student != null
                    ? $"{report.Student.Name} {report.Student.Surname}"
                    : "Bilinmiyor",
                TeacherId = report.TeacherId,
                TeacherName = report.Teacher != null
                    ? $"{report.Teacher.Name} {report.Teacher.Surname}"
                    : "Bilinmiyor",
                CreatedAt = report.CreatedAt,
                UpdatedAt = report.UpdatedAt,

                // Beslenme
                BreakfastNotes = report.BreakfastNotes,
                LunchNotes = report.LunchNotes,
                SnackNotes = report.SnackNotes,
                AteWell = report.AteWell,
                NutritionNotes = report.NutritionNotes,

                // Aktivite
                ActivityNotes = report.ActivityNotes,
                ActivityType = report.ActivityType,

                // Duygu ve Davranış
                MoodStatus = report.MoodStatus,
                BehaviorNotes = report.BehaviorNotes,
                BehaviorScore = report.BehaviorScore,

                // Uyku ve Hijyen
                NapTaken = report.NapTaken,
                NapDurationMinutes = report.NapDurationMinutes,
                ToiletUsed = report.ToiletUsed,

                // Genel
                GeneralNotes = report.GeneralNotes
            };
        }
    }
}