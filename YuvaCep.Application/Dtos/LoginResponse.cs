using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YuvaCep.Application.Dtos
{
    public class LoginResponse
    {
        public required string Token { get; set; }//başarılı giriş sonrası appe verilecek anahtar (jwt)
        public required string UserRole { get; set; }//user rolü
        public required string Message { get; set; } //kullanıcıya karşılama mesajı
    }
}
