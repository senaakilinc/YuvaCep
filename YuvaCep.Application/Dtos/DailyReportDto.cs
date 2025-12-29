using System;
using YuvaCep.Domain.Enums;

namespace YuvaCep.Application.Dtos
{
    public class DailyReportDto
    {
        public Guid Id { get; set; }
        public DateTime Date { get; set; }
        public string StudentName { get; set; }

        // 1. Mod Kartı
        public MoodStatus Mood { get; set; }
        public string MoodNote { get; set; }

        // 2. Beslenme Kartı
        public FoodStatus Breakfast { get; set; }
        public FoodStatus Lunch { get; set; }
        public string FoodNote { get; set; }

        // 3. Uyku Kartı
        public SleepStatus Sleep { get; set; }

        // 4. Etkinlik Kartı
        public ActivityStatus Activity { get; set; }
        public string ActivityNote { get; set; }

        // 5. Genel Değerlendirme
        public string TeacherNote { get; set; }
    }
}