using System;

namespace YuvaCep.Domain.Entities
{
    public class Announcement
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }

        // 3 farklı hedef kitle seçeneği
        public string TargetAudience { get; set; } // "All", "Class", "Individual"

        // Class için
        public Guid? ClassId { get; set; }
        public Class Class { get; set; }

        // Individual için
        public Guid? StudentId { get; set; }
        public Student Student { get; set; }

        // Kim yaptı?
        public Guid TeacherId { get; set; }
        public Teacher Teacher { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Bildirim gönderildi mi?
        public bool NotificationSent { get; set; } = false;
        public int? RecipientCount { get; set; }
    }
    public static class TargetAudienceType
    {
        public const string All = "All";           // Tüm veliler
        public const string Class = "Class";       // Belirli bir sınıf
        public const string Individual = "Individual"; // Tek bir öğrenci/veli
    }
}