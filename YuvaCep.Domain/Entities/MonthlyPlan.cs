using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YuvaCep.Domain.Entities
{
    public class MonthlyPlan
    {
        public Guid Id { get; set; }
        public Guid ClassId { get; set; }
        public Class Class {  get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }

        public int Year { get; set; }

        [Range(1,12)]
        public int Month {  get; set; }

        // Plan Türü: "MenuPlan" veya "LessonPlan"
        [StringLength(20)]
        public string PlanType { get; set; }

        [StringLength(100)]
        public string Title { get; set; }

        // Fotoğraf URL'i - S3, Azure Blob, veya local storage
        [StringLength(500)]
        public string ImageUrl { get; set; }
        //TODO teacher entitysini de ekle ama nasıl yapacağına bak

        public bool IsActive { get; set; } = true;
    }
}
