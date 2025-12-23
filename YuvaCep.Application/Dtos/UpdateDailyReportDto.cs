using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YuvaCep.Application.Dtos
{
    public class UpdateDailyReportDto
    {
        [StringLength(500)]
        public string BreakfastNotes { get; set; }

        [StringLength(500)]
        public string LunchNotes { get; set; }

        [StringLength(500)]
        public string SnackNotes { get; set; }

        public bool AteWell { get; set; }

        [StringLength(1000)]
        public string ActivityNotes { get; set; }

        [StringLength(200)]
        public string ActivityType { get; set; }

        [StringLength(50)]
        public string MoodStatus { get; set; }

        [StringLength(1000)]
        public string BehaviorNotes { get; set; }

        [Range(0, 10)]
        public int BehaviorScore { get; set; }

        public bool NapTaken { get; set; }

        [Range(0, 300)]
        public int? NapDurationMinutes { get; set; }

        public bool ToiletUsed { get; set; }

        [StringLength(2000)]
        public string GeneralNotes { get; set; }

        [StringLength(500)]
        public string NutritionNotes { get; set; }
    }
}
