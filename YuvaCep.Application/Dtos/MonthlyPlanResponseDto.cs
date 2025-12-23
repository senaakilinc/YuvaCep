using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YuvaCep.Application.Dtos
{
    public class MonthlyPlanResponseDto
    {
        public Guid Id { get; set; }
        public Guid ClassId { get; set; }
        public string ClassName { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public int Year { get; set; }
        public int Month { get; set; }
        public string MonthName { get; set; } // "Ocak", "Şubat", vb.
        public string PlanType { get; set; }
        public string PlanTypeDisplay { get; set; } // "Yemek Menüsü", "Ders Programı"
        public string Title { get; set; }
        public string ImageUrl { get; set; }
        public bool IsActive { get; set; }

        // Helper property
        public string PeriodDisplay => $"{MonthName} {Year}";
    }
}
