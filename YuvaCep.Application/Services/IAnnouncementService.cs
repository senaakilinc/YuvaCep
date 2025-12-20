using YuvaCep.Application.Dtos;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace YuvaCep.Application.Services
{
    public interface IAnnouncementService
    {
        // Duyuru oluştur ve bildirim gönder
        Task<(AnnouncementResponseDto Announcement, NotificationResultDto NotificationResult)> CreateAndNotifyAsync(
            CreateAnnouncementDto request,
            Guid teacherId);

        // Duyuru oluştur (bildirim göndermeden)
        Task<AnnouncementResponseDto> CreateAsync(CreateAnnouncementDto request, Guid teacherId);

        Task<AnnouncementResponseDto> GetByIdAsync(Guid id);

        Task<List<AnnouncementResponseDto>> GetAllAsync(int pageNumber = 1, int pageSize = 10);

        // Öğretmenin duyuruları
        Task<List<AnnouncementResponseDto>> GetByTeacherIdAsync(Guid teacherId, int pageNumber = 1, int pageSize = 10);

        // Veli için duyuruları getir (kendi çocuklarına ve "All" hedefli olanlar)
        Task<List<AnnouncementResponseDto>> GetForParentAsync(Guid parentId, int pageNumber = 1, int pageSize = 10);

        // Belirli sınıfa ait duyurular
        Task<List<AnnouncementResponseDto>> GetByClassIdAsync(Guid classId, int pageNumber = 1, int pageSize = 10);

        // Belirli öğrenciye ait duyurular
        Task<List<AnnouncementResponseDto>> GetByStudentIdAsync(Guid studentId, int pageNumber = 1, int pageSize = 10);

        Task<bool> DeleteAsync(Guid id);
    }
}