using System;
using System.ComponentModel.DataAnnotations;

namespace YuvaCep.Domain.Entities
{
    public class DailyReport
    {
        public Guid Id { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
        public string NutritionNotes { get; set; } = string.Empty;
        public int BehaviorScore { get; set; }

        public Guid StudentId { get; set; }
        public Student Student { get; set; } = null!;

        public Guid TeacherId { get; set; }
        public Teacher Teacher { get; set; } = null!;

        //Beslenme bilgileri
        [StringLength(500)]
        public string BreakfastNotes { get; set; }

        [StringLength(500)]
        public string LunchNotes { get; set; }

        [StringLength(500)]
        public string SnackNotes { get; set; }

        public bool AteWell {  get; set; }

        //AktiviteBilgileri
        [StringLength(1000)]
        public string ActivityNotes { get; set; }

        [StringLength(200)]
        public string ActivityType {  get; set; }

        //Duygu ve Davranış
        [StringLength(50)]
        public string MoodStatus { get; set; }

        [StringLength(1000)]
        public string BehaviorNotes { get; set; }

        //Uyku ve Hijyen
        public bool NapTaken { get; set; }
        public int? NapDurationMinutes { get; set; }
        public bool ToiletUsed { get; set; }

        //Öğretmen bilgisi gün sonu gibi
        [StringLength(2000)]
        public string GeneralNotes { get; set; }
    }
}