using System.ComponentModel.DataAnnotations;

namespace YuvaCep.Api.DTOs
{
    public class CreateStudentDto
    {
        [Required(ErrorMessage = "Ad zorunludur")]
        [MaxLength(50)]
        public string Name { get; set; }

        [Required(ErrorMessage = "Soyad zorunludur")]
        [MaxLength(50)]
        public string Surname { get; set; }

        [Required(ErrorMessage = "TC Kimlik No zorunludur")]
        [StringLength(11, MinimumLength = 11, ErrorMessage = "TC Kimlik No 11 karakter olmalıdır")]
        public string TCIDNumber { get; set; }

        [Required(ErrorMessage = "Referans kodu zorunludur")]
        [MaxLength(20)]
        public string ReferenceCode { get; set; }

        [Required(ErrorMessage = "Doğum tarihi zorunludur")]
        public DateTime DateOfBirth { get; set; }

        [MaxLength(500)]
        public string? HealthNotes { get; set; }

        [Required(ErrorMessage = "Sınıf seçimi zorunludur")]
        public Guid ClassId { get; set; }
    }
}