using System;

namespace YuvaCep.Domain.Entities
{
    public class DailyReport
    {
        public Guid Id { get; set; }
        public DateTime Date { get; set; }
        public string NutritionNotes { get; set; } = string.Empty;
        public int BehaviorScore { get; set; }

        public Guid StudentId { get; set; }
        public Student Student { get; set; } = null!;

        public Guid TeacherId { get; set; }
        public Teacher Teacher { get; set; } = null!;
    }
}