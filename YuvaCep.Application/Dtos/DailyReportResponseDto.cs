using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YuvaCep.Application.Dtos
{
    public class DailyReportResponseDto
    {
        public Guid Id { get; set; }
        public Guid StudentId { get; set; }
        public string StudentName { get; set; }
        public Guid TeacherId { get; set; }
        public string TeacherName { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        // Beslenme
        public string BreakfastNotes { get; set; }
        public string LunchNotes { get; set; }
        public string SnackNotes { get; set; }
        public bool AteWell { get; set; }
        public string NutritionNotes { get; set; }

        // Aktivite
        public string ActivityNotes { get; set; }
        public string ActivityType { get; set; }

        // Duygu ve Davranış
        public string MoodStatus { get; set; }
        public string BehaviorNotes { get; set; }
        public int BehaviorScore { get; set; }

        // Uyku ve Hijyen
        public bool NapTaken { get; set; }
        public int? NapDurationMinutes { get; set; }
        public bool ToiletUsed { get; set; }

        // Genel
        public string GeneralNotes { get; set; }
    }
}
