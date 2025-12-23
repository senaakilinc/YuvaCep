using System;

namespace YuvaCep.Domain.Entities
{
    public class StudentMood
    {
        public Guid Id { get; set; }
        public string Emoji { get; set; } = string.Empty; // Örn: "😊"
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // İlişki: Hangi Öğrenci?
        public Guid StudentId { get; set; }
        public Student Student { get; set; } = null!;
    }
}