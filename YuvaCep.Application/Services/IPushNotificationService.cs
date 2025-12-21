using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using YuvaCep.Persistence.Contexts;

namespace YuvaCep.Application.Services
{
    public interface IPushNotificationService
    {
        // Tek bir cihaza bildirim gönder
        Task<bool> SendToDeviceAsync(string deviceToken, string title, string body, Dictionary<string, string> data = null);

        // Birden fazla cihaza bildirim gönder
        Task<int> SendToMultipleDevicesAsync(List<string> deviceTokens, string title, string body, Dictionary<string, string> data = null);

        // Tüm velilere bildirim gönder
        Task<int> SendToAllParentsAsync(string title, string body, Dictionary<string, string> data = null);

        // Belirli sınıfın velilerine bildirim gönder
        Task<int> SendToClassParentsAsync(Guid classId, string title, string body, Dictionary<string, string> data = null);

        // Belirli öğrencinin velisine bildirim gönder
        Task<int> SendToStudentParentAsync(Guid studentId, string title, string body, Dictionary<string, string> data = null);

        // Topic'e abone ol (örn: "all-parents", "class-123")
        Task<bool> SubscribeToTopicAsync(string deviceToken, string topic);

        // Topic'ten abonelikten çık
        Task<bool> UnsubscribeFromTopicAsync(string deviceToken, string topic);
    }

    // Hafta 4'te implementasyon yapılacak
    // Şimdilik mock versiyon kullanılabilir
    public class MockPushNotificationService : IPushNotificationService
    {
        private readonly YuvaCepDbContext _context;

        public MockPushNotificationService(YuvaCepDbContext context)
        {
            _context = context;
        }

        public Task<bool> SendToDeviceAsync(string deviceToken, string title, string body, Dictionary<string, string> data = null)
        {
            Console.WriteLine($"[MOCK] Bildirim gönderildi: {title}");
            return Task.FromResult(true);
        }

        public Task<int> SendToMultipleDevicesAsync(List<string> deviceTokens, string title, string body, Dictionary<string, string> data = null)
        {
            Console.WriteLine($"[MOCK] {deviceTokens.Count} cihaza bildirim gönderildi: {title}");
            return Task.FromResult(deviceTokens.Count);
        }

        public async Task<int> SendToAllParentsAsync(string title, string body, Dictionary<string, string> data = null)
        {
            // Tüm velilerin sayısını bul
            var parentCount = await _context.Parents.CountAsync();
            Console.WriteLine($"[MOCK] Tüm velilere bildirim gönderildi: {title} ({parentCount} veli)");
            return parentCount;
        }

        public async Task<int> SendToClassParentsAsync(Guid classId, string title, string body, Dictionary<string, string> data = null)
        {
            // Sınıftaki öğrencilerin velilerini bul
            var parentIds = await _context.ParentStudent
                .Where(ps => ps.Student.ClassId == classId)
                .Select(ps => ps.ParentId)
                .Distinct()
                .ToListAsync();

            Console.WriteLine($"[MOCK] Sınıf {classId} velilerine bildirim gönderildi: {title} ({parentIds.Count} veli)");
            return parentIds.Count;
        }

        public async Task<int> SendToStudentParentAsync(Guid studentId, string title, string body, Dictionary<string, string> data = null)
        {
            // Öğrencinin velilerini bul (ParentStudent ara tablosu)
            var parentIds = await _context.ParentStudent
                .Where(ps => ps.StudentId == studentId)
                .Select(ps => ps.ParentId)
                .ToListAsync();

            Console.WriteLine($"[MOCK] Öğrenci {studentId} velilerine bildirim gönderildi: {title} ({parentIds.Count} veli)");
            return parentIds.Count;
        }

        public Task<bool> SubscribeToTopicAsync(string deviceToken, string topic)
        {
            Console.WriteLine($"[MOCK] Cihaz {topic} topic'ine abone oldu");
            return Task.FromResult(true);
        }

        public Task<bool> UnsubscribeFromTopicAsync(string deviceToken, string topic)
        {
            Console.WriteLine($"[MOCK] Cihaz {topic} topic'inden çıktı");
            return Task.FromResult(true);
        }
    }
}