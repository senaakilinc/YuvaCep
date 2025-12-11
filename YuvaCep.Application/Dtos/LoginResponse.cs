using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YuvaCep.Application.Dtos
{
    public class LoginResponse
    {
<<<<<<< Updated upstream
        public required string Token { get; set; }//başarılı giriş sonrası appe verilecek anahtar (jwt)
        public required string UserRole { get; set; }//user rolü
        public required string Message { get; set; } //kullanıcıya karşılama mesajı
=======
        public string Token { get; set; } = string.Empty;//başarılı giriş sonrası appe verilecek anahtar (jwt)
        public string UserRole { get; set; } = string.Empty;//user rolü
        public string Message { get; set; } = string.Empty;//kullanıcıya karşılama mesajı
        public bool IsSuccess { get; set; }
>>>>>>> Stashed changes
    }
}
