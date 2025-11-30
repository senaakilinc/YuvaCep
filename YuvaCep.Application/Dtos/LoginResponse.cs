using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YuvaCep.Application.Dtos
{
    public class LoginResponse
    {
        public string Token { get; set; }//başarılı giriş sonrası appe verilecek anahtar (jwt)
        public string UserRole { get; set; }//user rolü
        public string Message { get; set; }//kullanıcıya karşılama mesajı
    }
}
