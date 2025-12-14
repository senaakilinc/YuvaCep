using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YuvaCep.Application.Dtos
{
    public class CreateMonthlyPlanDto
    {
        [Required(ErrorMessage = "Sınıf seçilmelidir")]
        public Guid ClassId { get; set; }

        [Required(ErrorMessage = "Yıl girilmelidir")]
        [Range(2020, 2100, ErrorMessage = "Geçerli bir yıl giriniz")]
        public int Year { get; set; }

        [Required(ErrorMessage = "Ay girilmelidir")]
        [Range(1, 12, ErrorMessage = "Ay 1-12 arasında olmalıdır")]
        public int Month { get; set; }

        [Required(ErrorMessage = "Plan türü seçilmelidir")]
        [StringLength(20, ErrorMessage = "Plan türü en fazla 20 karakter olabilir")]
        public string PlanType { get; set; } // "MenuPlan" veya "LessonPlan"

        [Required(ErrorMessage = "Başlık girilmelidir")]
        [StringLength(100, ErrorMessage = "Başlık en fazla 100 karakter olabilir")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Görsel yüklenmeli")]
        public IFormFile ImageFile { get; set; } // Yüklenecek fotoğraf
    }

    public class UpdateMonthlyPlanDto
    {
        [Range(2020, 2100, ErrorMessage = "Geçerli bir yıl giriniz")]
        public int? Year { get; set; }

        [Range(1, 12, ErrorMessage = "Ay 1-12 arasında olmalıdır")]
        public int? Month { get; set; }

        [StringLength(20, ErrorMessage = "Plan türü en fazla 20 karakter olabilir")]
        public string PlanType { get; set; }

        [StringLength(100, ErrorMessage = "Başlık en fazla 100 karakter olabilir")]
        public string Title { get; set; }

        public bool? IsActive { get; set; }
    }

    public class UploadImageDto
    {
        [Required(ErrorMessage = "Görsel yüklenmeli")]
        public IFormFile ImageFile { get; set; }
    }

}
