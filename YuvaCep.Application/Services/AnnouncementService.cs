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
    public class AnnouncementService : IAnnouncementService
    {
        private readonly YuvaCepDbContext _context;
        private readonly IPushNotificationService _pushNotificationService;

        public AnnouncementService(
            YuvaCepDbContext context,
            IPushNotificationService pushNotificationService)
        {
            _context = context;
            _pushNotificationService = pushNotificationService;
        }

        public async Task<(AnnouncementResponseDto Announcement, NotificationResultDto NotificationResult)> CreateAndNotifyAsync(
            CreateAnnouncementDto request,
            Guid teacherId)
        {
            // 1. Duyuruyu oluştur
            var announcement = await CreateAnnouncementEntity(request, teacherId);

            // 2. Bildirim gönder
            var recipientCount = await SendNotificationAsync(announcement, request);

            // 3. Bildirim durumunu güncelle
            announcement.NotificationSent = true;
            announcement.RecipientCount = recipientCount;
            await _context.SaveChangesAsync();

            var responseDto = await GetByIdAsync(announcement.Id);
            var notificationResult = new NotificationResultDto
            {
                Success = recipientCount > 0,
                RecipientCount = recipientCount,
                Message = recipientCount > 0
                    ? $"{recipientCount} kişiye bildirim gönderildi."
                    : "Bildirim gönderilemedi."
            };

            return (responseDto, notificationResult);
        }

        public async Task<AnnouncementResponseDto> CreateAsync(CreateAnnouncementDto request, Guid teacherId)
        {
            var announcement = await CreateAnnouncementEntity(request, teacherId);
            return await GetByIdAsync(announcement.Id);
        }

        private async Task<Announcement> CreateAnnouncementEntity(CreateAnnouncementDto request, Guid teacherId)
        {
            // Validasyon
            ValidateRequest(request);

            // Öğretmen kontrolü
            var teacherExists = await _context.Teachers.AnyAsync(t => t.Id == teacherId);
            if (!teacherExists)
            {
                throw new Exception("Öğretmen bulunamadı.");
            }

            // Entity oluştur
            var announcement = new Announcement
            {
                Id = Guid.NewGuid(),
                Title = request.Title,
                Content = request.Content,
                TargetAudience = request.TargetAudience,
                ClassId = request.ClassId,
                StudentId = request.StudentId,
                TeacherId = teacherId,
                CreatedAt = DateTime.UtcNow
            };

            _context.Announcements.Add(announcement);
            await _context.SaveChangesAsync();

            return announcement;
        }

        private async Task<int> SendNotificationAsync(Announcement announcement, CreateAnnouncementDto request)
        {
            var title = announcement.Title;
            var body = announcement.Content.Length > 100
                ? announcement.Content.Substring(0, 97) + "..."
                : announcement.Content;

            var data = new Dictionary<string, string>
            {
                { "announcementId", announcement.Id.ToString() },
                { "type", "announcement" }
            };

            int recipientCount = 0;

            switch (request.TargetAudience)
            {
                case "All":
                    recipientCount = await _pushNotificationService.SendToAllParentsAsync(title, body, data);
                    break;

                case "Class":
                    if (!request.ClassId.HasValue)
                        throw new Exception("Sınıf ID'si gerekli.");

                    recipientCount = await _pushNotificationService.SendToClassParentsAsync(
                        request.ClassId.Value, title, body, data);
                    break;

                case "Individual":
                    if (!request.StudentId.HasValue)
                        throw new Exception("Öğrenci ID'si gerekli.");

                    recipientCount = await _pushNotificationService.SendToStudentParentAsync(
                        request.StudentId.Value, title, body, data);
                    break;

                default:
                    throw new Exception("Geçersiz hedef kitle.");
            }

            return recipientCount;
        }

        private void ValidateRequest(CreateAnnouncementDto request)
        {
            if (request.TargetAudience == "Class" && !request.ClassId.HasValue)
            {
                throw new Exception("Sınıf duyurusu için sınıf ID'si gereklidir.");
            }

            if (request.TargetAudience == "Individual" && !request.StudentId.HasValue)
            {
                throw new Exception("Bireysel duyuru için öğrenci ID'si gereklidir.");
            }
        }

        public async Task<AnnouncementResponseDto> GetByIdAsync(Guid id)
        {
            var announcement = await _context.Announcements
                .Include(a => a.Teacher)
                .Include(a => a.Class)
                .Include(a => a.Student)
                .FirstOrDefaultAsync(a => a.Id == id);

            if (announcement == null)
            {
                return null;
            }

            return MapToResponseDto(announcement);
        }

        public async Task<List<AnnouncementResponseDto>> GetAllAsync(int pageNumber = 1, int pageSize = 10)
        {
            var announcements = await _context.Announcements
                .Include(a => a.Teacher)
                .Include(a => a.Class)
                .Include(a => a.Student)
                .OrderByDescending(a => a.CreatedAt)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return announcements.Select(MapToResponseDto).ToList();
        }

        public async Task<List<AnnouncementResponseDto>> GetByTeacherIdAsync(Guid teacherId, int pageNumber = 1, int pageSize = 10)
        {
            var announcements = await _context.Announcements
                .Include(a => a.Teacher)
                .Include(a => a.Class)
                .Include(a => a.Student)
                .Where(a => a.TeacherId == teacherId)
                .OrderByDescending(a => a.CreatedAt)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return announcements.Select(MapToResponseDto).ToList();
        }

        public async Task<List<AnnouncementResponseDto>> GetForParentAsync(Guid parentId, int pageNumber = 1, int pageSize = 10)
        {
            // Velinin çocuklarını ParentStudent ara tablosu üzerinden bul
            var studentIds = await _context.ParentStudent
                .Where(ps => ps.ParentId == parentId)
                .Select(ps => ps.StudentId)
                .ToListAsync();

            if (!studentIds.Any())
            {
                return new List<AnnouncementResponseDto>();
            }

            // Çocukların sınıflarını bul
            var classIds = await _context.Students
                .Where(s => studentIds.Contains(s.Id))
                .Select(s => s.ClassId)
                .Distinct()
                .ToListAsync();

            // Veliye ait duyurular:
            // 1. TargetAudience = "All"
            // 2. TargetAudience = "Individual" ve StudentId velinin çocuklarından biri
            // 3. TargetAudience = "Class" ve ClassId velinin çocuklarının sınıflarından biri
            var announcements = await _context.Announcements
                .Include(a => a.Teacher)
                .Include(a => a.Class)
                .Include(a => a.Student)
                .Where(a =>
                    a.TargetAudience == "All" ||
                    (a.TargetAudience == "Individual" && studentIds.Contains(a.StudentId.Value)) ||
                    (a.TargetAudience == "Class" && classIds.Contains(a.ClassId.Value)))
                .OrderByDescending(a => a.CreatedAt)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return announcements.Select(MapToResponseDto).ToList();
        }

        public async Task<List<AnnouncementResponseDto>> GetByClassIdAsync(Guid classId, int pageNumber = 1, int pageSize = 10)
        {
            var announcements = await _context.Announcements
                .Include(a => a.Teacher)
                .Include(a => a.Class)
                .Include(a => a.Student)
                .Where(a => a.ClassId == classId || a.TargetAudience == "All")
                .OrderByDescending(a => a.CreatedAt)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return announcements.Select(MapToResponseDto).ToList();
        }

        public async Task<List<AnnouncementResponseDto>> GetByStudentIdAsync(Guid studentId, int pageNumber = 1, int pageSize = 10)
        {
            var student = await _context.Students.FindAsync(studentId);
            if (student == null)
            {
                throw new Exception("Öğrenci bulunamadı.");
            }

            var announcements = await _context.Announcements
                .Include(a => a.Teacher)
                .Include(a => a.Class)
                .Include(a => a.Student)
                .Where(a =>
                    a.TargetAudience == "All" ||
                    a.StudentId == studentId ||
                    a.ClassId == student.ClassId)
                .OrderByDescending(a => a.CreatedAt)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return announcements.Select(MapToResponseDto).ToList();
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var announcement = await _context.Announcements.FindAsync(id);
            if (announcement == null)
            {
                return false;
            }

            _context.Announcements.Remove(announcement);
            await _context.SaveChangesAsync();

            return true;
        }

        // Helper: Entity → Response DTO dönüşümü
        private AnnouncementResponseDto MapToResponseDto(Announcement announcement)
        {
            return new AnnouncementResponseDto
            {
                Id = announcement.Id,
                Title = announcement.Title,
                Content = announcement.Content,
                TargetAudience = announcement.TargetAudience,
                TargetAudienceDisplay = GetTargetAudienceDisplay(announcement),
                ClassId = announcement.ClassId,
                ClassName = announcement.Class?.Name,
                StudentId = announcement.StudentId,
                StudentName = announcement.Student != null
                    ? $"{announcement.Student.Name} {announcement.Student.Surname}"
                    : null,
                TeacherId = announcement.TeacherId,
                TeacherName = announcement.Teacher != null
                    ? $"{announcement.Teacher.FirstName} {announcement.Teacher.LastName}"
                    : "Bilinmiyor",
                CreatedAt = announcement.CreatedAt,
                NotificationSent = announcement.NotificationSent,
                RecipientCount = announcement.RecipientCount
            };
        }

        private string GetTargetAudienceDisplay(Announcement announcement)
        {
            return announcement.TargetAudience switch
            {
                "All" => "Tüm Veliler",
                "Class" => announcement.Class != null ? announcement.Class.Name : "Sınıf",
                "Individual" => announcement.Student != null
                    ? $"{announcement.Student.Name} {announcement.Student.Surname}"
                    : "Öğrenci",
                _ => announcement.TargetAudience
            };
        }
    }
}