using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YuvaCep.Application.Dtos
{
    public class CreateDailyReportDto
    {
        [Required(ErrorMessage = "Öğrenci seçilmelidir")]
        public Guid StudentId { get; set; }

        [StringLength(500, ErrorMessage = "Kahvaltı notu en fazla 500 karakter olabilir")]
        public string BreakfastNotes { get; set; }

        [StringLength(500, ErrorMessage = "Öğle yemeği notu en fazla 500 karakter olabilir")]
        public string LunchNotes { get; set; }

        [StringLength(500, ErrorMessage = "Atıştırma notu en fazla 500 karakter olabilir")]
        public string SnackNotes { get; set; }

        public bool AteWell { get; set; }

        [StringLength(1000, ErrorMessage = "Aktivite notu en fazla 1000 karakter olabilir")]
        public string ActivityNotes { get; set; }

        [StringLength(200, ErrorMessage = "Aktivite türü en fazla 200 karakter olabilir")]
        public string ActivityType { get; set; }

        [StringLength(50, ErrorMessage = "Ruh hali en fazla 50 karakter olabilir")]
        public string MoodStatus { get; set; }

        [StringLength(1000, ErrorMessage = "Davranış notu en fazla 1000 karakter olabilir")]
        public string BehaviorNotes { get; set; }

        [Range(0, 10, ErrorMessage = "Davranış skoru 0-10 arasında olmalıdır")]
        public int BehaviorScore { get; set; }

        public bool NapTaken { get; set; }

        [Range(0, 300, ErrorMessage = "Uyku süresi 0-300 dakika arası olmalıdır")]
        public int? NapDurationMinutes { get; set; }

        public bool ToiletUsed { get; set; }

        [StringLength(2000, ErrorMessage = "Genel notlar en fazla 2000 karakter olabilir")]
        public string GeneralNotes { get; set; }

        [StringLength(500, ErrorMessage = "Beslenme notu en fazla 500 karakter olabilir")]
        public string NutritionNotes { get; set; }
    }
}
