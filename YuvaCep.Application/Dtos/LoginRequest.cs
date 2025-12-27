using System.ComponentModel.DataAnnotations;

namespace YuvaCep.Application.DTOs
{
    public class LoginRequest
    {
        [Required(ErrorMessage = "TC Kimlik No zorunludur.")]
        [StringLength(11, MinimumLength = 11, ErrorMessage = "TC No 11 haneli olmalıdır.")]
        public string TCIDNumber { get; set; }

        [Required(ErrorMessage = "Şifre zorunludur.")]
        public string Password { get; set; }
    }
}