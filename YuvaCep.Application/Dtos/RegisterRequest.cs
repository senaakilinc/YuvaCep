using System.ComponentModel.DataAnnotations;

namespace YuvaCep.Application.Dtos
{
    public class RegisterRequest
    {
        [Required(ErrorMessage = "Ad zorunludur")]
        [StringLength(50)]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Soyad zorunludur")]
        [StringLength(50)]
        public string LastName { get; set; }

        [Required(ErrorMessage = "TC Kimlik numarası zorunludur")]
        [StringLength(11, MinimumLength = 11)]
        [RegularExpression("^[0-9]{11}$", ErrorMessage = "TC Kimlik numarası 11 haneli olmalıdır")]
        public string TCIDNumber { get; set; }

        [Required(ErrorMessage = "Şifre zorunludur")]
        [StringLength(100, MinimumLength = 6)]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).{6,}$",
            ErrorMessage = "Şifre en az bir büyük harf, küçük harf ve rakam içermelidir")]
        public string Password { get; set; }

        // OPSIYONEL: Rol belirtilmezse Parent
        public string Role { get; set; }

        // Teacher için
        [StringLength(100)]
        public string Subject { get; set; }

        // Parent için
        [Phone]
        public string PhoneNumber { get; set; }

        [StringLength(50)]
        public string Relationship { get; set; }
    }
}