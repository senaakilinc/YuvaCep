using System;
using YuvaCep.Mobile.Enums; // Enumları buradan çekecek

namespace YuvaCep.Mobile.Dtos
{
    public class DailyReportDto
    {
        public Guid Id { get; set; }
        public DateTime Date { get; set; }
        public string StudentName { get; set; }

        // --- 1. MOD ---
        public MoodStatus Mood { get; set; }
        public string MoodNote { get; set; }

        // --- 2. YEMEK ---
        public FoodStatus Breakfast { get; set; }
        public FoodStatus Lunch { get; set; }
        public string FoodNote { get; set; }

        // --- 3. UYKU ---
        public SleepStatus Sleep { get; set; }

        // --- 4. ETKİNLİK ---
        public ActivityStatus Activity { get; set; }
        public string ActivityNote { get; set; }

        // --- 5. GENEL ---
        public string TeacherNote { get; set; }
    }
}