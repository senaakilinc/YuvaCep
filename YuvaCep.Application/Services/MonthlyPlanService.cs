using Microsoft.EntityFrameworkCore;
using YuvaCep.Application.Dtos;
using YuvaCep.Domain.Entities;
using YuvaCep.Persistence.Contexts;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace YuvaCep.Application.Services
{
    public class MonthlyPlanService : IMonthlyPlanService
    {
        private readonly YuvaCepDbContext _context;
        // TODO: IFileService eklenecek (fotoğraf yükleme için)

        public MonthlyPlanService(YuvaCepDbContext context)
        {
            _context = context;
        }

        public async Task<MonthlyPlanResponseDto> CreateAsync(CreateMonthlyPlanDto request, Guid teacherId)
        {
            // Sınıf kontrolü
            var classExists = await _context.Classes.AnyAsync(c => c.Id == request.ClassId);
            if (!classExists)
            {
                throw new Exception("Sınıf bulunamadı.");
            }

            // Aynı dönem için aynı tip plan var mı kontrol et
            var existingPlan = await _context.MonthlyPlans
                .AnyAsync(p => p.ClassId == request.ClassId
                    && p.Year == request.Year
                    && p.Month == request.Month
                    && p.PlanType == request.PlanType
                    && p.IsActive);

            if (existingPlan)
            {
                throw new Exception($"{GetMonthName(request.Month)} {request.Year} için bu tip plan zaten mevcut.");
            }

            // TODO: Fotoğraf yükleme işlemi
            // var imageUrl = await _fileService.UploadImageAsync(request.ImageFile);
            var imageUrl = "temp-image-url.jpg"; // Geçici

            // Entity oluştur
            var plan = new MonthlyPlan
            {
                Id = Guid.NewGuid(),
                ClassId = request.ClassId,
                Year = request.Year,
                Month = request.Month,
                PlanType = request.PlanType,
                Title = request.Title,
                ImageUrl = imageUrl,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };

            _context.MonthlyPlans.Add(plan);
            await _context.SaveChangesAsync();

            return await GetByIdAsync(plan.Id);
        }

        public async Task<MonthlyPlanResponseDto> GetByIdAsync(Guid id)
        {
            var plan = await _context.MonthlyPlans
                .Include(p => p.Class)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (plan == null)
            {
                return null;
            }

            return MapToResponseDto(plan);
        }

        public async Task<List<MonthlyPlanResponseDto>> GetAllAsync(int pageNumber = 1, int pageSize = 10)
        {
            var plans = await _context.MonthlyPlans
                .Include(p => p.Class)
                .Where(p => p.IsActive)
                .OrderByDescending(p => p.Year)
                .ThenByDescending(p => p.Month)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return plans.Select(MapToResponseDto).ToList();
        }

        public async Task<List<MonthlyPlanResponseDto>> GetByClassIdAsync(Guid classId, int year, int month)
        {
            var plans = await _context.MonthlyPlans
                .Include(p => p.Class)
                .Where(p => p.ClassId == classId
                    && p.Year == year
                    && p.Month == month
                    && p.IsActive)
                .OrderBy(p => p.PlanType)
                .ToListAsync();

            return plans.Select(MapToResponseDto).ToList();
        }

        public async Task<List<MonthlyPlanResponseDto>> GetCurrentMonthByClassIdAsync(Guid classId)
        {
            var now = DateTime.UtcNow;
            return await GetByClassIdAsync(classId, now.Year, now.Month);
        }

        public async Task<List<MonthlyPlanResponseDto>> GetByPlanTypeAsync(Guid classId, string planType)
        {
            var plans = await _context.MonthlyPlans
                .Include(p => p.Class)
                .Where(p => p.ClassId == classId
                    && p.PlanType == planType
                    && p.IsActive)
                .OrderByDescending(p => p.Year)
                .ThenByDescending(p => p.Month)
                .ToListAsync();

            return plans.Select(MapToResponseDto).ToList();
        }

        public async Task<MonthlyPlanResponseDto> UpdateAsync(Guid id, UpdateMonthlyPlanDto request)
        {
            var plan = await _context.MonthlyPlans.FindAsync(id);
            if (plan == null)
            {
                return null;
            }

            // Güncelleme
            if (request.Year.HasValue)
                plan.Year = request.Year.Value;

            if (request.Month.HasValue)
                plan.Month = request.Month.Value;

            if (!string.IsNullOrEmpty(request.PlanType))
                plan.PlanType = request.PlanType;

            if (!string.IsNullOrEmpty(request.Title))
                plan.Title = request.Title;

            if (request.IsActive.HasValue)
                plan.IsActive = request.IsActive.Value;

            plan.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return await GetByIdAsync(id);
        }

        public async Task<MonthlyPlanResponseDto> UpdateImageAsync(Guid id, string newImageUrl)
        {
            var plan = await _context.MonthlyPlans.FindAsync(id);
            if (plan == null)
            {
                return null;
            }

            // TODO: Eski fotoğrafı sil
            // await _fileService.DeleteImageAsync(plan.ImageUrl);

            plan.ImageUrl = newImageUrl;
            plan.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return await GetByIdAsync(id);
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var plan = await _context.MonthlyPlans.FindAsync(id);
            if (plan == null)
            {
                return false;
            }

            // TODO: Fotoğrafı sil
            // await _fileService.DeleteImageAsync(plan.ImageUrl);

            _context.MonthlyPlans.Remove(plan);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> SetActiveStatusAsync(Guid id, bool isActive)
        {
            var plan = await _context.MonthlyPlans.FindAsync(id);
            if (plan == null)
            {
                return false;
            }

            plan.IsActive = isActive;
            plan.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return true;
        }

        // Helper: Entity → Response DTO dönüşümü
        private MonthlyPlanResponseDto MapToResponseDto(MonthlyPlan plan)
        {
            return new MonthlyPlanResponseDto
            {
                Id = plan.Id,
                ClassId = plan.ClassId,
                ClassName = plan.Class != null ? plan.Class.Name : "Bilinmiyor",
                CreatedAt = plan.CreatedAt,
                UpdatedAt = plan.UpdatedAt,
                Year = plan.Year,
                Month = plan.Month,
                MonthName = GetMonthName(plan.Month),
                PlanType = plan.PlanType,
                PlanTypeDisplay = GetPlanTypeDisplay(plan.PlanType),
                Title = plan.Title,
                ImageUrl = plan.ImageUrl,
                IsActive = plan.IsActive
            };
        }

        // Helper: Ay numarasından Türkçe ay adı
        private string GetMonthName(int month)
        {
            var culture = new CultureInfo("tr-TR");
            return culture.DateTimeFormat.GetMonthName(month);
        }

        // Helper: Plan türünü Türkçe göster
        private string GetPlanTypeDisplay(string planType)
        {
            return planType switch
            {
                "MenuPlan" => "Yemek Menüsü",
                "LessonPlan" => "Ders Programı",
                _ => planType
            };
        }
    }
}