using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YuvaCep.Application.Dtos
{
    public class LoginRequest
    {
        //sisteme tc ve şifre ile girer veli
        public string TCIDNumber { get; set; }
        public string Password { get; set; }
    }
}
