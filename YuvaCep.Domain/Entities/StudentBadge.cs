using System;

namespace YuvaCep.Domain.Entities
{
    public class StudentBadge
    {
        public Guid Id { get; set; }
        public DateTime EarnedAt { get; set; } = DateTime.UtcNow;

        // Kime verildi?
        public Guid StudentId { get; set; }
        public Student Student { get; set; } = null!;

        // Hangi rozet verildi?
        public Guid BadgeId { get; set; }
        public Badge Badge { get; set; } = null!;
    }
}