using System;

namespace YuvaCep.Domain.Entities
{
    public class DailyReport
    {
        public Guid Id { get; set; }
        public DateTime Date { get; set; }
        public string NutritionNotes { get; set; } = string.Empty;
        public int BehaviorScore { get; set; }

        public Guid ChildId { get; set; }
        public Child Child { get; set; }

        public Guid TeacherId { get; set; }
        public Teacher Teacher { get; set; }
    }
}