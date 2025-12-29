using System;
using YuvaCep.Domain.Enums;

namespace YuvaCep.Domain.Entities
{
    public class DailyReport
    {
        public Guid Id { get; set; }
        public DateTime Date { get; set; } = DateTime.UtcNow;

        // Rapor Kime Ait?
        public Guid StudentId { get; set; }
        public Student Student { get; set; }

        // --- 1. KART: MOD (Günü Nasıl Geçti?) ---
        public MoodStatus Mood { get; set; }
        public string MoodNote { get; set; } = string.Empty; 

        // --- 2. KART: BESLENME ---
        public FoodStatus Breakfast { get; set; }
        public FoodStatus Lunch { get; set; }
        public string FoodNote { get; set; } = string.Empty;

        // --- 3. KART: UYKU ---
        public SleepStatus Sleep { get; set; }

        // --- 4. KART: ETKİNLİK (YENİ) ---
        public ActivityStatus Activity { get; set; }
        public string ActivityNote { get; set; } = string.Empty; 

        // --- 5. KART: GENEL DEĞERLENDİRME ---
        public string TeacherNote { get; set; } = string.Empty; 
    }
}